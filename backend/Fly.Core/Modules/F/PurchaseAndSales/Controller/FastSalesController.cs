/*using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using DiscountType = Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType;
using LinqKit;
using System.Data;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class FastSalesController
    : CustomControllerFeatrueBased<DtoSalesInvoice, TSalesInvoice, SalesInvoiceQueryRequest, DeleteSalesInvoiceRequest, FastSaleInvoiceCreateRequest>,
        IFastSalesFeatures
{

    private readonly IConfiguration configuration;
    RelatedInventory relatedInventory = RelatedInventory.FastSale;


    public FastSalesController(ILocalizationService localizationService, ILogger<CustomControllerFeatrueBased<DtoSalesInvoice, TSalesInvoice, SalesInvoiceQueryRequest, DeleteSalesInvoiceRequest, FastSaleInvoiceCreateRequest>> logger, IMediator mediator, IServiceBase<DtoSalesInvoice, TSalesInvoice> service, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapster, IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdateEx += FastSalesController_OnCreateUpdateEx;
        this.configuration = configuration;
    }

    [NonAction]
    private async ValueTask<Framework.Shared.Common.Http.HttpResponseWrapper<DtoSalesInvoice?>> FastSalesController_OnCreateUpdateEx(FastSaleInvoiceCreateRequest q, Framework.Server.Common.ResultOrFailer<DtoSalesInvoice?> opt)
    {
        
        return null;
        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var bankService = _service.MakeChildService<DtoBank, TBank>();
        var entryService = _service.MakeChildService<IEntryService>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var storeService = _service.MakeChildService<DtoStore, TStore>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var marketerSrvice = _service.MakeChildService<DtoMarketer, TMarketer>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var invoiceSrviceDetails = _service.MakeChildService<DtoSalesInvoiceDetail, TSalesInvoiceDetail>();
        
        var items = q.Items.Reverse().GroupBy(w => new { w.UnitId, w.ItemId })
            .Select(g => new SalesInvoiceDetail(g.Key.UnitId, g.Key.ItemId,
                g.Sum(s => s.Quantity), g.Max(g => g.Price),g.Max(g => g.Discount), null))
            .ToList();
        
        SalesInvoiceCreateRequest invoiceRequest = new()
        {
            Date = DateTime.Now,
            ClientId = q.ClientId,
            SafeId = q.SafeId,
            StoreId = q.StoreId,
            StorageType = StorageType.Normal,
            Discount = q.DiscountValue,
            DiscountType = q.IsPercentageBased ? (int)DiscountType.Percent : (int)DiscountType.Static,
            Details = items,
            PeymentType = q.PaymentType switch
            {
                PointOfSalePaymentType.Cash => PeymentType.Cash,
                PointOfSalePaymentType.BankServices => PeymentType.BankServices,
                PointOfSalePaymentType.Deferred => PeymentType.Deferred,
            },
            InvoiceStatus = q.IsOnHold ? InvoiceStatus.InvoiceOnHold : InvoiceStatus.InvoicePaid
        };
        
        var sellInvoice = q.Adapt<DtoSalesInvoice>();
        if (sellInvoice.StoreId is null || sellInvoice.StoreId.Value.IsNullOrEmpty())
            sellInvoice.StoreId = null;
        DtoAccount? saleInvoiceBaseAccount = null;
        var selectedItems = new List<Identifier>();
        var selectedUnits = new List<Identifier>();

        var (actualTotal, netTotal, discountTotal) = sellInvoice.Count();
        var finalTotal = netTotal;
        if (finalTotal < 0)
        {
            throw new Exception("الاجمالي يجب أن يكون أكبر من صفر");
        }

        sellInvoice.NetAmount = netTotal;
        sellInvoice.TotalAmount = actualTotal;
        sellInvoice.DiscountAmount = discountTotal;

        sellInvoice.Details.Distinct().ForEach(f =>
        {
            selectedItems.Add(f.ItemId);
            selectedUnits.Add(f.UnitId);
        });
        
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);

        var clientInfo = await clientService.GetOneAsAsync(x => new { x.AccountId, x.MaxDebt }, w => w.Id == sellInvoice.ClientId);
        if (clientInfo is null)
        {
            sellInvoice.ClientId = null;
        }

        #region Accounts

        switch (sellInvoice.StorageType)
        {
            case StorageType.Normal:
                if (sellInvoice.StoreId is null)
                {
                    throw new InvalidInputException("StoreId not found");
                }
                sellInvoice.MarketerStorageId = null;
                break;
        }

        var representativeInfo = await marketerSrvice.GetOneAsAsync(x => new { x.AccountId, x.Percentage }, w => w.Id == sellInvoice.RepresentativeId);
        if (representativeInfo is null) sellInvoice.RepresentativeId = null;
        var generalGlobalShareAccountConfig = await configService.GetOneAsAsync(s => s.GeneralShareAccount);
        if (generalGlobalShareAccountConfig is null & representativeInfo is not null) throw new Exception("generalGlobalShareAccount not found");
        var generalGlobalShareAccount = await accountService
            .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(generalGlobalShareAccountConfig));
        if (generalGlobalShareAccount is null & representativeInfo is not null) throw new Exception("generalGlobalShareAccount not found.");

        var discountAccountAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
        if (discountAccountAccountConfig is null) throw new Exception("DiscountAccount not found");
        var discountAccountAccount = await accountService
           .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountAccountConfig));
        if (discountAccountAccount is null) throw new Exception("DiscountAccount not found.");

        var safeInfo = await safeService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == sellInvoice.SafeId);
        if (safeInfo is null) sellInvoice.SafeId = null;
        if (safeInfo is null && PeymentType.Cash == sellInvoice.PeymentType)
            throw new InvalidInputException("يرجى أختيار خزينة."); // TODO: Localization
        
        var bankInfo = await bankService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == sellInvoice.BankId);
        if (bankInfo is null) sellInvoice.BankId = null;
        if (bankInfo is null && PeymentType.BankServices == sellInvoice.PeymentType)
            throw new InvalidInputException("يرجى أختيار بنك."); // TODO: Localization
        //
        //  SALE
        var saleAccountId = await configService.GetOneAsAsync(x => x.SalesInvoicesAccountId);
        if (saleAccountId is null)
            throw new Exception("SalesInvoicer account not ready"); // TODO: Localization
        saleInvoiceBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(saleAccountId));
        if (saleInvoiceBaseAccount is null)
            throw new Exception("SalesInvoicer account not ready"); // TODO: Localization
        //
        //
        var generalClientId = await configService.GetOneAsAsync(x => x.GeneralClientAccountAccount);
        if (generalClientId is null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization
        var generalClientAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(generalClientId));
        if (generalClientAccount is null && sellInvoice.ClientId == null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization

        #endregion

        long invoiceNumber = 0;

        async Task<bool> FinacialProcess(decimal _discountedTtoal, decimal _total, bool IsRoleback = false, bool softDelete = false)
        {
            var discountAmount = _total - _discountedTtoal;
            //  1
            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    OprationTypeEnum.Sales,
                    PurchaseAndSalesConstants.TransactionOprationType__SalesRelated,
                    sellInvoice.Id,
                    invoiceNumber,
                   "");
            //
            var theClient = clientInfo?.AccountId ?? generalClientAccount!.Id;
            finacialTransaction.AppendEntry(theClient, saleInvoiceBaseAccount.Id, _discountedTtoal);

            if (discountAmount > 0)
            {
                finacialTransaction.AppendEntry(discountAccountAccount.Id, saleInvoiceBaseAccount.Id, discountAmount);
            }

            switch (sellInvoice.PeymentType)
            {
                case PeymentType.Cash:
                    // NOTE: if the safe has `loss percent` ?
                    finacialTransaction.AppendEntry(safeInfo.AccountId, theClient, _discountedTtoal);
                    break;
                case PeymentType.BankServices:
                    finacialTransaction.AppendEntry(bankInfo.AccountId, theClient, _discountedTtoal);
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
        //
        async Task InventiryProcess(IEnumerable<DtoSalesInvoiceDetail> invoiceDetails, bool isLose = false, bool softDelete = true)
        {

            foreach (var item in invoiceDetails)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = sellInvoice.StoreId;
                var qun = item.Quantity;
                var mrk = sellInvoice.MarketerStorageId;
                var isNormalMode = sellInvoice.StorageType == StorageType.Normal;

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

                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);

                {
                    if (isNormalMode)
                    {
                        var minusSellConfig = configuration.GetSection("MinusSellCheck");
                        if (minusSellConfig is null || minusSellConfig.Get<bool>())
                        {
                            var quntityInfo = await storageModuleService.CanLoseItemQuntityInfo(itmId, untId, strId.Value, qun, properties);
                            var errQaun = (quntityInfo.Info?.Inventory?.Quantity is not null) ? "أقل من " +
                                quntityInfo.Info?.Inventory?.Quantity.ToString() : " غير متوفرة";
                            if (!quntityInfo.CanLoost)
                                throw new InvalidInputException($"لا يمكن بيع الصنف " +
                                $" {itemInfo?.Name ?? "بدون اسم"}. " +
                                $"الكمية المطلوبة" +
                                $"{qun} " +
                                $" {errQaun}");
                        }
                    }
                    else
                    {
                        var quntityInfo = await marketerModuleService.CanLoseItemQuntityInfo(itmId, untId, mrk.Value, qun, sellInvoice.Id);
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
                {
                    if (!isNormalMode)
                    {
                        await storageModuleService.RegainOrLoseItem(itmId, untId, storesIds.LastOrDefault(), qun, allOnes, itemInfo.Cost,
                            avragePiceCost: item.Price / selectedOne.TransferRate,
                            price: item.Price,
                            relatedId: sellInvoice.Id,
                            relatedInventory: RelatedInventory.MarketerSalePreparation_Sale,
                            isLose: isLose,
                            softDelete: softDelete,
                            storageItemProperties: properties
                        );
                        await marketerModuleService.RegainOrLoseItem(itmId, untId, mrk.Value, qun, allOnes,
                           relatedId: sellInvoice.Id,
                           relatedInventory: RelatedInventory.MarketerSalePreparation_Marketer,
                           isLose: !isLose,
                           softDelete: softDelete,
                           storageItemProperties: properties
                       );
                        await storageModuleService.RegainOrLoseItem(itmId, untId, storesIds.LastOrDefault(), qun, allOnes, itemInfo.Cost,
                            avragePiceCost: item.Price / selectedOne.TransferRate,
                            price: item.Price,
                            relatedId: sellInvoice.Id,
                            relatedInventory: RelatedInventory.MarketerSaleSell,
                            isLose: !isLose,
                            softDelete: softDelete,
                            storageItemProperties: properties
                        );
                    }
                    else
                    {
                        //  todo: check devided by zero issue
                        await storageModuleService.RegainOrLoseItem(itmId, untId, strId.Value, qun, allOnes,

                            cost: itemInfo.Cost,
                            avragePiceCost: item.Price / selectedOne.TransferRate,
                            price: item.Price,

                            relatedId: sellInvoice.Id,
                            relatedInventory: relatedInventory,
                            isLose: !isLose,
                            softDelete: softDelete,
                            storageItemProperties: properties
                        );
                    }
                }
            }
        }

        if (q.IsNew())
        {
            sellInvoice.Date = DateTime.Now;

            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            sellInvoice.Number = invoiceNumber;
            sellInvoice.RepresentativePercentage = representativeInfo?.Percentage ?? 0;
            await _service.Add(sellInvoice);
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoSalesInvoice), sellInvoice.Id);

            await InventiryProcess(sellInvoice.Details);

            if (q.InvoiceStatus is InvoiceStatus.InvoicePaid)
            {
                await FinacialProcess(finalTotal, actualTotal);
            }
        }
        else
        {
            invoiceNumber = sellInvoice.Number;
            var old = await _service.GetById(sellInvoice.Id);
            await _service.Update(sellInvoice);

            await invoiceSrviceDetails.Remove(w => w.SalesInvoiceId == sellInvoice.Id);
            await storageModuleService.LossAllEver(q.Id,
                [RelatedInventory.Sale, RelatedInventory.MarketerSaleSell,
                RelatedInventory.MarketerSalePreparation_Sale]
            );
            
            await marketerModuleService.LossAllEver(q.Id, RelatedInventory.MarketerSalePreparation_Marketer);
            await entryService.RemoveAllRelated(sellInvoice.Id, OprationTypeEnum.Sales, PurchaseAndSalesConstants.TransactionOprationType__SalesRelated);
            sellInvoice.Details.ForEach(f => f.SalesInvoiceId = sellInvoice.Id);
            await invoiceSrviceDetails.AddRange(sellInvoice.Details);

            if (sellInvoice.Details is not null && sellInvoice.Details.Count != 0)
            {
                await InventiryProcess(sellInvoice.Details);
            }
            
            if (old.InvoiceStatus is InvoiceStatus.InvoicePaid && q.InvoiceStatus == InvoiceStatus.InvoiceOnHold)
            {
                throw new InvalidInputException("لا يمكن تعليق فاتورة تم دفعها. يرجى أنشاء فاتورة أرجاع مبيعات."); // TODO: Localization
            }
            
            await FinacialProcess(finalTotal, actualTotal);
        }
        //
        //
        //
        Logger.LogTrace("0x0001 - CommitTransaction");
        var status = _service.CommitTransaction();
        if (!status)
            throw new InvalidInputException("Sales operation failed. Contact support.");
        var invoiceDb = await _service.GetById(sellInvoice.Id);
        //
        //
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
    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes([FromQuery] SafeQueryRequest request)
        => GetSelectableList<DtoSafe, TSafe>(x => x.Name);

    [HttpGet("Banks")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupBanks([FromQuery] BankQueryRequest request)
        => GetSelectableList<DtoBank, TBank>(x => x.Name);
    
    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores([FromQuery] BankQueryRequest request)
        => GetSelectableList<DtoStore, TStore>(x => x.Name);
    
    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients([FromQuery] BankQueryRequest request)
        => GetSelectableList<DtoClient, TClient>(x => x.Name);
}*/