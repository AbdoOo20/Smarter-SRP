using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.PurchaseAndSales;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Fly.Core.Modules.Factoring.FactoringInvoices.Services.InternalServices;
using Fly.Core.Modules.Factoring.ManufacturingFacilities.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Enums;
using Fly.Shared.Modules.Factoring.Features;
using Fly.Shared.Modules.Factoring.Requests;
using Fly.Shared.Modules.Global.Dtos;
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

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Controller;

public class FactoringInvoiceController
    : CustomControllerFeatrueBased<DtoFactoringInvoice,
            TFactoringInvoice, FactoringInvoiceQueryRequest,
            FactoringInvoiceDeleteRequest,
            FactoringInvoiceCreateUpdateRequest>,
        IFactoringInvoiceFeatures, IDisposable
{
    private readonly IConfiguration _configuration;

    public FactoringInvoiceController(ILocalizationService localizationService,
        ILogger<FactoringInvoiceController> logger,
        IMediator mediator, IFactoringInvoiceService service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster,
        IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _configuration = configuration;
        OnCreateUpdate += FactoringInvoiceControllerOnCreateUpdate;
    }

    public override async Task BeforReturn(PageList<DtoFactoringInvoice> list)
    {
        #region Counting Balances

        var entryService = _service.MakeChildService<IEntryService>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var arr = list.Items.Where(w => w.ClientId != null).Select(w => w.ClientId.Value).Distinct().ToArray();
        var accArr = await clientService.GetAllAsAsync(s => new { s.AccountId, s.Id }, w => ((IEnumerable<Identifier>)arr).Contains(w.Id));
        var accArrIds = accArr.Items.Select(s => s.AccountId).ToArray();
        var multiQuery = await entryService.GetBalance(accArrIds, null, null);

        var nonClientInvoice = list.Items.Where(w => w.ClientId == null).ToList();
        /*var list2 = multiQuery
            .Join(accArr.Items, _ => _.AccountId, _ => _.AccountId, (x, y) => new
            {
                y.Id,
                x.Balance,
                x.AccountId,
            });
        List<DtoFactoringInvoice> all = [.. nonClientInvoice];*/
        list.Items = list.Items.OrderByDescending(s => s.Number);

        #endregion

        #region Counting Qauntities

        /*var sellReturnInvoiceService = _service.MakeChildService<DtoSellReturnInvoice, TSellReturnInvoice>();
        var sellInvoices = list.Items.Select(s => s.Id).Distinct().ToArray();
        var aa = await sellReturnInvoiceService.GetAllAsync(w => sellInvoices.Contains(w.InvoiceId!.Value));
        foreach (var invoice in list.Items)
        {

            //var returns = aa.Items.Where(w => w.InvoiceId == invoice.Id).ToArray();
            //if (returns.Length != 0)
            //{
            //    invoice.ReturnInvoices = returns.SelectMany(s => s.Details).ToArray();
            //}
            AdjustQuantity(invoice, aa.Items.Where(w => w.InvoiceId == invoice.Id).ToArray(), true);
            if (invoice.Details.All(w => w.Quantity == 0))
            {
                list.Items = list.Items.Where(w => w.Id != invoice.Id);
            }

        }*/
        #endregion

    }
        
    public override Expression<Func<DtoFactoringInvoice, bool>> GetWhere(
        Expression<Func<DtoFactoringInvoice, bool>> expression,
        FactoringInvoiceQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Number.ToString().Contains(query.SearchText) |
                x.ManufacturingFacilityName.Contains(query.SearchText) |
                x.FactoringStatus.ToString().Contains(query.SearchText) |
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
    public ValueTask<HttpResponseWrapper<DtoFactoringInvoice?>> GetInvoiceByNumber([FromQuery] string number)
        =>
            TryOne<DtoFactoringInvoice?>(async (opt) =>
            {
                if (!long.TryParse(number, out var num))
                    return opt.Problem("Wrong number"); // locale
                
                var next = await _service.GetOneAsAsync(x => x, y => y.Number == num);
                return opt.Success(next);
            });
    
    private async ValueTask<DtoFactoringInvoice> FactoringInvoiceControllerOnCreateUpdate(FactoringInvoiceCreateUpdateRequest q)
    {
        var factoringDetailService = _service.MakeChildService<DtoFactoringInvoiceDetail, TFactoringInvoiceDetail>();
        var factoringCostService = _service.MakeChildService<DtoFactoringInvoiceCost, TFactoringInvoiceCost>();
        var factoringCompositionService = _service.MakeChildService<DtoFactoringInvoiceComposition, TFactoringInvoiceComposition>();
        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var manuService = _service.MakeChildService<DtoManufacturingFacility, TManufacturingFacility>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var entryService = _service.MakeChildService<IEntryService>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var itemComp = _service.MakeChildService<DtoItemComposition, TItemComposition>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        //

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        
        var mainInvoice = q.Adapt<DtoFactoringInvoice>();
        var safeInfo = await safeService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == mainInvoice.SafeId);
        if (safeInfo is null) mainInvoice.SafeId = null;
        if (safeInfo is null && PeymentType.Cash == mainInvoice.PaymentType)
            throw new InvalidInputException("Please Select Safe"); // TODO: Localization
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
        var (actualTotal, netTotal, discountTotal) = mainInvoice.Count();
        var finalTotal = netTotal;
        if (finalTotal < 0)
        {
            throw new Exception("الاجمالي يجب أن يكون أكبر من صفر");
        }
        var bankService = _service.MakeChildService<DtoBank, TBank>();

        var bankInfo = await bankService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == mainInvoice.BankId);
        if (bankInfo is null) mainInvoice.BankId = null;
        if (bankInfo is null && PeymentType.BankServices == mainInvoice.PaymentType)
            throw new InvalidInputException("يرجى أختيار بنك."); // TODO: Localization
        
        var manuInfo = await manuService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == mainInvoice.ManufacturingFacilityId);
        if (manuInfo is null) mainInvoice.ManufacturingFacilityId = null;
        if (manuInfo is null)
        {
            mainInvoice.IsInternal = true;
            var internalMain = await configService.GetOneAsAsync(s => s.InternalManufacturingFacilitiesAccount);
            manuInfo = new { AccountId =  Identifier.Parse(internalMain) };
        }

        long invoiceNumber;
        
        if (mainInvoice.CollectionRate < 0)
        {
            throw new InvalidInputException("لا يمكن لقيمة العربون ان تكون سالبة.");
        }
        
        if (mainInvoice is { CollectionRate: <= 0, FactoringStatus: FactoringStatus.ReceivedFeeInAdvance })
        {
            throw new InvalidInputException("لا يمكن لقيمة العربون ان تكون صفر أذا كانت حالة الدفع أن العربون تم أستلامه.");
        }

        bool payInAdvanceThisTime = false;
        
        if (mainInvoice.IsNew())
        {
            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            mainInvoice.Number = invoiceNumber;
            var receivedInAdvance = mainInvoice.FactoringStatus
                is FactoringStatus.ReceivedFeeInAdvance && mainInvoice.CollectionRate > 0;

            if (receivedInAdvance)
            {
                mainInvoice.ReceivedFeeInAdvance = true;
                payInAdvanceThisTime = true;
            }
            
            await _service.Add(mainInvoice);
            await InventoryProcess(mainInvoice.Details, mainInvoice.Compositions, true);
            
            await FinancialProcess(discountTotal, actualTotal, payInAdvance: payInAdvanceThisTime);
        }
        else
        {
            var db = await _service.GetById(mainInvoice.Id);
            await factoringDetailService.Remove(w => w.FactoringInvoiceId == db.Id, false);
            var all = mainInvoice.Details.ToList();
            mainInvoice.Adapt(db);
            invoiceNumber = mainInvoice.Number;
            all.ForEach(f => f.FactoringInvoiceId = db.Id);
            await factoringDetailService.AddRange(all);
            
            await factoringCostService.Remove(w => w.FactoringInvoiceId == db.Id, false);
            var allCost = mainInvoice.Costs.ToList();
            allCost.ForEach(f => f.FactoringInvoiceId = db.Id);
            await factoringCostService.AddRange(allCost);
            
            await factoringCompositionService.Remove(w => w.FactoringInvoiceId == db.Id, false);
            var allComp = mainInvoice.Compositions.ToList();
            allComp.ForEach(f => f.FactoringInvoiceId = db.Id);
            await factoringCompositionService.AddRange(allComp);
            var statusInDb = db.FactoringStatus;
            if (db.CollectionRate != mainInvoice.CollectionRate)
            {
                throw new InvalidInputException("لا يمكن لقيمة العربون ان تتغير عن القيمة القديمة.");
            }
            
            if (db.FactoringStatus is FactoringStatus.FullyPaid && mainInvoice.FactoringStatus is FactoringStatus.ReceivedFeeInAdvance)
            {
                throw new InvalidInputException("هذه الفاتورة قد تم دفعها بالكامل, لا يمكن أضافة عربون الأن.");
            }
            
            if (db.FactoringItemsStatus is FactoringItemsStatus.Finished && mainInvoice.FactoringItemsStatus is not FactoringItemsStatus.Finished)
            {
                throw new InvalidInputException("الأصناف في هذه الفاتورة تم أستعمالها في الصيانة ولا يمكن تغيير حالة الصيانة الأن.");
            }
            
            payInAdvanceThisTime = mainInvoice.FactoringStatus is FactoringStatus.ReceivedFeeInAdvance &&
                                    !mainInvoice.ReceivedFeeInAdvance;

            if (payInAdvanceThisTime)
            {
                mainInvoice.ReceivedFeeInAdvance = true;
            }
            
            await storageModuleService.LossAllEver(q.Id,
                [RelatedInventory.Factoring]
            );
            
            await _service.Update(mainInvoice);
            
            await entryService.RemoveAllRelated(mainInvoice.Id, OprationTypeEnum.Maintenance,
                PurchaseAndSalesConstants.TransactionOprationType__FactoringRelated);

            await InventoryProcess(mainInvoice.Details, mainInvoice.Compositions, true);
            await FinancialProcess(finalTotal, actualTotal, payInAdvance: payInAdvanceThisTime);
        }

        _service.CommitTransaction();
        return mainInvoice;
        
        async Task InventoryProcess(IEnumerable<DtoFactoringInvoiceDetail> invoiceDetails,
            IEnumerable<DtoFactoringInvoiceComposition> compositions,
            bool isLose = false, bool softDelete = true)
        {
            if (mainInvoice.FactoringItemsStatus is not FactoringItemsStatus.Finished)
                return;
            
            Logger.LogTrace("items updating : {0}, IsLose: {1}", invoiceDetails.Count(), isLose);
            var minusSellConfig = _configuration.GetSection("MinusSellCheck");

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
                    throw new InvalidInputException($"Cannot get item unit info: {item.ItemName}, {item.UnitName}");
                }

                var itemInfo = await itemService.GetOneAsAsync(s => new { s.Name, s.Cost }, w => w.Id == itmId);

                if (minusSellConfig is null || minusSellConfig.Get<bool>())
                {
                    var quntityInfo = await storageModuleService.CanLoseItemQuntityInfo(itmId, untId, strId.Value, qun, new());
                    var errQaun = (quntityInfo.Info?.Inventory?.Quantity is not null) ? "أقل من " +
                        quntityInfo.Info?.Inventory?.Quantity.ToString() : " غير متوفرة";
                    if (!quntityInfo.CanLoost)
                        throw new InvalidInputException($"لا يمكن أكمال التصنيع بالصنف " +
                                                        $" {itemInfo?.Name ?? "بدون اسم"}. " +
                                                        $"لان الكمية المطلوبة غير كافية في المخزن:" +
                                                        $"{qun} " +
                                                        $" {errQaun}");
                }

                var allOnes = selectedOne.TransferRate * qun;

                await storageModuleService.RegainOrLoseItem(itmId, untId, strId.Value, qun, allOnes,
                    cost: itemInfo.Cost,
                    avragePiceCost: item.Price / selectedOne.TransferRate,
                    price: item.Price,
                    relatedId: mainInvoice.Id,
                    relatedInventory: RelatedInventory.Factoring,
                    isLose: isLose,
                    softDelete: softDelete,
                    storageItemProperties: new()
                );
            }
        }
        
        async Task<bool> FinancialProcess(decimal _discountedTtoal, decimal _total, bool IsRoleback = false, bool softDelete = false, bool payInAdvance = false)
        {
            
            if (mainInvoice.FactoringStatus != FactoringStatus.FullyPaid
                && !payInAdvance)
            {
                return false;
            }
            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    OprationTypeEnum.Factoring,
                    PurchaseAndSalesConstants.TransactionOprationType__FactoringRelated,
                    mainInvoice.Id,
                    invoiceNumber,
                    "");
            
            var theClient = clientInfo?.AccountId ?? generalClientAccount.Id;

            if (mainInvoice.FactoringStatus is FactoringStatus.FullyPaid)
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
        OnCreateUpdate -= FactoringInvoiceControllerOnCreateUpdate;
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
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name} • {x.Barcode}",
            request.ItemId != null
                ? x => x.CategoryId == request.ItemId.Value && x.IsActive
                : y => y.IsActive);
    
    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(
        [FromQuery] UnitQueryRequest request)
        => GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));
    
    [HttpGet("Banks")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupBanks([FromQuery] BankQueryRequest request)
        => GetSelectableList<DtoBank, TBank>(x => x.Name);
    
    [HttpGet("ItemsVariances")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItemVariances(
        [FromQuery] ItemVarianceQueryRequest request)
        => GetSelectableList<DtoItemVariance, TItemVariance>(x => x.Name);
    
    [HttpGet("ItemCompositions")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCompositions([FromQuery] ItemCompositionQueryRequest request)
        => GetSelectableList<DtoItemComposition, TItemComposition>(x => $"{x.Name}");

    [HttpGet("ItemCompositionDetails")]
    public async ValueTask<HttpResponseWrapper<DtoItemComposition>> GetLookupCompositionsDetails(
        [FromQuery] ItemCompositionQueryRequest request)
    {
        var service = _service.MakeChildService<DtoItemComposition, TItemComposition>();

        var dto = await service.GetOneAsAsync(b => b, a => a.Id == request.ItemCompositionId);
        
        if(dto is null)
            return HttpResponseWrapper<DtoItemComposition>.Problem("No composition found");
        
        return HttpResponseWrapper<DtoItemComposition>.Success(dto);
    }

    [HttpGet("ManufacturingFacilities")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupManufacturingFacilities([FromQuery] ManufacturingFacilityQueryRequest request)
        => GetSelectableList<DtoManufacturingFacility, TManufacturingFacility>(x => x.Name);
}