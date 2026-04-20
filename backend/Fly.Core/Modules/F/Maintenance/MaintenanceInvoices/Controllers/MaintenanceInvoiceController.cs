using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Models.DbModels;
using Fly.Core.Modules.F.Maintenance.Services;
using Fly.Core.Modules.F.PurchaseAndSales;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Enums;
using Fly.Shared.Modules.Maintenance.Features;
using Fly.Shared.Modules.Maintenance.Requests;
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

namespace Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.Controllers;

public class MaintenanceInvoiceController
    : CustomControllerFeatrueBased<DtoMaintenanceInvoice,
            TMaintenanceInvoice, MaintenanceInvoiceQueryRequest,
            MaintenanceInvoiceDeleteRequest,
            MaintenanceInvoiceCreateUpdateRequest>,
        IMaintenanceInvoiceFeatures, IDisposable
{
    private readonly IConfiguration _configuration;

    public MaintenanceInvoiceController(ILocalizationService localizationService,
        ILogger<MaintenanceInvoiceController> logger,
        IMediator mediator, IMaintenanceInvoiceService service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster,
        IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _configuration = configuration;
        OnCreateUpdate += MaintenanceInvoiceControllerOnCreateUpdate;
    }

    public override Expression<Func<DtoMaintenanceInvoice, bool>> GetWhere(
        Expression<Func<DtoMaintenanceInvoice, bool>> expression,
        MaintenanceInvoiceQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Number.ToString().Contains(query.SearchText) |
                x.MaintenanceSectorName.Contains(query.SearchText) |
                x.MaintenanceStatus.ToString().Contains(query.SearchText) |
                x.SafeName.Contains(query.SearchText) |
                x.ClientName.Contains(query.SearchText) |
                x.StoreName.Contains(query.SearchText) |
                x.Details.Any(u => u.ItemName.Contains(query.SearchText))
            );
        
        return base.GetWhere(expression, query);
    }

    [HttpGet("Number")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber()
        =>
            TryOne<long>(async (opt) =>
            {
                var next = await _service.NextNumber(x => x.Number);
                return opt.Success(next);
            });
    
    [HttpGet("ByNumber")]
    public ValueTask<HttpResponseWrapper<DtoMaintenanceInvoice?>> GetInvoiceByNumber([FromQuery] string number)
        =>
            TryOne<DtoMaintenanceInvoice?>(async (opt) =>
            {
                if (!long.TryParse(number, out var num))
                    return opt.Problem("Wrong number"); // locale
                
                var next = await _service.GetOneAsAsync(x => x, y => y.Number == num);
                return opt.Success(next);
            });
    
    private async ValueTask<DtoMaintenanceInvoice> MaintenanceInvoiceControllerOnCreateUpdate(MaintenanceInvoiceCreateUpdateRequest q)
    {
        var maintenanceDetailService = _service.MakeChildService<DtoMaintenanceInvoiceDetail, TMaintenanceInvoiceDetail>();
        var maintenanceCostService = _service.MakeChildService<DtoMaintenanceInvoiceCost, TMaintenanceInvoiceCost>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var entryService = _service.MakeChildService<IEntryService>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();

        var mainInvoice = q.Adapt<DtoMaintenanceInvoice>();
        var (actualTotal, netTotal, discountTotal) = mainInvoice.Count();
        var finalTotal = netTotal;
        if (finalTotal < 0)
        {
            throw new InvalidInputException("الاجمالي يجب أن يكون أكبر من صفر");
        }
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var manuService = _service.MakeChildService<DtoMaintenanceSector, TMaintenanceSector>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();

        var safeInfo = await safeService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == mainInvoice.SafeId);
        if (safeInfo is null) mainInvoice.SafeId = null;
        if (safeInfo is null && PeymentType.Cash == mainInvoice.PaymentType)
            throw new InvalidInputException("يرجى أختيار خزينة."); // TODO: Localization
        
        var manuInfo = await manuService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == mainInvoice.MaintenanceSectorId);
        if (manuInfo is null) mainInvoice.MaintenanceSectorId = null;
        if (manuInfo is null)
        {
            mainInvoice.IsInternal = true;
            var internalMain = await configService.GetOneAsAsync(s => s.InternalMaintenanceSectorsAccount);
            manuInfo = new { AccountId =  Identifier.Parse(internalMain) };
        }
                 
        var bankService = _service.MakeChildService<DtoBank, TBank>();

        var bankInfo = await bankService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == mainInvoice.BankId);
        if (bankInfo is null) mainInvoice.BankId = null;
        if (bankInfo is null && PeymentType.BankServices == mainInvoice.PaymentType)
            throw new InvalidInputException("يرجى أختيار بنك."); // TODO: Localization
        
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        //
        var discountAccountAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
        if (discountAccountAccountConfig is null) throw new Exception("DiscountAccount not found");
        var generalClientAccountConfig = await configService.GetOneAsAsync(s => s.GeneralClientAccountAccount);
        if (generalClientAccountConfig is null) throw new Exception("ClientAccount not found");
        var generalClientAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(generalClientAccountConfig));
        if (generalClientAccount is null && mainInvoice.ClientId == null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization
        var clientInfo = await clientService.GetOneAsAsync(x => new { x.AccountId, x.MaxDebt }, w => w.Id == mainInvoice.ClientId);
        if (clientInfo is null)
        {
            mainInvoice.ClientId = null;
        }
        
        var discountAccountAccount = await accountService
            .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountAccountConfig));
        if (discountAccountAccount is null) throw new Exception("DiscountAccount not found.");
        long invoiceNumber;
        bool payInAdvanceThisTime = false;

        if (mainInvoice.IsNew())
        {
            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            mainInvoice.Number = invoiceNumber;
            var receivedInAdvance = mainInvoice.MaintenanceStatus
                is MaintenanceStatus.ReceivedFeeInAdvance && mainInvoice.CollectionRate > 0;

            if (receivedInAdvance)
            {
                mainInvoice.ReceivedFeeInAdvance = true;
                payInAdvanceThisTime = true;
            }
            await _service.Add(mainInvoice);
            await InventoryProcess(mainInvoice.Details, true);
            await FinancialProcess(discountTotal, actualTotal, payInAdvance: payInAdvanceThisTime);
        }
        else
        {
            var db = await _service.GetById(mainInvoice.Id);
            await maintenanceDetailService.Remove(w => w.MaintenanceInvoiceId == db.Id, false);
            var all = mainInvoice.Details.ToList();
            mainInvoice.Adapt(db);
            invoiceNumber = mainInvoice.Number;
            all.ForEach(f => f.MaintenanceInvoiceId = db.Id);
            await maintenanceDetailService.AddRange(all);
            
            await maintenanceCostService.Remove(w => w.MaintenanceInvoiceId == db.Id, false);
            var allCost = mainInvoice.Costs.ToList();
            allCost.ForEach(f => f.MaintenanceInvoiceId = db.Id);
            await maintenanceCostService.AddRange(allCost);
            var statusInDb = db.MaintenanceStatus;
            if (db.CollectionRate != mainInvoice.CollectionRate)
            {
                throw new InvalidInputException("لا يمكن لقيمة العربون ان تتغير عن القيمة القديمة.");
            }
            
            if (db.MaintenanceStatus is MaintenanceStatus.FullyPaid && mainInvoice.MaintenanceStatus is MaintenanceStatus.ReceivedFeeInAdvance)
            {
                throw new InvalidInputException("هذه الفاتورة قد تم دفعها بالكامل, لا يمكن أضافة عربون الأن.");
            }
            
            if (db.MaintenanceItemsStatus is MaintenanceItemsStatus.Finished && mainInvoice.MaintenanceItemsStatus is not MaintenanceItemsStatus.Finished)
            {
                throw new InvalidInputException("الأصناف في هذه الفاتورة تم تصنيعها ولا يمكن تغيير حالة التصنيع الأن.");
            }
            
            payInAdvanceThisTime = mainInvoice.MaintenanceStatus is MaintenanceStatus.ReceivedFeeInAdvance &&
                                   !mainInvoice.ReceivedFeeInAdvance;

            if (payInAdvanceThisTime)
            {
                mainInvoice.ReceivedFeeInAdvance = true;
            }
            
            await _service.Update(mainInvoice);
            
            await storageModuleService.LossAllEver(q.Id,
                [RelatedInventory.Maintenance]
            );

            await entryService.RemoveAllRelated(mainInvoice.Id, OprationTypeEnum.Maintenance, PurchaseAndSalesConstants.TransactionOprationType__MaintenanceRelated);

            await InventoryProcess(mainInvoice.Details, true);
            await FinancialProcess(finalTotal, actualTotal, payInAdvance: payInAdvanceThisTime);
        }

        _service.CommitTransaction();

        return mainInvoice;

        async Task InventoryProcess(IEnumerable<DtoMaintenanceInvoiceDetail> invoiceDetails, bool isLose = false, bool softDelete = true)
        {
            if (mainInvoice.MaintenanceItemsStatus is not MaintenanceItemsStatus.Finished)
                return;
            
            //****************************(0)****************************
            Logger.LogTrace("items updating : {0}, IsLose: {1}", invoiceDetails.Count(), isLose);
            //****************************(0)****************************
            foreach (var item in invoiceDetails)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = mainInvoice.StoreId;
                var qun = item.Quantity;

                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                if (selectedOne is null)
                {
                    Logger.LogTrace("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }

                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);

                {
                    var minusSellConfig = _configuration.GetSection("MinusSellCheck");
                    if (minusSellConfig is null || minusSellConfig.Get<bool>())
                    {
                        var quntityInfo = await storageModuleService.CanLoseItemQuntityInfo(itmId, untId, strId.Value, qun, new());
                        var errQaun = (quntityInfo.Info?.Inventory?.Quantity is not null) ? "أقل من " +
                            quntityInfo.Info?.Inventory?.Quantity.ToString() : " غير متوفرة";
                        if (!quntityInfo.CanLoost)
                            throw new InvalidInputException($"لا يمكن عمل الصيانة بالصنف " +
                                                            $" {itemInfo?.Name ?? "بدون اسم"}. " +
                                                            $"لان الكمية المطلوبة غير كافية في المخزن:" +
                                                            $"{qun} " +
                                                            $" {errQaun}");
                    }
                }

                var allOnes = selectedOne.TransferRate * qun;

                await storageModuleService.RegainOrLoseItem(itmId, untId, strId.Value, qun, allOnes,
                    cost: itemInfo.Cost,
                    avragePiceCost: item.Price / selectedOne.TransferRate,
                    price: item.Price,
                    relatedId: mainInvoice.Id,
                    relatedInventory: RelatedInventory.Maintenance,
                    isLose: isLose,
                    softDelete: softDelete,
                    storageItemProperties: new()
                );
            }
        }
        
        async Task<bool> FinancialProcess(decimal _discountedTtoal, decimal _total, bool IsRoleback = false, bool softDelete = false, bool payInAdvance = false)
        {
            
            if (mainInvoice.MaintenanceStatus != MaintenanceStatus.FullyPaid
                && !payInAdvance)
            {
                return false;
            }
            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    OprationTypeEnum.Maintenance,
                    PurchaseAndSalesConstants.TransactionOprationType__MaintenanceRelated,
                    mainInvoice.Id,
                    invoiceNumber,
                    "");
            
            var theClient = clientInfo?.AccountId ?? generalClientAccount.Id;

            if (mainInvoice.MaintenanceStatus is MaintenanceStatus.FullyPaid)
            {
                var final = _total - _discountedTtoal;

                if (mainInvoice.ReceivedFeeInAdvance)
                {
                    final -= mainInvoice.CollectionRate;
                }
                
                finacialTransaction.AppendEntry(manuInfo.AccountId, theClient, final);

                if (_discountedTtoal > 0)
                {
                    finacialTransaction.AppendEntry(discountAccountAccount.Id, manuInfo.AccountId, final);
                }
                
                switch (mainInvoice.PaymentType)
                {
                    case PeymentType.Cash:
                        // NOTE: if the safe has `loss percent` ?
                        finacialTransaction.AppendEntry(safeInfo.AccountId, theClient, final);
                        break;
                    case PeymentType.Deferred:
                        break;
                    case PeymentType.BankServices:
                        finacialTransaction.AppendEntry(bankInfo.AccountId, theClient, final);
                        break;
                }
            }
            else if(payInAdvance)
            {
                finacialTransaction.AppendEntry(manuInfo.AccountId, theClient, q.CollectionRate);
                
                switch (mainInvoice.PaymentType)
                {
                    case PeymentType.Cash:
                        // NOTE: if the safe has `loss percent` ?
                        finacialTransaction.AppendEntry(safeInfo.AccountId, theClient, q.CollectionRate);
                        break;
                    case PeymentType.Deferred:
                        break;
                    case PeymentType.BankServices:
                        finacialTransaction.AppendEntry(bankInfo.AccountId, theClient, q.CollectionRate);
                        break;
                }
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
    }

    public virtual void Dispose()
    {
        OnCreateUpdate -= MaintenanceInvoiceControllerOnCreateUpdate;
    }

    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(
        [FromQuery] StoreQueryRequest request)
        => GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients(
        [FromQuery] ClientQueryRequest request)
        => GetSelectableList<DtoClient, TClient>(x => x.Name);

    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes([FromQuery] SafeQueryRequest request)
        => GetSelectableList<DtoSafe, TSafe>(x => x.Name);

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(
        [FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name} ({x.Barcode}) • {x.Price}",
            request.ItemId != null
                ? x => x.CategoryId == request.ItemId.Value && x.IsActive
                : y => y.IsActive);
    
    [HttpGet("Banks")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupBanks([FromQuery] BankQueryRequest request)
        => GetSelectableList<DtoBank, TBank>(x => x.Name);
    
    [HttpGet("ItemVariances")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItemVariances(
        [FromQuery] ItemVarianceQueryRequest request)
        => GetSelectableList<DtoItemVariance, TItemVariance>(x => x.Name);
    
    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(
        [FromQuery] UnitQueryRequest request)
        => GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));
    
    [HttpGet("MaintenanceSectors")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupMaintenanceSectors(
        [FromQuery] MaintenanceSectorQueryRequest request)
        => GetSelectableList<DtoMaintenanceSector, TMaintenanceSector>(x => x.Name);
}