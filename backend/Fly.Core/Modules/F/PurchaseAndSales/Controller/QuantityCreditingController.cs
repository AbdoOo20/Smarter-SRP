// = saleInvoice.StoreId;>();>);("client debt is not enough");
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;//generalGlobalShareAccount!.Value!;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Mapster;
using Fly.Shared.Modules.Store.Enums;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Common;
using Microsoft.Extensions.Configuration;
using Fly.Shared.Modules.PurchaseAndQuantityCrediting.Requests;
using MapsterMapper;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using System.Linq.Expressions;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

//QuantityCrediting
public class QuantityCreditingController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoQuantityCreditingInvoice, TQuantityCreditingInvoice, QuantityCreditingInvoiceQueryRequest, DeleteQuantityCreditingInvoiceRequest, QuantityCreditingInvoiceCreateRequest>,
        IQuantityCreditingFeatures
{
    private readonly IConfiguration configuration;
    private readonly CustomLoggingService _customLogger;
    RelatedInventory relatedInventory = RelatedInventory.QuantityCrediting;

    public QuantityCreditingController(ILocalizationService localizationService, ILogger<QuantityCreditingController> logger, IMediator mediator, IServiceBase<DtoQuantityCreditingInvoice, TQuantityCreditingInvoice> service, AutoMapper.IMapper mapper, IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _customLogger = new CustomLoggingService(logger);
        /*
         * delete the invoice
         * delete invoice-details
         * delete qauntities
         */
        OnDeleteEx += QuantityCreditingController_OnDeleteEx;
        OnCreateUpdateEx += QuantityCreditingController_OnCreateUpdateEx;
    }

    private async ValueTask<HttpResponseWrapper<DtoQuantityCreditingInvoice?>> QuantityCreditingController_OnDeleteEx(DeleteQuantityCreditingInvoiceRequest q, ResultOrFailer<DtoQuantityCreditingInvoice?> opt)
    {
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var invoiceSrviceDetailsService = _service.MakeChildService<DtoQuantityCreditingInvoiceDetail, TQuantityCreditingInvoiceDetail>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        var invoice = await _service.GetById(q.Id);
        await invoiceSrviceDetailsService.Remove(w => w.QuantityCreditingInvoiceId == invoice.Id);
        await storageModuleService.LossAllEver(invoice.Id, [RelatedInventory.QuantityCrediting, RelatedInventory.QuantityCrediting_Zero]);

        await _service.Remove(invoice);
        var status = _service.CommitTransaction();
        if (!status)
            return opt.Problem("cannot proccesed the sale opration");
        return opt.Success(invoice);
    }

    private async ValueTask<HttpResponseWrapper<DtoQuantityCreditingInvoice?>> QuantityCreditingController_OnCreateUpdateEx(QuantityCreditingInvoiceCreateRequest q, ResultOrFailer<DtoQuantityCreditingInvoice?> opt)
    {
        var service = _service.MakeChildService<DtoQuantityCreditingInvoice, TQuantityCreditingInvoice>();
        //
        // var q =  _service.MakeChildService<DtoQuantityCreditingInvoice, TQuantityCreditingInvoice>();
        var unititemService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var invoiceSrviceDetailsService = _service.MakeChildService<DtoQuantityCreditingInvoiceDetail, TQuantityCreditingInvoiceDetail>();
        _customLogger.TraceGettingServices(unititemService.GetType()
            , storageModuleService.GetType()
            , invoiceSrviceDetailsService.GetType());
        //
        q.Details = q.Details
            .GroupBy(w => new { w.UnitId, w.ItemId, w.ExpireDate })
            .Select(g => new QuantityCreditingInvoiceDetail(g.Key.UnitId, g.Key.ItemId, g.Sum(s => s.Quantity), g.Max(g => g.Price), g.Max(g => g.Discount), g.Key.ExpireDate))
            .ToList();
        var isNew = q.IsNew();
        var invoice = q.Adapt<DtoQuantityCreditingInvoice>()!;
        //
        Logger.LogTrace("Mapping Request and Working on {0} with id {1}", nameof(QuantityCreditingController), q.Id);
        if (invoice.StoreId is null) return opt.Problem("invoice.StoreId");
        //
        //  invoice.Details = q.Details.Adapt<List<DtoQuantityCreditingDetail>>();
        var itemsUnits = invoice.Details.Select(w => new { w.UnitId, w.ItemId }).ToArray();
        var itemsUnitsO = invoice.Details.Select(w => w.ItemId).ToArray();
        //
        var unitInfo = await unititemService
            .GetAllAsAsync(
                s => new { s.ItemId, s.UnitId, s.IsPrimary, s.TransferRate },
                s => itemsUnitsO.Contains(s.ItemId)
            );
        //
        service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        //
        async Task<HttpResponseWrapper<DtoQuantityCreditingInvoice?>> AddDetails()
        {
            foreach (var gggByItem in invoice.Details.GroupBy(grp => new { grp.ItemId }))
            {
                var myPrimary = unitInfo.Items.Where(w => w.ItemId == gggByItem.Key.ItemId).FirstOrDefault(w => w.IsPrimary);
                if (myPrimary is null) return opt.Problem(l["PrimaryUnitNotFound"]!); // Primary unit not found

                var info = await storageModuleService.GetItemQuntity(gggByItem.Key.ItemId, myPrimary.UnitId, invoice.StoreId.Value);
                //var infox = await storageModuleService.Getex(gggByItem.Key.ItemId, myPrimary.UnitId, invoice.StoreId.Value);
                if (info is null || info.Inventory is null)
                {
                    return opt.Problem("info.Inventory not exists");
                }
                var all = info.Inventory.TotalQuantity;
                all = all > 0 ? all : -all;
                //var toBeCre = all > 0 ? info.Inventory.TotalQuantity : info.Inventory.TotalQuantity * -1;
                if (all != 0)
                {

                    await storageModuleService
                          .RegainOrLoseItem(gggByItem.Key.ItemId, myPrimary.UnitId, invoice.StoreId.Value
                          , all
                          , all
                          , 0
                          , relatedId: invoice.Id
                          , relatedInventory: RelatedInventory.QuantityCrediting_Zero
                          , isLose: all > 0
                          /*, storageItemProperties: storageItemProperties*/);
                }


                foreach (var gggByItemUnit in gggByItem.GroupBy(grp => new { grp.ItemId, grp.UnitId }))
                {
                    var q = gggByItemUnit.Sum(s => s.Quantity);

                    var selectedUnit = unitInfo.Items.FirstOrDefault(w => w.UnitId == gggByItemUnit.Key.UnitId && w.ItemId == gggByItem.Key.ItemId);
                    if (selectedUnit is null)
                    {
                        return opt.Problem("Selected unit is not exists");
                    }

                    foreach (var item in gggByItemUnit)
                    {

                        StorageItemProperties storageItemProperties = new()
                        {
                            ExpireDate = item.ExpireDate,
                        };

                        var cost = item.Price;
                        var costPerUnit = cost / selectedUnit.TransferRate;
                        await storageModuleService
                            .RegainItem(item.ItemId, item.UnitId, invoice.StoreId.Value, item.Quantity,
                            item.Quantity * selectedUnit.TransferRate,

                            cost: cost,
                            avrageCost: cost,
                            avragePiceCost: costPerUnit,
                            price: item.Price,
                            sellPrice: 0m,

                            relatedId: invoice.Id,
                            relatedInventory: RelatedInventory.QuantityCrediting, storageItemProperties: storageItemProperties);
                    }
                }
            }

            return null;
        }
        //
        if (isNew)
        {

            //sellInvoice.Date = DateTime.Now;
            var lastNumber = await service.GetMaxAsync(c => c.Number);
            invoice.Number = lastNumber + 1;

            await service.Add(invoice);
            Logger.LogTrace("saved {0} with id {1}", nameof(DtoQuantityCreditingInvoice), invoice.Id);

            var res = await AddDetails();
            if (res is not null) return res;
        }
        else
        {
            invoice.Date = DateTime.Now;
            //
            Logger.LogTrace("removing details {0} for id {1}", nameof(DtoQuantityCreditingInvoiceDetail), invoice.Id);
            await invoiceSrviceDetailsService.Remove(w => w.QuantityCreditingInvoiceId == invoice.Id);
            await storageModuleService.LossAllEver(invoice.Id, RelatedInventory.QuantityCrediting);
            await storageModuleService.LossAllEver(invoice.Id, RelatedInventory.QuantityCrediting_Zero);
            //
            await service.Update(invoice);
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoQuantityCreditingInvoice), invoice.Id);
            invoice.Details.ForEach(f => f.QuantityCreditingInvoiceId = invoice.Id);
            await invoiceSrviceDetailsService.AddRange(invoice.Details);
            Logger.LogTrace("saved details {0} for id {1}", nameof(DtoQuantityCreditingInvoiceDetail), invoice.Id);
            //
            var res = await AddDetails();
            if (res is not null) return res;
            //
        }
        //
        var status = service.CommitTransaction();
        var invoiceDb = await service.GetById(invoice.Id);
        return opt.Success(invoiceDb);
        //
    }

    [HttpPost("fix/{identifier}")]
    public ValueTask<HttpResponseWrapper<DtoQuantityCreditingInvoice?>> ReCreaditing(Identifier identifier)
    => TryOne<DtoQuantityCreditingInvoice?>(async (opt) =>
    {
        // _service.MakeChildService<>
        var unitService = _service.MakeChildService<DtoUnit, TUnit>();
        var inventoryService = _service.MakeChildService<DtoInventory, TInventory>();
        var inventoryCostService = _service.MakeChildService<DtoInventoryCost, TInventoryCost>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();

        _service.BeginTransaction();

        var old = await _service.GetById(identifier);
        if (old is null || old.StoreId is null) return opt.Problem("Invoice not found");

        var allBefore = await inventoryCostService.GetAllAsync(w => w.CreateDate < old.CreateDate);
        foreach (var item in old.Details)
        {
            var itemINfo = allBefore.Items.Where(w => w.ItemId == item.ItemId /*&& w.UnitId == item.UnitId*/);
            foreach (var rgpExp in itemINfo.GroupBy(c => c.ExpireDate))
            {
                var q = rgpExp.Sum(s => s.Inventory!.Ingress ? s.Inventory.TotalQuantity : -s.Inventory.TotalQuantity);
                if (q != 0)
                {
                    if (q < 0) q *= -1;
                    var isLode = q > 0;
                    await storageModuleService
                    .RegainOrLoseItem(item.ItemId, item.UnitId, old.StoreId.Value
                    , q
                    , q
                    , 0
                    , relatedId: old.Id, relatedInventory: RelatedInventory.QuantityCrediting_Zero
                    , isLose: isLode, storageItemProperties: new() { ExpireDate = rgpExp.Key });
                }
            }
            await inventoryService.Remove(
                w =>
                w.RelatedId == old.Id
                && w.RelatedIndex == RelatedInventory.QuantityCrediting_Zero
                && w.UnitId == item.UnitId
                && w.ItemId == item.ItemId
            );
        }

        _service.CommitTransaction();

        return opt.Success(old);
    });



    [HttpGet("ExpireDate")]
    public ValueTask<HttpResponseWrapperList<LookupOf<string, DateTime>>> GetLookupExpireDate([FromQuery] ExpireDateQueryRequest request)
        =>
        TryMeny<LookupOf<string, DateTime>>(async _ =>
        {
            if (request.StorageType == StorageType.Marketer)
            {
                var marketerStorageModuleService = _.GetService<IMarketerModuleService>();
                var datesx = await marketerStorageModuleService.GetItemExpireDates(request.ItemId, request.StoreId);
                return _.Success(datesx);
            }
            var storageModuleService = _.GetService<IStorageModuleService>();
            var dates = await storageModuleService.GetItemExpireDates(request.ItemId, request.StoreId);
            return _.Success(dates);
        });


    [HttpGet("Categories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCat([FromQuery] CategoryQueryRequest request)
        =>
        GetSelectableList<DtoCategory, TCategory>(x => x.Name);


    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        =>
        GetSelectableList<DtoItem, TItem>(x => $"{x.Name} - {x.Barcode}",
            request.ItemId != null
            ? (x => x.CategoryId == request.ItemId.Value)
            : (Expression<Func<DtoItem, bool>>)null
        );


    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));


    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores([FromQuery] StoreQueryRequest request)
    //=> TryMeny<DefaultLookup>(async _ =>
    //{
    //    var srorageServic = _.GetService<DtoStore, TStore>();
    //    var MarketerServic = _.GetService<DtoMarketer, TMarketer>();

    //    var allStorages = await srorageServic.GetAllAsAsync(s => new DefaultLookup { Display = s.Name, Value = s.Id });
    //    var allMarketers = await MarketerServic.GetAllAsAsync(s => new DefaultLookup { Display = s.Name, Value = s.Id });

    //    var all = allMarketers.Items.Concat(allStorages.Items);

    //    return _.Success(PageList<DefaultLookup>.Create(all, 0, 0));
    //});
    => GetSelectableList<DtoStore, TStore>(x => x.Name);


    [HttpGet("MarketerStores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupMarketerStores([FromQuery] StoreQueryRequest request)
        => GetSelectableList<DtoMarketer, TMarketer>(x => x.Name);


    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients([FromQuery] ClientQueryRequest request)
        => TryMeny<DefaultLookup>(async _ =>
        {
            var clientService = _.GetService<DtoClient, TClient>();
            var where = clientService.Build();

            if (request.MarketerId is not null)
            {
                var marketerServic = _.GetService<DtoMarketerClient, TMarketerClient>();
                var seletedMarketerUsers = await marketerServic.GetAllAsAsync(s => new { s.ClientId }, w => w.MarketerId == request.MarketerId);
                var ids = seletedMarketerUsers.Items.Select(s => s.ClientId);
                where = where.And(w => ids.Contains(w.Id));
            }

            var all = await clientService.GetSelecListAsync(s => s.Name, where);
            return _.Success(all);
        });
    //=> GetSelectableList<DtoClient, TClient>(x => x.Name);


    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes([FromQuery] SafeQueryRequest request)
        =>
        GetSelectableList<DtoSafe, TSafe>(x => x.Name);


    [HttpGet("Representatives")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupRepresentatives([FromQuery] MarketerQueryRequest request)
        =>
        GetSelectableList<DtoMarketer, TMarketer>(x => x.Name + " | " + x.Percentage, x => x.IsRepresentative);


    [HttpGet("ItemInfo")]
    public ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> PostItemInfoQuery([FromQuery] GetItemInfoQueryRequest queryRequest)
    =>
        TryOne<ItemInfoQueryResponse>(async opt =>
        {
            //var request = this.Request;
            //var itemService = _service.MakeChildService<DtoItem, TItem>();
            //var itemUnitService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
            //var storageService = _service.MakeChildService<IStorageModuleService>();

            //var inventoryInfo = await storageService.GetItemQuntity(queryRequest.ItemId, queryRequest.UnitId, queryRequest.StoreId);

            //var selectedUnit = await itemUnitService.GetOneAsAsync(s => new { s.TransferRate }, w => w.UnitId == queryRequest.UnitId && w.ItemId == queryRequest.ItemId);
            //var price = await itemService.GetOneAsAsync(s => new { s.Price }, w => w.Id == queryRequest.ItemId);


            //return opt.Success(new ItemInfoQueryResponse()
            //{
            //    Price = price!.Price * selectedUnit.TransferRate,
            //    Quntity = inventoryInfo.Quantity,
            //    AdditionalQuntity = inventoryInfo.AdditionalQuantity,
            //});

            var itemService = _service.MakeChildService<DtoItem, TItem>();
            var itemUnitService = _service.MakeChildService<DtoItemUnit, TItemUnit>();

            var itemInfo = await itemService
                .GetOneAsAsync(s => new { s.Price }, w => queryRequest.ItemId == w.Id);
            var selectedUnit = await itemUnitService
                .GetOneAsAsync(s => new { s.TransferRate }, w => w.UnitId == queryRequest.UnitId && w.ItemId == queryRequest.ItemId);

            if (selectedUnit is null)
            {
                return opt.Problem(l.GetText("unit-not-found"));
            }

            return opt.Success(new ItemInfoQueryResponse()
            {
                Price = itemInfo!.Price * selectedUnit.TransferRate,
                Quntity = 0, // inventoryInfo.Quantity,
                AdditionalQuntity = 0, // inventoryInfo.AdditionalQuantity,
            });

        });


    [HttpGet("Number")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber()
    =>
      TryOne<long>(async (opt) =>
      {
          var next = await _service.NextNumber(x => x.Number);
          return opt.Success(next);
      });
}
