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
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.E.Store.Controller;

public class SupplyPermitController
    : CustomControllerFeatrueBased<DtoSupplyPermit, TSupplyPermit, SupplyPermitQueryRequest, DeleteRequestBase, CreateUpdateSupplyPermitRequest>
        , ISupplyPermitFeatures
        , IDisposable
{
    public SupplyPermitController(
        ILocalizationService localizationService,
        ILogger<SupplyPermitController> logger,
        IMediator mediator,
        IServiceBase<DtoSupplyPermit, TSupplyPermit> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += OnOnCreateUpdate;
    }

    [NonAction]
    private async ValueTask<DtoSupplyPermit> OnOnCreateUpdate(CreateUpdateSupplyPermitRequest q)
    {
        //
        var isNew = q.Id == Identifier.Empty;
        //
        var permit = q.Adapt<DtoSupplyPermit>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var det = q.SupplyPermitDetails.Adapt<List<DtoSupplyPermitDetail>>();
        var storageModuleService = _service.MakeChildService<IStorageModuleService>();
        var detService = _service.MakeChildService<DtoSupplyPermitDetail, TSupplyPermitDetail>();
        //
        var selectedItems = new List<Identifier>();
        var selectedUnitsx = new List<Identifier>();
        permit.SupplyPermitDetails.Distinct().ForEach(f =>
        {
            selectedItems.Add(f.ItemId);
            selectedUnitsx.Add(f.UnitId);
        });
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        //
        if (isNew)
        {
            await _service.Add(permit);
            //var selectedUnits = permit.SupplyPermitDetails
            //   .Select(detail => new { detail.ItemId, detail.UnitId })
            //   .ToArray();
            //var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
            //       unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
            //       unit => selectedUnits.Any(selected => unit.UnitId == selected.UnitId && unit.ItemId == selected.ItemId)
            //   )).Items;
            var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
            unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
            unit => selectedItems.Contains(unit.ItemId) & selectedUnitsx.Contains(unit.UnitId)
        )).Items;
            if (permit.SupplyPermitDetails?.Any() == true)
            {
                foreach (var item in permit.SupplyPermitDetails)
                {
                    var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                    var itmId = item.ItemId;
                    var untId = item.UnitId;
                    var strId = permit.StoreId;
                    var qun = item.Quantity;
                    var allOnes = selectedOne.TransferRate * qun;
                    await storageModuleService.RegainItem(itmId, untId, strId, qun, allOnes, permit.Id, RelatedInventory.Supply);
                }
            }
        }
        else
        {
            var old = (await detService.GetAllAsAsync(s => new { s.ItemId, s.UnitId, s.Quantity }, w => w.SupplyPermitId == permit.Id)).Items.ToList();
            var db = await _service.GetById(permit.Id);
            var oldStoreId = db.StoreId;

            // Load unit conversion rates for both the old and the new lines. (The previous
            // composite `selectedUnits.Any(...)` filter could not be translated to SQL, so
            // every edit failed; it also skipped lines that were newly added on edit.)
            var unitItemIds = old.Select(x => x.ItemId)
                .Concat(permit.SupplyPermitDetails?.Select(x => x.ItemId) ?? [])
                .Distinct().ToList();
            var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
                   unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
                   unit => unitItemIds.Contains(unit.ItemId)
               )).Items.ToList();

            // Reverse what the permit added before: the OLD lines, from the OLD store.
            foreach (var item in old)
            {
                var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                var allOnes = selectedOne.TransferRate * item.Quantity;
                await storageModuleService.LoseItem(item.ItemId, item.UnitId, oldStoreId, item.Quantity, allOnes, permit.Id, RelatedInventory.Supply_Return);
            }
            // Replace the lines: the old code only removed them and never saved the new
            // ones, so an edited permit lost all of its lines.
            await detService.Remove(w => w.SupplyPermitId == permit.Id, false);
            var newLines = (permit.SupplyPermitDetails ?? []).ToList();
            newLines.ForEach(f => f.SupplyPermitId = permit.Id);
            if (newLines.Count > 0)
                await detService.AddRange(newLines);
            permit.Adapt(db);
            await _service.Update(permit);
            if (permit.SupplyPermitDetails?.Any() == true)
            {
                foreach (var item in permit.SupplyPermitDetails)
                {
                    var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                    var itmId = item.ItemId;
                    var untId = item.UnitId;
                    var strId = permit.StoreId;
                    var qun = item.Quantity;
                    var allOnes = selectedOne.TransferRate * qun;
                    await storageModuleService.RegainItem(itmId, untId, strId, qun, allOnes, permit.Id, RelatedInventory.Supply);
                }
            }
        }
        //  
        _service.CommitTransaction();
        return permit;
    }

    /// <summary>
    /// Deleting a permit must undo its stock movements. Every movement it ever made
    /// (including the reverse/re-apply pairs written on edits) carries its id as
    /// RelatedId, so removing them returns the stock to how it was before the permit.
    /// </summary>
    protected override async ValueTask AfterDelete(DtoSupplyPermit dto, DeleteRequestBase command, Framework.Server.Common.ResultOrFailer<DtoSupplyPermit?> opt)
    {
        var inventoryService = _service.MakeChildService<DtoInventory, TInventory>();
        var inventoryCostService = _service.MakeChildService<DtoInventoryCost, TInventoryCost>();
        var detService = _service.MakeChildService<DtoSupplyPermitDetail, TSupplyPermitDetail>();
        var ids = (await inventoryService.GetAllAsAsync(s => s.Id,
            w => w.RelatedId == dto.Id && (w.RelatedIndex == RelatedInventory.Supply || w.RelatedIndex == RelatedInventory.Supply_Return))).Items.ToList();
        if (ids.Count > 0)
        {
            await inventoryCostService.Remove(w => ids.Contains(w.TInventoryId));
            await inventoryService.Remove(w => ids.Contains(w.Id));
        }
        await detService.Remove(w => w.SupplyPermitId == dto.Id);
    }



    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => x.Name, y => y.IsActive);


    [HttpGet("stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStore([FromQuery] StoreQueryRequest request)
        =>
            GetSelectableList<DtoStore, TStore>(x => x.Name);


    [HttpGet("units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));


    [HttpGet("suppliers")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSuppliers([FromQuery] SupplierQueryRequest request)
        =>
            GetSelectableList<DtoSupplier, TSupplier>(x => x.Name);

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
        OnCreateUpdate -= OnOnCreateUpdate;
    }
}