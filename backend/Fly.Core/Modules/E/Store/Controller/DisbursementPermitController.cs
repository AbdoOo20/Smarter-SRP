using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.E.Store.Controller;

public class DisbursementPermitController
    : CustomControllerFeatrueBased<DtoDisbursementPermit,
            TDisbursementPermit, DisbursementPermitQueryRequest,
            DeleteRequestBase, CreateDisbursementPermitRequest, UpdateDisbursementPermitRequest>
        , IDisbursementPermitFeatures
        , IDisposable

{
    public DisbursementPermitController(
        ILocalizationService localizationService, ILogger<DisbursementPermitController> logger,
        IMediator mediator,
        IServiceBase<DtoDisbursementPermit, TDisbursementPermit> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreate += DisbursementPermitController_OnCreate;
        OnUpdate += DisbursementPermitController_OnUpdate;
    }

    [NonAction]
    private async ValueTask<DtoDisbursementPermit> DisbursementPermitController_OnUpdate(UpdateDisbursementPermitRequest q)
    {
        var storageModuleService = _service.MakeChildService<IStorageModuleService>();
        var detService = _service.MakeChildService<DtoDisbursementPermitDetail, TDisbursementPermitDetail>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var permit = q.Adapt<DtoDisbursementPermit>();
        //
        var a = _service.Repositry.DatabaseAccesorId;
        var b = detService.Repositry.DatabaseAccesorId;
        //
        //
        //
        var tr = _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        storageModuleService.SetTransaction(tr);
        detService.BeginTransaction(tr);
        //
        var old = await detService.GetAllAsAsync(s => new { s.ItemId, s.UnitId, s.Quantity }, w => w.DisbursementPermitId == permit.Id);
        var selectedUnits = permit.DisbursementPermitDetails
            .Join(old.Items,
                detail => new { detail.ItemId, detail.UnitId },
                item => new { item.ItemId, item.UnitId },
                (detail, item) => new { detail.ItemId, detail.UnitId })
                .ToArray();
        var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
               unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
               unit => selectedUnits.Any(selected => unit.UnitId == selected.UnitId && unit.ItemId == selected.ItemId)
           )).Items;
        //
        foreach (var item in old.Items)
        {
            var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
            var itmId = item.ItemId;
            var untId = item.UnitId;
            var strId = permit.StoreId;
            var qun = item.Quantity;
            var allOnes = selectedOne.TransferRate * qun;

            await storageModuleService.RegainItem(itmId, untId, strId, qun, allOnes, permit.Id, RelatedInventory.Disbursement_Return);
        }
        await detService.Remove(w => w.DisbursementPermitId == permit.Id);
        //
        var db = await _service.GetById(permit.Id);
        permit.Adapt(db);
        await _service.Update(db);
        if (permit.DisbursementPermitDetails?.Any() == true)
        {
            foreach (var item in permit.DisbursementPermitDetails)
            {
                var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = permit.StoreId;
                var qun = item.Quantity;
                var allOnes = selectedOne.TransferRate * qun;
                await storageModuleService.LoseItem(itmId, untId, strId, qun, allOnes, permit.Id, RelatedInventory.Disbursement);
            }
        }
        _service.CommitTransaction();
        return permit;
    }

    [NonAction]
    private async ValueTask<DtoDisbursementPermit> DisbursementPermitController_OnCreate(CreateDisbursementPermitRequest q)
    {

        //  VALIDATION

        q.Id = Identifier.Empty;
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var storageModuleService = _service.MakeChildService<IStorageModuleService>();
        var permit = q.Adapt<DtoDisbursementPermit>();
        //
        //
        //
        var selectedItems = new List<Identifier>();
        var selectedUnitsx = new List<Identifier>();
        permit.DisbursementPermitDetails.Distinct().ForEach(f =>
        {
            selectedItems.Add(f.ItemId);
            selectedUnitsx.Add(f.UnitId);
        });
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        //
        //
        await _service.Add(permit);
        var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
            unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
            unit => selectedItems.Contains(unit.ItemId) & selectedUnitsx.Contains(unit.UnitId)
        )).Items;

        //var selectedUnits = permit.DisbursementPermitDetails
        //       .Select(detail => new { detail.ItemId, detail.UnitId })
        //       .ToArray();
        //var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
        //                unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
        //                unit => selectedUnits.Any(selected => unit.UnitId == selected.UnitId && unit.ItemId == selected.ItemId)
        //            )).Items;
        //
        //
        //
        if (permit.DisbursementPermitDetails?.Any() == true)
        {
            foreach (var item in permit.DisbursementPermitDetails)
            {
                var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = permit.StoreId;
                var qun = item.Quantity;
                var allOnes = selectedOne.TransferRate * qun;
                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name }, w => w.Id == itmId);
                var canLose = await storageModuleService.CanLoseItemQuntity(itmId, untId, strId, qun);
                if (!canLose) throw new Exception($"Can not disbursement the item {itemInfo.Name}.");
                await storageModuleService.LoseItem(itmId, untId, strId, qun, allOnes, permit.Id, RelatedInventory.Disbursement);
            }
        }
        _service.CommitTransaction();
        return permit;
    }

    [HttpGet("stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStore([FromQuery] StoreQueryRequest request)
        =>
            GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("costcente")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCostcenter([FromQuery] SupplierQueryRequest request)
        =>
            GetSelectableList<DtoSupplier, TCostCenter>(x => x.Name);

    [HttpGet("items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        =>
            GetSelectableList<DtoItem, TItem>(x => x.Name + " - " + x.Barcode, y => y.IsActive);

    [HttpGet("units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));

    [HttpPost("ItemInfo")]
    public ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> GetItemInfoQuery([FromBody] GetItemInfoQueryRequest queryRequest)
    =>
        TryOne<ItemInfoQueryResponse>(async opt =>
        {
            var request = this.Request;
            var storageService = opt.GetService<IStorageModuleService>();

            var inventoryInfo = await storageService.GetItemQuntity(queryRequest.ItemId, queryRequest.UnitId, queryRequest.StoreId);

            return opt.Success(new ItemInfoQueryResponse()
            {
                Price = 0,
                Quntity = inventoryInfo.Quantity,
                AdditionalQuntity = inventoryInfo.AdditionalQuantity,
            });

        });

    public virtual void Dispose()
    {
        OnCreate -= DisbursementPermitController_OnCreate;
        OnUpdate -= DisbursementPermitController_OnUpdate;
    }
}