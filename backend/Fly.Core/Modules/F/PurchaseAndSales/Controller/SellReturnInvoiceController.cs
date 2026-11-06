//, returnTotal = 0m;).GetAwaiter().GetResult();

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
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class SellReturnInvoiceController
    :
        CustomControllerFeatrueBased<DtoSellReturnInvoice, TSellReturnInvoice, SellReturnInvoiceQueryRequest,
            DeleteSellReturnInvoiceRequest, SellReturnInvoiceCreateRequest>,
        SellReturnInvoiceFeatures
{

    private readonly IConfiguration _configuration;


    public SellReturnInvoiceController(ILocalizationService localizationService,
        ILogger<CustomControllerFeatrueBased<DtoSellReturnInvoice, TSellReturnInvoice, SellReturnInvoiceQueryRequest,
            DeleteSellReturnInvoiceRequest, SellReturnInvoiceCreateRequest>> logger, IMediator mediator,
        IServiceBase<DtoSellReturnInvoice, TSellReturnInvoice> service, IMapper mapper,
        MapsterMapper.IMapper mapster, IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _configuration = configuration;
        OnDeleteEx += SellReturnInvoiceController_OnDeleteEx;
        OnCreateUpdateEx += SellReturnInvoiceController_OnCreateUpdateEx;
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
        OnDeleteEx -= SellReturnInvoiceController_OnDeleteEx;
        OnCreateUpdateEx -= SellReturnInvoiceController_OnCreateUpdateEx;
    }

    private sealed record SourceLine(Identifier ItemId, Identifier UnitId, decimal Quantity, decimal Discount);

    /// <summary>The sales invoice as sold, before any returns reduce its quantities.</summary>
    private sealed record InvoiceSnapshot(decimal DetailsTotal, List<SourceLine> Lines);

    /// <summary>
    /// Take this before AdjustQuantity, which reduces the invoice quantities.
    /// DetailsTotal matches the sales invoice: price x quantity minus the line's
    /// fixed discount.
    /// </summary>
    private static InvoiceSnapshot Snapshot(DtoSalesInvoice invoice)
        => new(invoice.Details.Sum(d => d.CountItemPrice(true)),
               invoice.Details.Select(d => new SourceLine(d.ItemId, d.UnitId, d.Quantity, d.Discount)).ToList());

    /// <summary>
    /// The return table has no discount columns, so a return linked to a sales
    /// invoice takes its discounts from that invoice: each line gets its share of
    /// the line discount, and the return gets its share of a fixed invoice
    /// discount (a percentage applies as is). Returning a whole invoice therefore
    /// refunds exactly what the customer paid.
    /// </summary>
    private static void ApplyInvoiceDiscount(DtoSellReturnInvoice ret, DtoSalesInvoice invoice, InvoiceSnapshot source)
    {
        ret.Discount = invoice.Discount;
        ret.DiscountType = invoice.DiscountType;
        ret.SourceInvoiceDetailsTotal = source.DetailsTotal;

        foreach (var line in ret.Details)
        {
            var sold = source.Lines.FirstOrDefault(l => l.ItemId == line.ItemId && l.UnitId == line.UnitId);
            if (sold is null)
                continue;

            line.SourceQuantity = sold.Quantity;
            line.SourceDiscount = sold.Discount;
            line.Discount = line.LineDiscount(); // stored as this line's share
        }

        ret.TotalAmount = decimal.Round(ret.Count().discountedTotal, 2);
    }

    private async Task ApplyInvoiceDiscountAsync(IEnumerable<DtoSellReturnInvoice> returns)
    {
        var linked = returns.Where(r => r.InvoiceId is not null).ToList();
        if (linked.Count == 0)
            return;

        var ids = linked.Select(r => r.InvoiceId!).Distinct().ToArray();
        var sellService = _service.MakeChildService<DtoSalesInvoice, TSalesInvoice>();
        var invoices = (await sellService.GetAllAsync(w => ids.Contains(w.Id))).Items.ToList();

        foreach (var ret in linked)
        {
            var invoice = invoices.FirstOrDefault(i => i.Id == ret.InvoiceId);
            if (invoice is not null)
                ApplyInvoiceDiscount(ret, invoice, Snapshot(invoice));
        }
    }

    public override async Task BeforReturn(PageList<DtoSellReturnInvoice> list)
    {
        // TotalAmount has no database column, so compute it for the list
        // (the same net total used when the return is saved).
        foreach (var item in list.Items)
        {
            item.TotalAmount = decimal.Round(item.Count().discountedTotal, 2);
        }

        // Returns made from a sales invoice: apply that invoice's discount.
        await ApplyInvoiceDiscountAsync(list.Items);

        return;
        var entryService = _service.MakeChildService<IEntryService>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var sellService = _service.MakeChildService<DtoSalesInvoice, TSalesInvoice>();

        var invoicesNumbers = list.Items.Select(W => W.InvoiceNumber);
        var sellInvoices = await sellService.GetAllAsync(w => invoicesNumbers.Contains(w.Number));

        var arr = sellInvoices.Items.Where(w => w.ClientId != null).Select(w => w.ClientId.Value).Distinct().ToArray();
        var accArr = await clientService.GetAllAsAsync(s => new { s.AccountId, s.Id }, w => arr.Contains(w.Id));
        var accArrIds = accArr.Items.Select(s => s.AccountId).ToArray();
        var multiQuery = await entryService.GetBalance(accArrIds, null, null);
        var nonSupplier = list.Items.Where(w => w.ClientId == null).ToList();
        //var nonSellInvoice = list.Items.Where(w => w.InvoiceId == null).ToList();
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
        List<DtoSellReturnInvoice> all = [.. nonSupplier, .. list2/*, .. nonSellInvoice*/];
        list.Items = [.. all.OrderByDescending(s => s.Number)];
    }

    private async ValueTask<HttpResponseWrapper<DtoSellReturnInvoice?>> SellReturnInvoiceController_OnDeleteEx(DeleteSellReturnInvoiceRequest q, Framework.Server.Common.ResultOrFailer<DtoSellReturnInvoice?> _)
    {
        var entryService = _service.MakeChildService<EntryService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var sellreturnDetailsService = _service.MakeChildService<DtoSellReturnInvoiceDetail, TSellReturnInvoiceDetail>();
        //
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        var returnInvoice = await _service.GetById(q.Id);
        await sellreturnDetailsService.Remove(w => w.ReturnInvoiceId == returnInvoice.Id);
        await storageModuleService.LossAllEver(returnInvoice.Id, [RelatedInventory.ReturnSale]);
        await entryService.RemoveAllRelated(returnInvoice.Id, OprationTypeEnum.Sales_Return, PurchaseAndSalesConstants.TransactionOprationType__SalesRelated);
        //
        await _service.Remove(returnInvoice.Id);
        var status = _service.CommitTransaction();
        if (!status)
            return _.Problem("cannot proccesed the sale opration");
        return _.Success(returnInvoice)!;
    }

    [HttpGet("ByNumber")]
    public ValueTask<HttpResponseWrapper<DtoSellReturnInvoice?>> GetInvoiceByNumber([FromQuery] string number)
        => TryOne<DtoSellReturnInvoice?>(async (opt) =>
            {
                if (!long.TryParse(number, out var num))
                    return opt.Problem("Wrong number"); // locale
                
                var next = await _service.GetOneAsAsync(x => x, y => y.Number == num);
                if (next is not null)
                    await ApplyInvoiceDiscountAsync([next]);
                return opt.Success(next);
            });
    
    [HttpPost("ReturnByNumber")]
    public ValueTask<HttpResponseWrapper<DtoSellReturnInvoice?>> Create(TypeOf<long> q)
    => TryOne<DtoSellReturnInvoice?>(async _ =>
    {

        var sellInvoiceService = _service.MakeChildService<DtoSalesInvoice, TSalesInvoice>();

        var invoice = await sellInvoiceService.GetAsync(w => w.Number == q.Value);
        if (invoice is null) return _.Problem(l["Invoice-not-found"]!); // Invoice not found

        var nextNumber = await _service.NextNumber(p => p.Number);
        DtoSellReturnInvoice sellReturnInvoice = new()
        {
            Date = invoice.Date,
            Number = nextNumber,
            SafeId = invoice.SafeId,
            StoreId = invoice.StoreId,
            ClientId = invoice.ClientId,
            InvoiceNumber = invoice.Number,
            Discount = invoice.Discount,

            DiscountType = invoice.DiscountType,

            InvoiceId = invoice.Id,
            PeymentType = invoice.PeymentType,
            StorageType = invoice.StorageType,
            SerialNumber = invoice.SerialNumber,
            MarketerStorageId = invoice.MarketerStorageId,

            //  TODO: ADD FLAG FOR RETURNED, TO NOT BE LOADED AGAIN
            Details = invoice.Details
                .Select(x => new DtoSellReturnInvoiceDetail
                {
                    Price = x.Price,
                    UnitId = x.UnitId,
                    ItemId = x.ItemId,
                    Discount = x.Discount,
                    Quantity = x.Quantity,
                    ExpireDate = x.ExpireDate,
                }).ToList(),
        };

        return await SellReturnInvoiceController_OnCreateUpdateEx(sellReturnInvoice.Adapt<SellReturnInvoiceCreateRequest>(), _);
    });

    public override Expression<Func<DtoSellReturnInvoice, bool>> GetWhere(Expression<Func<DtoSellReturnInvoice, bool>> expression, SellReturnInvoiceQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {

            var isNumber = int.TryParse(query.SearchText, out var number);
            expression = expression.And(
                x =>
                x.Details.Any(d => d.UnitName.Contains(query.SearchText)) |
                x.Details.Any(d => d.ItemName.Contains(query.SearchText)) |
                x.Number.ToString().Contains(query.SearchText) |
                x.ClientName.ToString().Contains(query.SearchText) |
                x.SafeName.Contains(query.SearchText) |
                x.TotalAmount.ToString() == query.SearchText |
                x.SerialNumber.ToString() == query.SearchText |
                x.ClientName.Contains(query.SearchText)
            );
        }
        return base.GetWhere(expression, query);
    }

    private async ValueTask<HttpResponseWrapper<DtoSellReturnInvoice?>> SellReturnInvoiceController_OnCreateUpdateEx(SellReturnInvoiceCreateRequest request, Framework.Server.Common.ResultOrFailer<DtoSellReturnInvoice?> _)
    {
        //
        //
        //
        var entryService = _service.MakeChildService<EntryService>();
        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var storeService = _service.MakeChildService<DtoStore, TStore>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var service = _service.MakeChildService<DtoSellReturnInvoice, TSellReturnInvoice>();
        var sellInvoiceService = _service.MakeChildService<DtoSalesInvoice, TSalesInvoice>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var sellreturnDetailsService = _service.MakeChildService<DtoSellReturnInvoiceDetail, TSellReturnInvoiceDetail>();
        //
        //
        //
        long number = 0L;
        decimal defTotal = 0m, returnDetailsNonDiscountedTotal = 0m,
            returnDetailsTotal = 0, returnTotal = 0,
            returnInvoiceTotalDiscount = 0;
        //
        //****************************(0)****************************
        var invoice = await sellInvoiceService.GetAsync(w => w.Number == request.InvoiceNumber);
        var source = invoice is null ? null : Snapshot(invoice);
        var otherReturns = await service.GetAllAsync(w => w.InvoiceNumber == request.InvoiceNumber && w.Id != request.Id);
        if (invoice is not null && otherReturns.Items.Any())
        {
            var result = AdjustQuantity(invoice, otherReturns.Items.ToArray());
            if (result is not null) return _.Problem(result);
        }
        request.Details = request.Details
            .GroupBy(w => new { w.UnitId, w.ItemId, w.ExpireDate })
            .Select(g => new SellReturnInvoiceDetail(g.Key.UnitId, g.Key.ItemId, g.Sum(s => s.Quantity), g.Max(g => g.Price), g.Max(g => g.Discount), g.Key.ExpireDate))
            .ToList();
        //****************************(0)****************************
        var returnInvoice = request.Adapt<DtoSellReturnInvoice>();
        var (actualTotal, netTotal, discountTotal) = returnInvoice.Count();
        if (netTotal < 0)
        {
            throw new InvalidInputException("الاجمالي يجب أن يكون أكبر من صفر");
        }
        //
        if (invoice is not null)
        {
            returnInvoice.Details.ForEach(f =>
            {
                var i = invoice.Details.FirstOrDefault(ff => ff.ItemId == f.ItemId && ff.UnitId == f.UnitId) ?? throw new Exception("Item Not exists in real invoice");
                f.Price = i.Price;
                f.Discount = i.Discount;
            });
            ApplyInvoiceDiscount(returnInvoice, invoice, source!);
            var resultx = AdjustQuantity(invoice, [returnInvoice]);
            if (resultx is not null) return _.Problem(System.Net.HttpStatusCode.BadRequest, resultx);
            //
            returnInvoice.InvoiceId = invoice.Id;
            returnInvoice.SafeId = invoice.SafeId;
            returnInvoice.StoreId = invoice.StoreId;
            returnInvoice.ClientId = invoice.ClientId;


            // Prices were replaced with the invoice's above, so recount from them.
            // A fixed invoice discount is shared in proportion to what is returned
            // (it used to be subtracted in full from every partial return).
            (returnDetailsNonDiscountedTotal, returnTotal, returnInvoiceTotalDiscount) = returnInvoice.Count();
            returnDetailsTotal = returnInvoice.Details.Sum(s => s.CountingPrince((int)returnInvoice.DiscountType));
            defTotal = invoice.NetAmount - netTotal;

            number = invoice.Number;
        }
        else
        {
            defTotal = netTotal;
            returnTotal = netTotal;
            returnInvoiceTotalDiscount = discountTotal;
            number = await _service.NextNumber(s => s.Number);
        }
        //
        // A return not linked to a sales invoice has no store to take, and the
        // inventory step below needs one (it used to fail with
        // "Nullable object must have a value").
        if (invoice is null
            && returnInvoice.StorageType == StorageType.Normal
            && (returnInvoice.StoreId is null || returnInvoice.StoreId.Value.IsNullOrEmpty()))
        {
            throw new InvalidInputException("رجاءً قم بتحديد المخزن"); // Localized
        }
        //
        //
        //-----------------
        //-----------------
        service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        //
        var generalClientId = await configService.GetOneAsAsync(x => x.GeneralClientAccountAccount);
        if (generalClientId is null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization
        var generalClientAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(generalClientId));
        if (generalClientAccount is null && returnInvoice.ClientId == null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization
        var clientInfo = await clientService.GetOneAsAsync(x => new { x.AccountId, x.MaxDebt }, w => w.Id == returnInvoice.ClientId);
        if (clientInfo is null)
        {
            returnInvoice.ClientId = null;
        }
        //
        //  check invoice.clientId
        var saleAccountId = await configService.GetOneAsAsync(x => x.SalesInvoicesAccountId);
        var saleInvoiceBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(saleAccountId));
        if (saleInvoiceBaseAccount is null) throw new Exception("DiscountAccount not found");
        //
        //
        //  SAFE
        var safeInfo = await safeService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == returnInvoice.SafeId);
        if (safeInfo is null) returnInvoice.SafeId = null;
        if (safeInfo is null && PeymentType.Cash == returnInvoice.PeymentType)
            throw new InvalidInputException("Please Select Safe"); // TODO: Localization
        //
        //
        var discountAccountAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
        if (discountAccountAccountConfig is null) throw new Exception("DiscountAccount not found");
        var discountAccountAccount = await accountService
           .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountAccountConfig));
        if (discountAccountAccount is null) throw new Exception("DiscountAccount not found.");
        //
        //
        var storesIds = (await storeService.GetAllAsAsync(s => s.Id)).Items.ToArray();
        //
        async Task<bool> FinacialProcess(decimal _discountedTtoal, decimal _total, bool IsRoleback = false, bool reverseEntry = false)
        {
            //  1
            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    OprationTypeEnum.Sales_Return,
                    PurchaseAndSalesConstants.TransactionOprationType__SalesRelated,
                    returnInvoice.Id,
                    number,
                   "");
            //
            var theClient = clientInfo?.AccountId ?? generalClientAccount!.Id;
            finacialTransaction.AppendEntry(saleInvoiceBaseAccount.Id, theClient, (_total + _discountedTtoal));

            if (_discountedTtoal > 0)
            {
                finacialTransaction.AppendEntry(saleInvoiceBaseAccount.Id, discountAccountAccount.Id, _discountedTtoal);
            }

            switch (returnInvoice.PeymentType)
            {
                case PeymentType.Cash:
                    // NOTE: if the safe has `loss percent` ?
                    finacialTransaction.AppendEntry(theClient, safeInfo.AccountId, _total);
                    break;
                case PeymentType.Deferred:
                    break;
            }

            if (IsRoleback)
            {
                await finacialTransaction.RevertTransaction(note: "", reverseEntry);
            }
            else
            {
                await finacialTransaction.ComplateTransaction();
            }
            return true;
        }
        //
        async Task InventiryProcess(IEnumerable<DtoSellReturnInvoiceDetail> invoiceDetails, bool isLose = true, bool softDelete = true)
        {
            foreach (var item in invoiceDetails)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = returnInvoice.StoreId;
                var qun = item.Quantity;
                var mrk = returnInvoice.MarketerStorageId;
                var isNormalMode = returnInvoice.StorageType == StorageType.Normal;

                var expire = item.ExpireDate;
                var useExpire = item.ExpireDate != null;

                var properties = new Shared.Modules.Store.Dtos.Extra.StorageItemProperties()
                {
                    ExpireDate = expire,
                };

                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                if (selectedOne is null)
                {
                    Logger.LogWarning("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }

                //var selectedOne = selectedUnitInfo.First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);

                //  CHECK-QUNTITY
                //{
                //    if (isNormalMode)
                //    {
                //        var minusSellConfig = configuration.GetSection("MinusSellCheck");
                //        if (minusSellConfig is null || minusSellConfig.Get<bool>())
                //        {
                //            var quntityInfo = await storageModuleService.CanLoseItemQuntityInfo(itmId, untId, strId.Value, qun, properties);
                //            var errQaun = (quntityInfo.Info?.Inventory?.Quantity is not null) ? "أقل من " +
                //                quntityInfo.Info?.Inventory?.Quantity.ToString() : " غير متوفرة";
                //            if (!quntityInfo.CanLoost)
                //                throw new InvalidInputException($"لا يمكن بيع الصنف " +
                //                $" {itemInfo?.Name ?? "بدون اسم"}. " +
                //                $"الكمية المطلوبة" +
                //                $"{qun} " +
                //                $" {errQaun}");
                //        }
                //    }
                //    else
                //    {
                //        var quntityInfo = await marketerModuleService.CanLoseItemQuntityInfo(itmId, untId, mrk.Value, qun);
                //        var errQaun = (quntityInfo.Info?.Quantity is not null) ? "أقل من " +
                //            quntityInfo.Info?.Quantity.ToString() : " غير متوفرة";
                //        if (!quntityInfo.CanLoost)
                //            throw new InvalidInputException($"لا يمكن بيع الصنف " +
                //                $" {itemInfo?.Name ?? "بدون اسم"}. " +
                //                $"الكمية المطلوبة" +
                //                $"{qun} " +
                //                $" {errQaun}");
                //    }
                //}

                var allOnes = selectedOne.TransferRate * qun;
                {
                    if (!isNormalMode)
                    {
                        await storageModuleService.RegainOrLoseItem(itmId, untId, storesIds.LastOrDefault(), qun, allOnes, itemInfo.Cost,
                            avragePiceCost: item.Price / selectedOne.TransferRate,
                            price: item.Price,
                            relatedId: returnInvoice.Id,
                            relatedInventory: RelatedInventory.MarketerSalePreparationUndo,
                            isLose: isLose,
                            softDelete: softDelete,
                            storageItemProperties: properties
                        );
                        await marketerModuleService.RegainOrLoseItem(itmId, untId, mrk.Value, qun, allOnes,
                           relatedId: returnInvoice.Id,
                           relatedInventory: RelatedInventory.MarketerSalePreparationUndo,
                           isLose: !isLose,
                           softDelete: softDelete
                       );
                        await storageModuleService.RegainOrLoseItem(itmId, untId, storesIds.LastOrDefault(), qun, allOnes, itemInfo.Cost,
                            avragePiceCost: item.Price / selectedOne.TransferRate,
                            price: item.Price,
                            relatedId: returnInvoice.Id,
                            relatedInventory: RelatedInventory.ReturnSale,
                            isLose: !isLose,
                            softDelete: softDelete
                        );
                    }
                    else
                    {

                        await storageModuleService.RegainOrLoseItem(itmId, untId, strId.Value, qun, allOnes, itemInfo.Cost,
                            avragePiceCost: item.Price / selectedOne.TransferRate,
                            price: item.Price,
                            relatedId: returnInvoice.Id,
                            relatedInventory: RelatedInventory.ReturnSale,
                            isLose: !isLose,
                            softDelete: softDelete,
                            storageItemProperties: properties
                        );
                    }

                }
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
            number = await _service.NextNumber(n => n.Number);
            returnInvoice.Number = number;
            await _service.Add(returnInvoice);
            //await FinacialProcess( invoice.TotalAmount, true, false, @return: false);
            await InventiryProcess(returnInvoice.Details);
            await FinacialProcess(returnInvoiceTotalDiscount, returnTotal);
        }
        else
        {
            //
            await sellreturnDetailsService.Remove(w => w.ReturnInvoiceId == returnInvoice.Id);
            await storageModuleService.LossAllEver(returnInvoice.Id, RelatedInventory.ReturnSale);
            await entryService.RemoveAllRelated(returnInvoice.Id, OprationTypeEnum.Sales_Return, PurchaseAndSalesConstants.TransactionOprationType__SalesRelated);
            returnInvoice.Details.ForEach(f => f.ReturnInvoiceId = returnInvoice.Id);
            await sellreturnDetailsService.AddRange(returnInvoice.Details);
            //  --
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoSellReturnInvoice), returnInvoice.Id);
            await service.Update(returnInvoice);
            var idsArray = returnInvoice.Details.Select(s => s.Id).ToArray();
            var ids = string.Join(", ", idsArray);
            //  --
            Logger.LogTrace("saved details {0} with for id {1} with ids: [{2}]", nameof(DtoSalesInvoiceDetail), returnInvoice.Id, ids);
            //
            await InventiryProcess(returnInvoice.Details);
            await FinacialProcess(returnInvoiceTotalDiscount, returnTotal);
        }
        //
        service.CommitTransaction();
        returnInvoice = await _service.GetById(returnInvoice.Id);
        if (invoice is not null && returnInvoice is not null)
        {
            returnInvoice.SellInvoice = invoice;
            ApplyInvoiceDiscount(returnInvoice, invoice, source!);
        }
        var allReturns = otherReturns.Items.Append(returnInvoice);
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
    public ValueTask<HttpResponseWrapper<DtoSellReturnInvoice>> GetVerifyInvoice(
        [FromQuery] SalesInvoiceQueryRequest request)
        => TryOne<DtoSellReturnInvoice>(async _ =>
        {
            var sellInvoiceService = _service.MakeChildService<DtoSalesInvoice, TSalesInvoice>();
            var invoice = await sellInvoiceService.GetAsync(w => w.Number == request.Number);
            if (invoice is null) return _.Problem(System.Net.HttpStatusCode.BadRequest, l["Invoice-not-found"]!); // Invoice not found
            var source = Snapshot(invoice);

            var returnInvoices = await _service.GetAllAsync(w => w.InvoiceNumber == request.Number);
            if (returnInvoices.Items.Any())
            {
                var result = AdjustQuantity(invoice, returnInvoices.Items.ToArray());
                if (result is not null) return _.Problem(System.Net.HttpStatusCode.BadRequest, result);
            }
            if (invoice.Details.All(x => x.Quantity == 0))
                return _.Problem(System.Net.HttpStatusCode.BadRequest, l["already-returned"]!); // Invoice details quantity is 0

            var nextNumber = await _service.NextNumber(p => p.Number);
            DtoSellReturnInvoice sellReturnInvoice = new()
            {
                Date = invoice.Date,
                Number = nextNumber,
                SafeId = invoice.SafeId,
                StoreId = invoice.StoreId,
                ClientId = invoice.ClientId,
                InvoiceNumber = invoice.Number,
                Discount = invoice.Discount,
                DiscountType = invoice.DiscountType,

                InvoiceId = invoice.Id,
                PeymentType = invoice.PeymentType,
                StorageType = invoice.StorageType,
                SerialNumber = invoice.SerialNumber,
                MarketerStorageId = invoice.MarketerStorageId,


                Details = invoice.Details
                .Where(w => w.Quantity > 0)
                .Select(x => new DtoSellReturnInvoiceDetail
                {
                    Price = x.Price,
                    UnitId = x.UnitId,
                    ItemId = x.ItemId,
                    Discount = x.Discount,
                    Quantity = x.Quantity,
                    ExpireDate = x.ExpireDate,
                }).ToList(),
            };

            ApplyInvoiceDiscount(sellReturnInvoice, invoice, source);
            return _.Success(sellReturnInvoice);
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

    private string? AdjustQuantity(DtoSalesInvoice dtoSalesInvoice, DtoSellReturnInvoice[] dtoSellReturnInvoices)
    {
        // TODO: Implement the code for adjusting the quantity
        foreach (var invoice in dtoSellReturnInvoices)
        {
            foreach (var item in invoice.Details)
            {
                var el = dtoSalesInvoice.Details.FirstOrDefault(w => w.ItemId == item.ItemId);
                if (el is null) continue; // todo: find solution
                el.Quantity -= item.Quantity;
                if (el.Quantity < 0)
                    return string.Format("item {0} have {1}, so can not return {2}", el.ItemName, el.Quantity, item.Quantity); // el.Quantity = 0; // todo: find solution
            }
        }
        return null;
    }
}