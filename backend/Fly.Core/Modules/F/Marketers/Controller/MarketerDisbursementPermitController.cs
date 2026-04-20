using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Framework.Shared.Common;
using Mapster;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.Marketers.Features;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Requests;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using System.Linq.Expressions;
using LinqKit;
using Fly.Framework.Server.Common.Localization;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using System.Data;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Microsoft.Extensions.Configuration;

namespace Fly.Core.Modules.F.Marketers.Controller;

public class MarketerDisbursementPermitController
    : CustomControllerFeatrueBased<DtoMarketerDisbursementPermit,
            TMarketerDisbursementPermit, MarketerDisbursementPermitQueryRequest,
            MarketerDisbursementPermitDeleteRequestBase, MarketerDisbursementPermitCreateUpdateRequest>
        , IMarketerDisbursementPermitFeatures
        , IDisposable
{
    RelatedInventory relatedInventory = RelatedInventory.MarketerDisbursement;
    private readonly IConfiguration configuration;

    public MarketerDisbursementPermitController(
        ILocalizationService localizationService, ILogger<MarketerDisbursementPermitController> logger,
        IMediator mediator,
        IServiceBase<DtoMarketerDisbursementPermit, TMarketerDisbursementPermit> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster, IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += MarketerDisbursementPermitController_OnCreate;
        OnDeleteEx += MarketerDisbursementPermitController_OnDeleteEx;
        this.configuration = configuration;
    }

    private async ValueTask<HttpResponseWrapper<DtoMarketerDisbursementPermit?>> MarketerDisbursementPermitController_OnDeleteEx(MarketerDisbursementPermitDeleteRequestBase q, Framework.Server.Common.ResultOrFailer<DtoMarketerDisbursementPermit?> _)
    {
        var sellInvoiceService = _service.MakeChildService<DtoSalesInvoice, TSalesInvoice>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var sellreturnDetailsService = _service.MakeChildService<DtoDisbursementPermitDetail, TDisbursementPermitDetail>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        //
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        var returnInvoice = await _service.GetById(q.Id);
        await sellreturnDetailsService.Remove(w => w.DisbursementPermitId == returnInvoice.Id);
        await marketerModuleService.LossAllEver(returnInvoice.Id, relatedInventory);
        await storageModuleService.LossAllEver(returnInvoice.Id, [relatedInventory]);
        //
        await _service.Remove(returnInvoice.Id);
        var status = _service.CommitTransaction();
        if (!status)
            return _.Problem("cannot proccesed the sale opration");
        return _.Success(returnInvoice)!;
    }

    [NonAction]
    public override Expression<Func<DtoMarketerDisbursementPermit, bool>> GetWhere(Expression<Func<DtoMarketerDisbursementPermit, bool>> expression, MarketerDisbursementPermitQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Details.Any(d => d.UnitName.Contains(query.SearchText)) |
                x.Details.Any(d => d.ItemName.Contains(query.SearchText)) |
                x.Number.ToString().Contains(query.SearchText) |
                x.SerialNumber.ToString().Contains(query.SearchText) |
                x.StoreName.Contains(query.SearchText) |
                x.MarketerName.Contains(query.SearchText)
            );
        return base.GetWhere(expression, query);
    }

    [NonAction]
    private async ValueTask<DtoMarketerDisbursementPermit> MarketerDisbursementPermitController_OnCreate(MarketerDisbursementPermitCreateUpdateRequest q)
    {
        //
        //
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var marketerSrvice = _service.MakeChildService<DtoMarketer, TMarketer>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var storageModuleService = _service.MakeChildService<IStorageModuleService>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var marketerSrviceDetails = _service.MakeChildService<DtoMarketerDisbursementPermitDetail, TMarketerDisbursementPermitDetail>();
        //
        var disbursement = q.Adapt<DtoMarketerDisbursementPermit>();
        //
        DtoAccount? saleInvoiceBaseAccount = null;
        var selectedItems = new List<Identifier>();
        var selectedUnits = new List<Identifier>();
        //
        var total = disbursement.Details.Sum(s => s.Quantity * s.Price);
        disbursement.Details.Distinct().ForEach(f =>
        {
            selectedItems.Add(f.ItemId);
            selectedUnits.Add(f.UnitId);
        });
        //
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        //
        var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
            unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
            unit => selectedItems.Contains(unit.ItemId) & selectedUnits.Contains(unit.UnitId)
        )).Items;
        //
        //
        long invoiceNumber = 0;
        //
        //
        async Task InventiryProcess(IEnumerable<DtoMarketerDisbursementPermitDetail> invoiceDetails, bool isLose = false, bool softDelete = true)
        {
            foreach (var item in invoiceDetails)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = disbursement.StoreId;
                var markId = disbursement.MarketerId;
                var qun = item.Quantity;

                StorageItemProperties storageItemProperties = new()
                {
                    ExpireDate = item.ExpireDate,
                };

                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                if (selectedOne is null)
                {
                    Logger.LogWarning("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }
                //var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);
                if (!isLose)
                {
                    if (configuration.GetRequiredSection("MinusSellCheck").Get<bool>())
                    {
                        var quntityInfo = await storageModuleService.CanLoseItemQuntityInfo(itmId, untId, strId, qun);
                        var errQaun = (quntityInfo.Info?.Quantity is not null) ? "أقل من " +
                            quntityInfo.Info?.Quantity.ToString() : " غير متوفرة";
                        if (!quntityInfo.CanLoost)
                            throw new InvalidInputException($"لا يمكن بيع الصنف " +
                                $" {itemInfo?.Name ?? "بدون اسم"}. " +
                                $"الكمية المطلوبة" +
                                $"{qun} " +
                                $" {errQaun}");
                    }
                }
                var allOnes = selectedOne.TransferRate * qun;
                await storageModuleService.RegainOrLoseItem(itmId, untId, strId, qun, allOnes,
                    relatedId: disbursement.Id,
                    relatedInventory: relatedInventory,
                    isLose: !isLose,
                    softDelete: softDelete,
                    storageItemProperties: storageItemProperties
                );
                await marketerModuleService.RegainOrLoseItem(itmId, untId, markId, qun, allOnes,
                    relatedId: disbursement.Id,
                    relatedInventory: relatedInventory,
                    isLose: isLose,
                    softDelete: softDelete,
                    storageItemProperties: storageItemProperties
                );
            }
        }
        //
        //
        //
        if (q.IsNew())
        {
            //
            disbursement.Date = DateTime.Now;
            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            disbursement.Number = invoiceNumber;
            await _service.Add(disbursement);
            //
            //  Items
            if (disbursement.Details?.Any() == true)
            {
                await InventiryProcess(disbursement.Details);
            }
            //
        }
        else
        {
            invoiceNumber = disbursement.Number;
            //NEW 
            Logger.LogTrace("removing details {0} for id {1}", nameof(DtoDisbursementPermit), disbursement.Id);
            await marketerSrviceDetails.Remove(w => w.MarketerDisbursementPermitId == disbursement.Id);
            await storageModuleService.LossAllEver(disbursement.Id, RelatedInventory.MarketerDisbursement);
            await marketerModuleService.LossAllEver(disbursement.Id, RelatedInventory.MarketerDisbursement);
            //
            await _service.Update(disbursement);
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoMarketerDisbursementPermitDetail), disbursement.Id);
            disbursement.Details.ForEach(f => f.MarketerDisbursementPermitId = disbursement.Id);
            await marketerSrviceDetails.AddRange(disbursement.Details);
            var idsArray = disbursement.Details.Select(s => s.Id).ToArray();
            var ids = string.Join(", ", idsArray);
            Logger.LogTrace("saved details {0} with for id {1} with ids: [{2}]", nameof(DtoMarketerDisbursementPermitDetail), disbursement.Id, ids);
            //
            //  Items
            await InventiryProcess(disbursement.Details);
        }
        var status = _service.CommitTransaction();
        if (!status)
            throw new Exception("cannot proccesed the sale opration");
        var invoiceDb = await _service.GetById(disbursement.Id);
        return invoiceDb;
    }



    [HttpGet("Number")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber([FromQuery] StoreQueryRequest request)
     =>
          TryOne<long>(async (opt) =>
          {
              var next = await _service.NextNumber(x => x.Number);
              return opt.Success(next);
          });


    [HttpGet("ExpireDate")]
    public ValueTask<HttpResponseWrapperList<LookupOf<string, DateTime>>> GetLookupExpireDate([FromQuery] ExpireDateQueryRequest request)
        =>
       TryMeny<LookupOf<string, DateTime>>(async _ =>
       {
           Logger.LogTrace("[ExpireDate] {1} , {0}", request.ToJson(), request.GetType());
           if (request.StorageType == StorageType.Marketer)
           {
               var marketerStorageModuleService = _.GetService<IMarketerModuleService>();
               var datesx = await marketerStorageModuleService.GetItemExpireDates(request.ItemId, request.StoreId);
               Logger.LogTrace("Found : [{0}]", datesx.Count());
               return _.Success(datesx);
           }
           var storageModuleService = _.GetService<IStorageModuleService>();
           var dates = await storageModuleService.GetItemExpireDates(request.ItemId, request.StoreId);
           Logger.LogTrace("Found : [{0}]", dates.Count());
           return _.Success(dates);
       });


    [HttpGet("marketers")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupMarketers([FromQuery] StoreQueryRequest request)
        =>
            GetSelectableList<DtoMarketer, TMarketer>(x => x.Name);

    [HttpGet("stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStore([FromQuery] StoreQueryRequest request)
        =>
            GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        =>
            GetSelectableList<DtoItem, TItem>(x => $"{x.Name} - {x.Barcode}", y => y.IsActive);

    [HttpGet("units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty), x => x.TransferRate);

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
        OnCreateUpdate -= MarketerDisbursementPermitController_OnCreate;
    }
}
