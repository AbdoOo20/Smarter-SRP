using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Core.Modules.F.PurchaseAndSales;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Features;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Fly.Core.Modules.F.Marketers.Controller;

public class MarketerSalesInvoiceController2
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoMarketerSalesInvoice, TMarketerSalesInvoice, MarketerSalesInvoiceQueryRequest, MarketerDeleteSalesInvoiceRequest, MarketerSalesInvoiceCreateRequest>,
        IMarketerSalesInvoiceFeatures
{

    RelatedInventory relatedInventory = RelatedInventory.MarketerMobile_SellInvoice;
    private readonly IConfiguration configuration;

    public MarketerSalesInvoiceController2(ILocalizationService localizationService, ILogger<MarketerSalesInvoiceController2> logger, IMediator mediator, IServiceBase<DtoMarketerSalesInvoice, TMarketerSalesInvoice> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster,
        IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        /*
         * delete the invoice
         * delete invoice-details
         * delete qauntities
         * delete entries
         */
        OnDeleteEx += SalesController_OnDeleteEx;
        OnCreateUpdate += MarketerSalesInvoiceController_OnCreateUpdate;
        this.configuration = configuration;
    }

    private async ValueTask<HttpResponseWrapper<DtoMarketerSalesInvoice?>> SalesController_OnDeleteEx(MarketerDeleteSalesInvoiceRequest q, ResultOrFailer<DtoMarketerSalesInvoice?> opt)
    {
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        var entryService = _service.MakeChildService<IEntryService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var invoiceSrviceDetails = _service.MakeChildService<DtoMarketerSalesInvoiceDetail, TMarketerSalesInvoiceDetail>();
        //
        var invoice = await _service.GetById(q.Id);
        await invoiceSrviceDetails.Remove(w => w.MarketerSalesInvoiceId == q.Id);
        await storageModuleService.LossAllEver(q.Id,
            [RelatedInventory.Sale, RelatedInventory.MarketerSaleSell,
            RelatedInventory.MarketerSalePreparation_Sale]
        );
        await marketerModuleService.LossAllEver(q.Id, RelatedInventory.MarketerSalePreparation_Marketer);
        await entryService.RemoveAllRelated(q.Id, OprationTypeEnum.MarketerSell, PurchaseAndSalesConstants.TransactionOprationType__Marketer);
        //
        await _service.Remove(q.Id);
        var status = _service.CommitTransaction();
        if (!status)
            return opt.Problem("cannot proccesed the sale opration");
        return opt.Success(invoice)!;
    }

    public override async Task BeforReturn(PageList<DtoMarketerSalesInvoice> list)
    {

        #region Counting Balances

        var entryService = _service.MakeChildService<IEntryService>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var arr = list.Items.Where(w => w.ClientId != null).Select(w => w.ClientId.Value).Distinct().ToArray();
        var accArr = await clientService.GetAllAsAsync(s => new { s.AccountId, s.Id }, w => arr.Contains(w.Id));
        var accArrIds = accArr.Items.Select(s => s.AccountId).ToArray();
        var multiQuery = await entryService.GetBalance(accArrIds, null, null);

        var nonClientInvoice = list.Items.Where(w => w.ClientId == null).ToList();
        var list2 = multiQuery
            .Join(accArr.Items, _ => _.AccountId, _ => _.AccountId, (x, y) => new
            {
                y.Id,
                x.Balance,
                x.AccountId,
            })
            .Join(
            list.Items, _ => _.Id, _ => _.ClientId,
            (x, y) =>
            {
                y.Balance = x.Balance;
                return y;
            }).ToList();
        List<DtoMarketerSalesInvoice> all = [.. nonClientInvoice, .. list2];
        list.Items = [.. all.OrderBy(s => s.Number)];

        #endregion

        #region Counting Qauntities
        // todo: add retern
        //var sellReturnInvoiceService = _service.MakeChildService<DtoSellReturnInvoice, TSellReturnInvoice>();
        //var sellInvoices = list.Items.Select(s => s.Id).Distinct().ToArray();
        //var aa = await sellReturnInvoiceService.GetAllAsync(w => sellInvoices.Contains(w.InvoiceId!.Value));
        //foreach (var invoice in list.Items)
        //{
        //    AdjustQuantity(invoice, aa.Items.Where(w => w.InvoiceId == invoice.Id).ToArray());
        //}
        #endregion

    }

    public override Expression<Func<DtoMarketerSalesInvoice, bool>> GetWhere(Expression<Func<DtoMarketerSalesInvoice, bool>> expression, MarketerSalesInvoiceQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Details.Any(d => d.UnitName.Contains(query.SearchText)) |
                x.Details.Any(d => d.ItemName.Contains(query.SearchText)) |
                x.Number.ToString().Contains(query.SearchText) |
                //x.SafeName.Contains(query.SearchText) |
                x.TotalAmount.ToString() == query.SearchText |
                //x.SerialNumber.ToString() == query.SearchText |
                x.ClientName.Contains(query.SearchText)
            );
        return base.GetWhere(expression, query);
    }

    [NonAction]
    private async ValueTask<DtoMarketerSalesInvoice> MarketerSalesInvoiceController_OnCreateUpdate(MarketerSalesInvoiceCreateRequest q)
    {
        //
        //
        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var storeService = _service.MakeChildService<DtoStore, TStore>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var marketerSrvice = _service.MakeChildService<DtoMarketer, TMarketer>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var entryService = _service.MakeChildService<IEntryService>();
        var invoiceSrviceDetails = _service.MakeChildService<DtoMarketerSalesInvoiceDetail, TMarketerSalesInvoiceDetail>();
        //
        //
        var storesIds = (await storeService.GetAllAsAsync(s => s.Id)).Items.ToArray();
        //****************************(0)****************************
        //****************************(0)****************************
        Logger.LogTrace("items befor grouping: {0}", q.Details.Count());
        q.Details = q.Details
            .GroupBy(w => new { w.UnitId, w.ItemId, w.ExpireDate })
            .Select(g => new MarketerSalesInvoiceDetail(g.Key.UnitId, g.Key.ItemId, g.Sum(s => s.Quantity), g.Max(g => g.Price), g.Max(g => g.Discount), g.Key.ExpireDate))
            .ToList();
        Logger.LogTrace("items after grouping: {0}", q.Details.Count());
        //****************************(0)****************************
        //****************************(0)****************************
        var marketerSellInvoice = q.Adapt<DtoMarketerSalesInvoice>();
        //if (sellInvoice.StoreId is null || sellInvoice.StoreId.Value.IsNullOrEmpty())
        //    sellInvoice.StoreId = null;
        //
        DtoAccount? saleInvoiceBaseAccount = null;
        var selectedItems = new List<Identifier>();
        var selectedUnits = new List<Identifier>();
        //
        var totalNonDiscounted = marketerSellInvoice.CountingPrince(-100);
        var totalDetails = marketerSellInvoice.CountingPrince((int)marketerSellInvoice.DiscountType);
        var total = marketerSellInvoice.DiscountType == Shared.Modules.PurchaseAndSales.Enums.DiscountType.Percent 
            ? totalDetails - (totalDetails * marketerSellInvoice.Discount / 100) 
            : totalDetails - marketerSellInvoice.Discount;
        if (total < 0)
        {
            throw new InvalidInputException("الاجمالي يجب أن يكون أكبر من صفر");
        }
        var totalDiscounted = totalNonDiscounted - total;
        marketerSellInvoice.TotalAmount = totalNonDiscounted;
        marketerSellInvoice.Discount = totalDiscounted;
        marketerSellInvoice.Details.Distinct().ForEach(f =>
        {
            selectedItems.Add(f.ItemId);
            selectedUnits.Add(f.UnitId);
        });
        //
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        Logger.LogTrace("0x0001 - BeginTransaction");
        //
        var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
            unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
            unit => selectedItems.Contains(unit.ItemId) & selectedUnits.Contains(unit.UnitId)
        )).Items;
        //
        var clientInfo = await clientService.GetOneAsAsync(x => new { x.AccountId, x.MaxDebt }, w => w.Id == marketerSellInvoice.ClientId);
        if (clientInfo is null)
        {
            marketerSellInvoice.ClientId = null;
            //throw new Exception("client account not exists"); // TODO: Localization
        }

        //
        #region Accounts
        //
        //
        var discountAccountAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
        if (discountAccountAccountConfig is null) throw new Exception("DiscountAccount not found");
        var discountAccountAccount = await accountService
           .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountAccountConfig));
        if (discountAccountAccount is null) throw new Exception("DiscountAccount not found.");
        //
        //  SALE
        var saleAccountId = await configService.GetOneAsAsync(x => x.SalesInvoicesAccountId);
        if (saleAccountId is null)
            throw new Exception("MarketerSalesInvoicer account not ready"); // TODO: Localization
        saleInvoiceBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(saleAccountId));
        if (saleInvoiceBaseAccount is null)
            throw new Exception("MarketerSalesInvoicer account not ready"); // TODO: Localization
        //
        //
        var generalClientId = await configService.GetOneAsAsync(x => x.GeneralClientAccountAccount);
        if (generalClientId is null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization
        var generalClientAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(generalClientId));
        if (generalClientAccount is null && marketerSellInvoice.ClientId == null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization
        //
        //
        #endregion
        //
        //
        long invoiceNumber = 0;

        //
        //
        //
        async Task<bool> FinacialProcess(decimal _discountedTtoal, decimal _total, bool IsRoleback = false, bool softDelete = false)
        {
            //  1
            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    OprationTypeEnum.MarketerMobileSell,
                    PurchaseAndSalesConstants.TransactionOprationType__MarkterSalesRelated,
                    marketerSellInvoice.Id,
                    invoiceNumber,
                   "");
            //
            var theClient = clientInfo?.AccountId ?? generalClientAccount!.Id;
            finacialTransaction.AppendEntry(theClient, saleInvoiceBaseAccount.Id, (_total + _discountedTtoal));
            //  NOTE: remove the `representative counting logic`
            //if (representativeInfo is not null && generalGlobalShareAccount is not null)
            //{
            //    var sharedAmount = saleInvoice.TotalAmount * (representativeInfo.Percentage / 100);
            //    finacialTransaction.AppendEntry(representativeInfo.AccountId, generalGlobalShareAccount.Id, sharedAmount);
            //}
            if (_discountedTtoal > 0)
            {
                finacialTransaction.AppendEntry(discountAccountAccount.Id, saleInvoiceBaseAccount.Id, _discountedTtoal);
            }

            switch (marketerSellInvoice.PeymentType)
            {
                case PeymentType.Cash:
                    // NOTE: if the safe has `loss percent` ?
                    //finacialTransaction.AppendEntry(safeInfo.AccountId, theClient, _total);
                    break;
                case PeymentType.Deferred:
                    break;
            }

            if (IsRoleback)
            {
                await finacialTransaction.RevertTransaction(note: "", softDelete);
            }
            else
            {
                await finacialTransaction.ComplateTransaction();
            }
            return true;
        }
        //
        async Task InventiryProcess(IEnumerable<DtoMarketerSalesInvoiceDetail> invoiceDetails, bool isLose = false, bool softDelete = true)
        {

            //****************************(0)****************************
            Logger.LogTrace("items updating : {0}, IsLose: {1}", invoiceDetails.Count(), isLose);
            //****************************(0)****************************
            foreach (var item in invoiceDetails)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                //var strId = sellInvoice.StoreId;
                var qun = item.Quantity;
                var mrk = marketerSellInvoice.MarketerId;
                // StorageType.Marketer
                var isNormalMode = false;

                var expire = item.ExpireDate;
                var useExpire = item.ExpireDate != null;

                var properties = new Shared.Modules.Store.Dtos.Extra.StorageItemProperties()
                {
                    ExpireDate = expire,
                };

                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                if (selectedOne is null)
                {
                    Logger.LogTrace("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }

                //var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);

                //  CHECK-QUNTITY
                {
                    var quntityInfo = await marketerModuleService.CanLoseItemQuntityInfo(itmId, untId, mrk.Value, qun, marketerSellInvoice.Id);
                    var errQaun = (quntityInfo.Info?.Quantity is not null) ? "أقل من " +
                        quntityInfo.Info?.Quantity.ToString() : " غير متوفرة";
                    if (!quntityInfo.CanLoost)
                        throw new InvalidInputException($"لا يمكن بيع الصنف " +
                            $" {itemInfo?.Name ?? "بدون اسم"}. " +
                            $"الكمية المطلوبة" +
                            $"{qun} " +
                            $" {errQaun}");
                }

                var allOnes = selectedOne.TransferRate * qun;
                {

                    #region OLD
                    // await storageModuleService.RegainOrLoseItem(itmId, untId, storesIds.LastOrDefault(), qun, allOnes, itemInfo.Cost,
                    //     avragePiceCost: item.Price / selectedOne.TransferRate,
                    //     price: item.Price,
                    //     relatedId: sellInvoice.Id,
                    //     relatedInventory: RelatedInventory.MarketerSalePreparation_Sale,
                    //     isLose: isLose,
                    //     softDelete: softDelete,
                    //     storageItemProperties: properties
                    // );
                    // await marketerModuleService.RegainOrLoseItem(itmId, untId, mrk.Value, qun, allOnes,
                    //    relatedId: sellInvoice.Id,
                    //    relatedInventory: RelatedInventory.MarketerSalePreparation_Marketer,
                    //    isLose: !isLose,
                    //    softDelete: softDelete,
                    //    storageItemProperties: properties
                    //);
                    // await storageModuleService.RegainOrLoseItem(itmId, untId, storesIds.LastOrDefault(), qun, allOnes, itemInfo.Cost,
                    //     avragePiceCost: item.Price / selectedOne.TransferRate,
                    //     price: item.Price,
                    //     relatedId: sellInvoice.Id,
                    //     relatedInventory: RelatedInventory.MarketerSaleSell,
                    //     isLose: !isLose,
                    //     softDelete: softDelete,
                    //     storageItemProperties: properties
                    // );
                    #endregion

                    await marketerModuleService.RegainOrLoseItem(itmId, untId, mrk.Value, qun, allOnes,
                       relatedId: marketerSellInvoice.Id,
                       relatedInventory: RelatedInventory.MarketerMobile_SellInvoice,
                       isLose: !isLose,
                       softDelete: softDelete,
                       storageItemProperties: properties
                   );

                }
            }
        }
        //
        //
        //
        if (q.IsNew())
        {
            //sellInvoice.Date = DateTime.Now;
            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            marketerSellInvoice.TotalAmount = total;
            marketerSellInvoice.Number = invoiceNumber;
            //sellInvoice.RepresentativePercentage = representativeInfo?.Percentage ?? 0;
            await _service.Add(marketerSellInvoice);
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoMarketerSalesInvoice), marketerSellInvoice.Id);
            //
            //  Items
            await InventiryProcess(marketerSellInvoice.Details);
            //  Financial
            
            await FinacialProcess(totalDiscounted, total);
        }
        else
        {
            invoiceNumber = marketerSellInvoice.Number;
            Logger.LogInformation("updated {0} with id {1}", nameof(DtoMarketerSalesInvoice), marketerSellInvoice.Id);
            await _service.Update(marketerSellInvoice);

            //NEW 
            Logger.LogInformation("removing details {0} for id {1}", nameof(DtoMarketerSalesInvoiceDetail), marketerSellInvoice.Id);
            await invoiceSrviceDetails.Remove(w => w.MarketerSalesInvoiceId == marketerSellInvoice.Id);
            await storageModuleService.LossAllEver(q.Id,
                [RelatedInventory.Sale, RelatedInventory.MarketerSaleSell,
                RelatedInventory.MarketerSalePreparation_Sale]
            );
            await marketerModuleService.LossAllEver(q.Id, RelatedInventory.MarketerSalePreparation_Marketer);
            await entryService.RemoveAllRelated(marketerSellInvoice.Id, OprationTypeEnum.Sales, PurchaseAndSalesConstants.TransactionOprationType__SalesRelated);
            //
            marketerSellInvoice.Details.ForEach(f => f.MarketerSalesInvoiceId = marketerSellInvoice.Id);
            await invoiceSrviceDetails.AddRange(marketerSellInvoice.Details);
            var idsArray = marketerSellInvoice.Details.Select(s => s.Id).ToArray();
            var ids = string.Join(", ", idsArray);
            Logger.LogInformation("saved details {0} with for id {1} with ids: [{2}]", nameof(DtoMarketerSalesInvoiceDetail), marketerSellInvoice.Id, ids);
            //
            //  Items
            if (marketerSellInvoice.Details is not null && marketerSellInvoice.Details.Count() != 0)
            {
                Logger.LogInformation("0x0001 - CommitTransaction");
                await InventiryProcess(marketerSellInvoice.Details);
            }
            //  Financial
            await FinacialProcess(totalDiscounted, total);
        }

        Logger.LogTrace("0x0001 - CommitTransaction");
        var status = _service.CommitTransaction();
        if (!status)
            throw new Exception("cannot proccesed the sale opration");
        var invoiceDb = await _service.GetById(marketerSellInvoice.Id);

        if (clientInfo?.AccountId is not null)
        {
            var multiQuery = await entryService.GetBalance(clientInfo.AccountId.Value, null, null);
            invoiceDb.Balance = multiQuery;
        }
        //****************************(0)****************************
        Logger.LogTrace("items finish saving: {0}", invoiceDb.Details.Count());
        //****************************(0)****************************
        return invoiceDb;
    }

    //[HttpPost("ReturnSales")]
    //public ValueTask<HttpResponseWrapper<DtoMarketerSalesInvoice>> PostReturnSaleInvoice([FromBody] SellReturnInvoiceCreateRequest request)
    //{
    //    return TryOne<DtoMarketerSalesInvoice>(async _ =>
    //    {
    //        /* In return process
    //         * 00. check the invoice number
    //         * 00. GET: the related `sell-return` for same invoice.
    //         * 00. CHECK: the quntities and items if real invoice and releated `sell-return`
    //         * 00. save the master-invoice
    //         * 00. save the details-invoice
    //         * 00. lose the items in inventory flagged with `SellReturn` linked to Return-invoice-id
    //         * 00. return the last-update of the Sell-invoice
    //         * 00. commit the transaction
    //         */
    //        //
    //        var invoice = await _service.GetAsync(w => w.Number == request.Number);
    //        if (invoice is null) return _.Problem(l["Invoice-not-found"]!); // Invoice not found
    //                                                                        //

    //        //
    //        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
    //        if (request.IsNew())
    //        {
    //            /* In return process
    //             * 00. check the invoice number
    //             * 00. GET: the related `sell-return` for same invoice.
    //             * 00. CHECK: the quntities and items if real invoice and releated `sell-return`
    //             * 00. save the master-invoice
    //             * 00. save the details-invoice
    //             * 00. lose the items in inventory flagged with `SellReturn` linked to Return-invoice-id
    //             * 00. return the last-update of the Sell-invoice
    //             * 00. commit the transaction
    //             */

    //        }
    //        else
    //        {
    //            /* In return process
    //             * 00. check the invoice number
    //             * 00. GET: the related `sell-return` for same invoice.
    //             * 00. CHECK: the quntities and items if real invoice and releated `sell-return`
    //             * 00. save the master-invoice
    //             * 00. save the details-invoice
    //             * 00. lose the items in inventory flagged with `SellReturn` linked to Return-invoice-id
    //             * 00. return the last-update of the Sell-invoice
    //             * 00. commit the transaction
    //             */

    //        }
    //        //
    //        //
    //        return _.Success(invoice);
    //    });
    //}


    // VerifyInvoice

    [HttpGet("VerifyInvoice")]
    public ValueTask<HttpResponseWrapper<DtoMarketerSalesInvoice>> GetVerifyInvoice([FromQuery] MarketerSalesInvoiceQueryRequest request)
    {
        return TryOne<DtoMarketerSalesInvoice>(async _ =>
        {
            var invoice = await _service.GetAsync(w => w.Number == request.Number);
            if (invoice is null) return _.Problem(l["Invoice-not-found"]!); // Invoice not found
            return _.Success(invoice);
        });
    }


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

    [HttpGet("Categories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCat([FromQuery] CategoryQueryRequest request)
        =>
        GetSelectableList<DtoCategory, TCategory>(x => x.Name);

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name} • {x.Barcode}",
            request.ItemId != null
            ? x => x.CategoryId == request.ItemId.Value && x.IsActive
            : y => y.IsActive);

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
            Logger.LogTrace("GET: [GetLookupClients] - request : {0}", request.ToJson());
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

    [HttpPost("Report/{id}")]
    public ValueTask<HttpResponseWrapper<FileResponse>> GetSellInvoiceReport(Identifier id, [FromQuery] QueryRequestBase query)
        =>
        TryOne<FileResponse>(async opt =>
        {
            await Task.CompletedTask;
            return opt.Success(new FileResponse()
            {

            });
        });

    [NonAction]
    public virtual void Dispose()
    {
        OnCreateUpdate -= MarketerSalesInvoiceController_OnCreateUpdate;
    }

    //[NonAction]
    //private string? AdjustQuantity(DtoMarketerSalesInvoice dtoMarketerSalesInvoice, DtoSellReturnInvoice[] dtoSellReturnInvoices)
    //{
    //    // TODO: Implement the code for adjusting the quantity
    //    foreach (var invoice in dtoSellReturnInvoices)
    //    {
    //        foreach (var item in invoice.Details)
    //        {
    //            var el = dtoMarketerSalesInvoice.Details.FirstOrDefault(w => w.ItemId == item.ItemId);
    //            if (el is null) continue; // todo: find solution
    //            el.Quantity -= item.Quantity;
    //            if (el.Quantity < 0)
    //                return string.Format("item {0} have {1}, so can not return {2}", el.ItemName, el.Quantity, item.Quantity); // el.Quantity = 0; // todo: find solution
    //        }
    //    }
    //    return null;
    //}
}
