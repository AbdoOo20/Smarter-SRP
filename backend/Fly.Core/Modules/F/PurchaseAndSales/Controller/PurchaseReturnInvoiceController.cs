//

using System.Data;
using System.Linq.Expressions;
using AutoMapper;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store; //generalGlobalShareAccount!.Value!;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Shared.Modules.Store.Enums;
using DiscountType = Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Microsoft.Extensions.Configuration;
using Fly.Framework.Shared.Common.Collections;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class PurchaseReturnInvoiceController
    :
        CustomControllerFeatrueBased<DtoPurchaseReturnInvoice, TPurchaseReturnInvoice, PurchaseReturnInvoiceQueryRequest,
            DeletePurchaseReturnInvoiceRequest, PurchaseReturnInvoiceCreateRequest>,
        PurchaseReturnInvoiceFeatures
{

    private readonly IConfiguration _configuration;


    public PurchaseReturnInvoiceController(ILocalizationService localizationService,
        ILogger<PurchaseReturnInvoiceController> logger, IMediator mediator,
        IServiceBase<DtoPurchaseReturnInvoice, TPurchaseReturnInvoice> service, IMapper mapper,
        MapsterMapper.IMapper mapster, IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _configuration = configuration;
        OnDeleteEx += PurchaseReturnInvoiceController_OnDeleteEx;
        OnCreateUpdateEx += PurchaseReturnInvoiceController_OnCreateUpdateEx;
    }

    [NonAction]
    public void Dispose()
    {
        /*
         * delete the invoice
         * delete invoice-details
         * delete qauntities
         * delete entries
         */
        OnDeleteEx -= PurchaseReturnInvoiceController_OnDeleteEx;
        OnCreateUpdateEx -= PurchaseReturnInvoiceController_OnCreateUpdateEx;
    }

    [HttpGet("ByNumber")]
    public ValueTask<HttpResponseWrapper<DtoPurchaseReturnInvoice?>> GetInvoiceByNumber([FromQuery] string number)
        =>
            TryOne<DtoPurchaseReturnInvoice?>(async (opt) =>
            {
                if (!long.TryParse(number, out var num))
                    return opt.Problem("Wrong number"); // locale
                
                var next = await _service.GetOneAsAsync(x => x, y => y.Number == num);
                return opt.Success(next);
            });
    
    public override async Task BeforReturn(PageList<DtoPurchaseReturnInvoice> list)
    {
        var entryService = _service.MakeChildService<IEntryService>();
        var supplierService = _service.MakeChildService<DtoSupplier, TSupplier>();
        var purchaseReturnService = _service.MakeChildService<DtoPurchaseReturnInvoice, TPurchaseReturnInvoice>();

        var invoicesNumbers = list.Items.Select(W => W.InvoiceNumber);
        var sellInvoices = await purchaseReturnService.GetAllAsync(w => invoicesNumbers.Contains(w.Number));

        var arr = sellInvoices.Items.Where(w => w.SupplierId != null).Select(w => w.SupplierId.Value).Distinct().ToArray();

        if (arr.Length == 0)
        {
            return;
        }

        var accArr = await supplierService.GetAllAsAsync(s => new { s.AccountId, s.Id }, w => arr.Contains(w.Id));
        var accArrIds = accArr.Items.Select(s => s.AccountId).ToArray();
        var multiQuery = await entryService.GetBalance(accArrIds, null, null);

        var nonSupplier = list.Items.Where(w => w.SupplierId == null).ToList();

        var list2 = multiQuery
            .Join(accArr.Items, _ => _.AccountId, _ => _.AccountId, (x, y) => new
            {
                y.Id,
                x.Balance,
                x.AccountId,
            })
            .Join(
            list.Items, _ => _.Id, _ => _.SupplierId,
            (x, y) =>
            {
                y.Balance = x.Balance;
                return y;
            }).ToList();
        List<DtoPurchaseReturnInvoice> all = [.. nonSupplier, .. list2];
        list.Items = [.. all.OrderByDescending(s => s.Number)];
    }

    private async ValueTask<HttpResponseWrapper<DtoPurchaseReturnInvoice?>> PurchaseReturnInvoiceController_OnDeleteEx(DeletePurchaseReturnInvoiceRequest q, Framework.Server.Common.ResultOrFailer<DtoPurchaseReturnInvoice?> _)
    {
        var entryService = _service.MakeChildService<EntryService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var PurchaseReturnDetailsService = _service.MakeChildService<DtoPurchaseReturnInvoiceDetail, TPurchaseReturnInvoiceDetail>();
        //
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        var returnInvoice = await _service.GetById(q.Id);
        await PurchaseReturnDetailsService.Remove(w => w.ReturnInvoiceId == returnInvoice.Id);
        await storageModuleService.LossAllEver(returnInvoice.Id, RelatedInventory.ReturnPurchase);
        await entryService.RemoveAllRelated(returnInvoice.Id
            , OprationTypeEnum.Purchases_Return
            , PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
        //
        await _service.Remove(returnInvoice.Id);
        var status = _service.CommitTransaction();
        if (!status)
            return _.Problem("cannot proccesed the sale opration");
        return _.Success(returnInvoice)!;
    }

    [HttpPost("ReturnByNumber")]
    public ValueTask<HttpResponseWrapper<DtoPurchaseReturnInvoice?>> Create(TypeOf<long> q)
    => TryOne<DtoPurchaseReturnInvoice?>(async _ =>
    {

        var sellInvoiceService = _service.MakeChildService<DtoPurchaseInvoice, TPurchaseInvoice>();

        var invoice = await sellInvoiceService.GetAsync(w => w.Number == q.Value);
        if (invoice is null) return _.Problem(l["Invoice-not-found"]!); // Invoice not found

        var nextNumber = await _service.NextNumber(p => p.Number);
        DtoPurchaseReturnInvoice purchaseReturnInvoice = new()
        {
            Date = invoice.Date,
            Number = nextNumber,
            SafeId = invoice.SafeId,
            StoreId = invoice.StoreId,
            SupplierId = invoice.SupplierId,
            InvoiceNumber = invoice.Number,
            Discount = invoice.Discount,
            DiscountType = invoice.DiscountType,

            InvoiceId = invoice.Id,
            PeymentType = invoice.PeymentType,
            SerialNumber = invoice.SerialNumber,

            Details = invoice.Details
                .Select(x => new DtoPurchaseReturnInvoiceDetail
                {
                    Price = x.Price,
                    UnitId = x.UnitId,
                    ItemId = x.ItemId,
                    Discount = x.Discount,
                    Quantity = x.Quantity,
                    ExpireDate = x.ExpireDate,
                }).ToList(),
        };

        return await PurchaseReturnInvoiceController_OnCreateUpdateEx(purchaseReturnInvoice.Adapt<PurchaseReturnInvoiceCreateRequest>(), _);
    });

    public override Expression<Func<DtoPurchaseReturnInvoice, bool>> GetWhere(Expression<Func<DtoPurchaseReturnInvoice, bool>> expression, PurchaseReturnInvoiceQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {

            var isNumber = int.TryParse(query.SearchText, out var number);
            expression = expression.And(
                x =>
                x.Details.Any(d => d.UnitName.Contains(query.SearchText)) |
                x.Details.Any(d => d.ItemName.Contains(query.SearchText)) |
                x.Number.ToString().Contains(query.SearchText) |
                x.SupplierName.ToString().Contains(query.SearchText) |
                x.SafeName.Contains(query.SearchText) |
                x.TotalAmount.ToString() == query.SearchText |
                x.SerialNumber.ToString() == query.SearchText
            );
        }
        return base.GetWhere(expression, query);
    }

    private async ValueTask<HttpResponseWrapper<DtoPurchaseReturnInvoice?>> PurchaseReturnInvoiceController_OnCreateUpdateEx(PurchaseReturnInvoiceCreateRequest request, Framework.Server.Common.ResultOrFailer<DtoPurchaseReturnInvoice?> _)
    {
        var service = _service.MakeChildService<DtoPurchaseReturnInvoice, TPurchaseReturnInvoice>();
        var purchesInvoiceService = _service.MakeChildService<DtoPurchaseInvoice, TPurchaseInvoice>();
        var invoice = await purchesInvoiceService.GetAsync(w => w.Number == request.InvoiceNumber);
        //if (invoice is null) return _.Problem(l["Invoice-not-found"]!); // Invoice not found
        //
        if (invoice is not null)
        {
            var otherReturns = await service.GetAllAsync(w => w.InvoiceNumber == request.InvoiceNumber && w.Id != request.Id);
            if (otherReturns.Items.Any())
            {
                var result = AdjustQuantity(invoice, otherReturns.Items.ToArray());
                if (result is not null) return _.Problem(result);
            }
        }
        //
        request.Details = request.Details
            .GroupBy(w => new { w.UnitId, w.ItemId, w.ExpireDate })
            .Select(g => new PurchaseReturnInvoiceDetail(g.Key.UnitId, g.Key.ItemId, g.Sum(s => s.Quantity), g.Max(g => g.Price), g.Max(g => g.Discount), g.Key.ExpireDate))
            .ToList();
        var returnInvoice = request.Adapt<DtoPurchaseReturnInvoice>();
        var number = 0l;
        if (invoice is not null)
        {
            returnInvoice.Details.ForEach(f =>
            {
                var i = invoice.Details.FirstOrDefault(ff => ff.ItemId == f.ItemId && ff.UnitId == ff.UnitId) ?? throw new Exception("Item Not exists in real invoice");
                f.Price = i.Price;
                f.Discount = i.Discount;
            });
            returnInvoice.Discount = invoice.Discount;
            returnInvoice.DiscountType = invoice.DiscountType;
            var resultx = AdjustQuantity(invoice, [returnInvoice]);
            if (resultx is not null) return _.Problem(System.Net.HttpStatusCode.BadRequest, resultx);
            //
            returnInvoice.InvoiceId = invoice.Id;
            returnInvoice.SafeId = invoice.SafeId;
            returnInvoice.StoreId = invoice.StoreId;
            returnInvoice.SupplierId = invoice.SupplierId;
            returnInvoice.PeymentType = invoice.PeymentType;
            number = invoice.Number;
            //
        }
        else
        {
            number = await _service.NextNumber(w => w.Number);
            returnInvoice.Number = number;
        }
        //
        var returnDetailsNonDiscountedTotal = returnInvoice.Details.Sum(s => s.CountingPrince(-100));
        var returnDetailsTotal = returnInvoice.Details.Sum(s => s.CountingPrince((int)returnInvoice.DiscountType));
        var returnTotal = returnInvoice.DiscountType == DiscountType.Percent
            ? returnDetailsTotal - (returnDetailsTotal * returnInvoice.Discount / 100)
            : returnDetailsTotal - returnInvoice.Discount;
        var returnInvoiceTotalDiscount = returnDetailsNonDiscountedTotal - returnTotal;

        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var PurchaseReturnDetailsService = _service.MakeChildService<DtoPurchaseReturnInvoiceDetail, TPurchaseReturnInvoiceDetail>();
        //-----------------
        service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        //var defTotal = invoice.TotalAmount - returnTotal;
        //
        var generalSupplierId = await configService.GetOneAsAsync(x => x.GeneralSupplierAccountAccount);
        var generalSupplierAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(generalSupplierId));
        //  
        //  check returnInvoice.clientId
        var purchaseAccountId = await configService.GetOneAsAsync(x => x.PurchaseInvoicesAccount);
        var purchaseInvoiceBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(purchaseAccountId));
        //
        //
        //
        //  SAFE
        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        DtoAccount? safeInfoAccount = null;
        var safeInfo = await safeService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == returnInvoice.SafeId);
        if (safeInfo is null) returnInvoice.SafeId = null;
        else safeInfoAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(safeInfo.AccountId));
        if (safeInfo is null && PeymentType.Cash == returnInvoice.PeymentType)
            throw new InvalidInputException("Please Select Safe"); // TODO: Localization

        var discountAccountAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
        if (discountAccountAccountConfig is null) throw new Exception("DiscountAccount not found");
        var discountAccountAccount = await accountService
           .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountAccountConfig));
        if (discountAccountAccount is null) throw new Exception("DiscountAccount not found.");
        //
        //
        async Task<bool> FinacialProcess(bool IsRoleback = false, decimal oldTotalCosts = 0, decimal oldTotalPurchasers = 0, bool softDelete = false)
        {
            var countedTotalPurchasers = IsRoleback ? oldTotalPurchasers : returnTotal;

            //  1
            var finacialPurchersTransaction = accountingFinacialService
              .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                  OprationTypeEnum.Purchases_Return,
                  PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated,
                  returnInvoice.Id,
                  number,
                 "");
            // Set the invoice deption for the supplier
            var reelTotal = countedTotalPurchasers;// invoice.IsCostOnSupplier ? countedTotalPurchasers + countedTotalCosts : countedTotalPurchasers;
            finacialPurchersTransaction.AppendEntry(generalSupplierAccount.Id, purchaseInvoiceBaseAccount.Id, reelTotal);


            switch (returnInvoice.PeymentType)
            {
                case PeymentType.Cash:
                    // NOTE: if the safe has `loss percent` ?
                    finacialPurchersTransaction.AppendEntry(safeInfoAccount.Id, generalSupplierAccount.Id, reelTotal);
                    break;
                case PeymentType.Deferred:
                    break;
            }

            if (IsRoleback)
            {
                await finacialPurchersTransaction.RevertTransaction(note: "", softDelete);
            }
            else
            {
                await finacialPurchersTransaction.ComplateTransaction();
            }
            return true;
        }
        //
        //

        var entryService = _service.MakeChildService<EntryService>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var storeService = _service.MakeChildService<DtoStore, TStore>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        //
        var storesIds = (await storeService.GetAllAsAsync(s => s.Id)).Items.ToArray();
        async Task InventiryProcess(IEnumerable<DtoPurchaseReturnInvoiceDetail> invoiceDetails, bool isLose = true, bool softDelete = true)
        {
            foreach (var item in invoiceDetails)
            {
                //
                DtoPurchaseInvoiceDetail? realDet = invoice?.Details.FirstOrDefault(w => w.ItemId == item.ItemId);
                //
                //
                var properties = new Shared.Modules.Store.Dtos.Extra.StorageItemProperties();
                if (item.ExpireDate != null) properties.ExpireDate = item.ExpireDate;
                //
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var qun = item.Quantity;
                var strId = returnInvoice.StoreId;
                var useExpire = item.ExpireDate != null;
                //
                var itemInfo = await itemService
                    .GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);
                //
                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                var selectedOnePrimary = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId && w.IsPrimary);
                //
                var cost = itemInfo!.Cost;
                if (realDet is not null && realDet.UnitId == item.UnitId)
                {
                    cost = realDet.Price;

                    var realOnePrimary = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId && w.UnitId == realDet.UnitId);
                    var readUnitPrice = realDet.Price;
                    var pricePerOne = readUnitPrice / realOnePrimary!.TransferRate;
                    cost = pricePerOne * selectedOne!.TransferRate;
                }

                //
                //

                if (selectedOne is null || selectedOnePrimary is null)
                {
                    Logger.LogWarning("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }

                //var totalQuntity = selectedOne.TransferRate * qun;
                //var moves = await storageModuleService.GetMovements(item.ItemId, invoice.StoreId, [invoice.Id], RelatedInventory.Purchase);
                var allOnes = selectedOne.TransferRate * qun;

                await storageModuleService.RegainOrLoseItem(itmId, untId, strId.Value, qun, allOnes,
                    cost,
                    avragePiceCost: 0, //item.Price / selectedOne.TransferRate,
                    price: 0, // item.Price,
                    relatedId: returnInvoice.Id,
                    relatedInventory: RelatedInventory.ReturnPurchase,
                    isLose: isLose,
                    softDelete: softDelete,
                    storageItemProperties: properties
                );


            }
        }
        //
        //
        //
        if (request.IsNew())
        {
            /* In return process
             * Save the request
             */

            await _service.Add(returnInvoice);
            //await FinacialProcess( invoice.TotalAmount, true, false, @return: false);
            await FinacialProcess();
            await InventiryProcess(returnInvoice.Details);
        }
        else
        {
            //
            await PurchaseReturnDetailsService.Remove(w => w.ReturnInvoiceId == returnInvoice.Id);
            await storageModuleService.LossAllEver(returnInvoice.Id, RelatedInventory.ReturnPurchase);
            await entryService.RemoveAllRelated(returnInvoice.Id
                , OprationTypeEnum.Purchases_Return
                , PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
            //  --
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoPurchaseReturnInvoice), returnInvoice.Id);
            returnInvoice.Details.ForEach(f => f.ReturnInvoiceId = returnInvoice.Id);
            await service.Update(returnInvoice);
            await PurchaseReturnDetailsService.AddRange(returnInvoice.Details);
            var idsArray = returnInvoice.Details.Select(s => s.Id).ToArray();
            var ids = string.Join(", ", idsArray);
            //  --
            Logger.LogTrace("saved details {0} with for id {1} with ids: [{2}]", nameof(DtoPurchaseReturnInvoiceDetail), returnInvoice.Id, ids);
            //
            await InventiryProcess(returnInvoice.Details);
            await FinacialProcess();
        }

        service.CommitTransaction();
        returnInvoice.PurchaseInvoice = invoice;
        //var allReturns = otherReturns.Items.Append(returnInvoice);
        return _.Success(returnInvoice);
    }

    [HttpGet("Number")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber()
        => TryOne<long>(async (opt) =>
        {
            var next = await _service.NextNumber(x => x.Number);
            return opt.Success(next);
        });

    [HttpGet("VerifyInvoice")]
    public ValueTask<HttpResponseWrapper<DtoPurchaseReturnInvoice>> GetVerifyInvoice(
        [FromQuery] PurchaseInvoiceQueryRequest request)
        => TryOne<DtoPurchaseReturnInvoice>(async _ =>
        {
            var sellInvoiceService = _service.MakeChildService<DtoPurchaseInvoice, TPurchaseInvoice>();
            var invoice = await sellInvoiceService.GetAsync(w => w.Number == request.Number);
            if (invoice is null) return _.Problem(System.Net.HttpStatusCode.BadRequest, l["Invoice-not-found"]!); // Invoice not found

            var nextNumber = await _service.NextNumber(p => p.Number);
            DtoPurchaseReturnInvoice PurchaseReturnInvoice = new()
            {
                Date = invoice.Date,
                Number = nextNumber,
                SafeId = invoice.SafeId,
                StoreId = invoice.StoreId,
                SupplierId = invoice.SupplierId,
                InvoiceNumber = invoice.Number,
                Discount = invoice.Discount,
                DiscountType = invoice.DiscountType,

                InvoiceId = invoice.Id,
                PeymentType = invoice.PeymentType,
                SerialNumber = invoice.SerialNumber,


                Details = invoice.Details
                .Select(x => new DtoPurchaseReturnInvoiceDetail
                {
                    Price = x.Price,
                    UnitId = x.UnitId,
                    ItemId = x.ItemId,
                    Discount = x.Discount,
                    Quantity = x.Quantity,
                    ExpireDate = x.ExpireDate,
                }).ToList(),
            };

            return _.Success(PurchaseReturnInvoice);
        });

    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes([FromQuery] SafeQueryRequest request)
        => GetSelectableList<DtoSafe, TSafe>(x => x.Name);

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name} - {x.Barcode}",
            request.ItemId != null
                ? (x => x.CategoryId == request.ItemId.Value)
                : null
        );

    [HttpGet("Categories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCat([FromQuery] CategoryQueryRequest request)
        =>
        GetSelectableList<DtoCategory, TCategory>(x => x.Name);

    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        => GetSelectableList<DtoItemUnit, TItemUnit>(
            x => x.UnitId,
            x => x.Unit!.Name,
            w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));

    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores([FromQuery] StoreQueryRequest request)
        => GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients([FromQuery] ClientQueryRequest request)
        => TryMeny<DefaultLookup>(async _ =>
        {
            var clientService = _service.MakeChildService<DtoClient, TClient>();
            var where = clientService.Build();

            if (request.MarketerId is not null)
            {
                var marketerServic = _service.MakeChildService<DtoMarketerClient, TMarketerClient>();
                var seletedMarketerUsers
                    = await marketerServic.GetAllAsAsync(s
                        => new { s.ClientId }, w => w.MarketerId == request.MarketerId);
                var ids = seletedMarketerUsers.Items.Select(s => s.ClientId);
                where = where.And(w => ids.Contains(w.Id));
            }

            var all = await clientService.GetSelecListAsync(s => s.Name, where);
            return _.Success(all);
        });

    [HttpGet("ItemInfo")]
    public ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> PostItemInfoQuery([FromQuery] GetItemInfoQueryRequest queryRequest)
        => TryOne<ItemInfoQueryResponse>(async opt =>
        {
            var itemService = _service.MakeChildService<DtoItem, TItem>();
            var itemUnitService = _service.MakeChildService<DtoItemUnit, TItemUnit>();

            var itemInfo = await itemService
                .GetOneAsAsync(s => new { s.Price }, w => queryRequest.ItemId == w.Id);
            var selectedUnit = await itemUnitService
                .GetOneAsAsync(s => new { s.TransferRate },
                    w => w.UnitId == queryRequest.UnitId && w.ItemId == queryRequest.ItemId);

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

    [HttpGet("ExpireDate")]
    public ValueTask<HttpResponseWrapperList<LookupOf<string, DateTime>>> GetLookupExpireDate([FromQuery] ExpireDateQueryRequest request)
        =>
        TryMeny<LookupOf<string, DateTime>>(async _ =>
        {
            if (request.StorageType == StorageType.Marketer)
            {
                var marketerStorageModuleService = _service.MakeChildService<IMarketerModuleService>();
                var datesx = await marketerStorageModuleService.GetItemExpireDates(request.ItemId, request.StoreId);
                return _.Success(datesx);
            }
            var storageModuleService = _service.MakeChildService<IStorageModuleService>();
            var dates = await storageModuleService.GetItemExpireDates(request.ItemId, request.StoreId);
            return _.Success(dates);
        });

    [HttpGet("Supplier")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSuppliers([FromQuery] SupplierQueryRequest request)
        =>
        GetSelectableList<DtoSupplier, TSupplier>(x => x.Name);

    private string? AdjustQuantity(DtoPurchaseInvoice dtoPurchaseReturnInvoice, DtoPurchaseReturnInvoice[] dtoPurchaseReturnInvoices)
    {
        // TODO: Implement the code for adjusting the quantity
        foreach (var invoice in dtoPurchaseReturnInvoices)
        {
            foreach (var item in invoice.Details)
            {
                var el = dtoPurchaseReturnInvoice.Details.FirstOrDefault(w => w.ItemId == item.ItemId);
                if (el is null) continue; // todo: find solution
                el.Quantity += item.Quantity;
            }
        }
        return null;
    }
}