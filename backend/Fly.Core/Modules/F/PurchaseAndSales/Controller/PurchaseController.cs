using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Core.Modules.E.Store.Services.ModuleService;
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
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using Fly.Framework.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;


public class PurchaseController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoPurchaseInvoice, TPurchaseInvoice, PurchaseInvoiceQueryRequest, DeleteRequestBase, PurchaseInvoiceCreateRequest>,
        IPurchaseFeatures
{

    RelatedInventory relatedInventory = RelatedInventory.Purchase;


    public PurchaseController(ILocalizationService localizationService, ILogger<CustomControllerFeatrueBased<DtoPurchaseInvoice, TPurchaseInvoice, PurchaseInvoiceQueryRequest, DeleteRequestBase, PurchaseInvoiceCreateRequest>> logger, IMediator mediator, IServiceBase<DtoPurchaseInvoice, TPurchaseInvoice> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        /*
         * delete the invoice
         * delete invoice-details
         * delete qauntities
         * delete entries
         */
        OnDeleteEx += PurchaseController_OnDeleteEx;
        OnCreateUpdate += PurchaseInvoiceController_OnCreateUpdate;
    }

    [HttpGet("ItemInfo")]
    public ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> PostItemInfoQuery([FromQuery] GetItemInfoQueryRequest queryRequest)
    =>
        TryOne<ItemInfoQueryResponse>(async opt =>
        {
            var itemService = _service.MakeChildService<DtoItem, TItem>();

            var itemInfo = await itemService
                .GetOneAsAsync(s => s, w => queryRequest.ItemId == w.Id);

            if (itemInfo is null)
                return opt.Problem("item-not-found");
            
            return opt.Success(new ItemInfoQueryResponse
            {
                Price = itemInfo!.Price,
                Quntity = 0, // inventoryInfo.Quantity,
                AdditionalQuntity = 0, // inventoryInfo.AdditionalQuantity,#
                MinPrice = itemInfo.MinPrice,
                IsExpirable = itemInfo.Expirable
            });
        });
    
    private async ValueTask<HttpResponseWrapper<DtoPurchaseInvoice?>> PurchaseController_OnDeleteEx(DeleteRequestBase q, ResultOrFailer<DtoPurchaseInvoice?> _)
    {
        //
        var entryService = _service.MakeChildService<EntryService>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var sellreturnDetailsService = _service.MakeChildService<DtoPurchaseInvoiceDetail, TPurchaseInvoiceDetail>();
        //
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        var returnInvoice = await _service.GetById(q.Id);
        await sellreturnDetailsService.Remove(w => w.PurchaseInvoiceId == returnInvoice.Id);
        await storageModuleService.LossAllEver(returnInvoice.Id, [relatedInventory]);
        await entryService.RemoveAllRelated(returnInvoice.Id, OprationTypeEnum.Purchases, PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
        await entryService.RemoveAllRelated(returnInvoice.Id, OprationTypeEnum.PurchasesCost, PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
        //
        await _service.Remove(returnInvoice.Id);
        var status = _service.CommitTransaction();
        if (!status)
            return _.Problem("cannot proccesed the sale opration");
        return _.Success(returnInvoice)!;
    }

    public override async Task BeforReturn(PageList<DtoPurchaseInvoice> list)
    {
        var entryService = _service.MakeChildService<IEntryService>();
        var clientService = _service.MakeChildService<DtoSupplier, TSupplier>();
        var arr = list.Items.Where(w => w.SupplierId != null).Select(w => w.SupplierId.Value).Distinct().ToArray();
        var accArr = await clientService.GetAllAsAsync(s => new { s.AccountId, s.Id }, w => ((IEnumerable<Identifier>)arr).Contains(w.Id));
        var accArrIds = accArr.Items.Select(s => s.AccountId).ToArray();
        var multiQuery = await entryService.GetBalance(accArrIds, null, null);

        var nonSupplier = list.Items.Where(w => w.SupplierId == null).ToList();
        var list2 = multiQuery
            .Join(accArr.Items, _ => _.AccountId, _ => _.AccountId, (x, y) => new
            {
                Id = y.Id,
                Balance = x.Balance,
                x.AccountId,
            })
            .Join(
            list.Items, _ => _.Id, _ => _.SupplierId,
            (x, y) =>
            {
                y.Balance = x.Balance;
                return y;
            }).ToList();
        List<DtoPurchaseInvoice> all = [.. nonSupplier, .. list2];
        list.Items = [.. all.OrderByDescending(s => s.Number)];
    }
    
    protected override async ValueTask AfterDelete(DtoPurchaseInvoice dto, DeleteRequestBase command, ResultOrFailer<DtoPurchaseInvoice?> opt)
    {

        var entryService = _service.MakeChildService<DtoEntry, TEntry>();
        var entryDetailservice = _service.MakeChildService<DtoEntryDetail, TEntryDetail>();
        var inventoryservuce = _service.MakeChildService<DtoInventory, TInventory>();
        var inventoryCostService = _service.MakeChildService<DtoInventoryCost, TInventoryCost>();

        var toBeDeleted = await entryService.GetAllAsync(w => w.RelatedId == dto.Id && w.RelatedIndex == PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
        var toBeDeletedEntriesIds = toBeDeleted.Items.Select(x => x.Id).ToList();
        await entryDetailservice.Remove(w => toBeDeletedEntriesIds.Contains(w.EntryId));
        await entryService.Remove(w => toBeDeletedEntriesIds.Contains(w.Id));

        var toBedeletaedINventory = await inventoryCostService
            .GetAllAsAsync(s => s.Id, w => w.Inventory.RelatedId == dto.Id && w.Inventory.RelatedIndex == relatedInventory);
        await inventoryCostService.Remove(w => toBedeletaedINventory.Items.Contains(w.TInventoryId));
        await inventoryservuce.Remove(w => toBedeletaedINventory.Items.Contains(w.Id));

    }

    [NonAction]
    public override Expression<Func<DtoPurchaseInvoice, bool>> GetWhere(Expression<Func<DtoPurchaseInvoice, bool>> expression, PurchaseInvoiceQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Details.Any(d => d.UnitName.Contains(query.SearchText)) |
                x.Details.Any(d => d.ItemName.Contains(query.SearchText)) |
                x.Number.ToString().Contains(query.SearchText) |
                x.TotalCost.ToString() == query.SearchText |
                x.TotalAmount.ToString() == query.SearchText |
                x.SafeName.Contains(query.SearchText) |
                x.SupplierName.Contains(query.SearchText)
            );
        return base.GetWhere(expression, query);
    }

    [HttpGet("ByNumber")]
    public ValueTask<HttpResponseWrapper<DtoPurchaseInvoice?>> GetInvoiceByNumber([FromQuery] string number)
        =>
            TryOne<DtoPurchaseInvoice?>(async (opt) =>
            {
                if (!long.TryParse(number, out var num))
                    return opt.Problem("Wrong number"); // locale
                
                var next = await _service.GetOneAsAsync(x => x, y => y.Number == num);
                return opt.Success(next);
            });
    
    [NonAction]
    private async ValueTask ProcessItems(DtoPurchaseInvoice invoice)
    {
        // Process items
        // TODO: Implement the code for processing the items
    }
    
    [NonAction]
    private async ValueTask<DtoPurchaseInvoice> PurchaseInvoiceController_OnCreateUpdate(PurchaseInvoiceCreateRequest q)
    {
        //
        //
        var status = false;
        var unitService = _service.MakeChildService<DtoUnit, TUnit>();
        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var supplierService = _service.MakeChildService<DtoSupplier, TSupplier>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var invoiceSrvicecost = _service.MakeChildService<DtoPurchaseInvoiceCost, TPurchaseInvoiceCost>();
        var invoiceSrviceDetails = _service.MakeChildService<DtoPurchaseInvoiceDetail, TPurchaseInvoiceDetail>();
        var entryService = _service.MakeChildService<IEntryService>();
        //
        q.Details = q.Details
            .Reverse<PurchaseInvoiceDetail>()
            .GroupBy(w => new { w.UnitId, w.ItemId, w.ExpireDate })
            .Select(g => new PurchaseInvoiceDetail(g.Key.UnitId, g.Key.ItemId, g.Sum(s => s.Quantity), g.Max(g => g.Price), g.Max(g => g.Discount), g.Key.ExpireDate))
            .ToList();
        var invoice = q.Adapt<DtoPurchaseInvoice>();
        var invoiceDetails = invoice.Details.ToList();
        //
        DtoAccount? safeInfoAccount = null;
        DtoAccount? expenseInfoAccount = null;
        DtoAccount? purchaseBaseAccount = null;
        var selectedItems = new List<Identifier>();
        var selectedUnits = new List<Identifier>();
        //
        var pricingInfo = invoice.Count();
        var actualTotal = pricingInfo.total;
        var finalTotal = pricingInfo.discountedTotal;
        if (finalTotal < 0)
        {
            throw new InvalidInputException("الاجمالي يجب أن يكون أكبر من صفر");
        }
        //
        decimal totalCosts = invoice.Costs.Sum(cost => cost.Ammount);
        decimal totalPurchasers = actualTotal;
        invoiceDetails.Distinct().ForEach(f =>
        {
            selectedItems.Add(f.ItemId);
            selectedUnits.Add(f.UnitId);
        });
        //
        _service.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        //
        var selectedUnitInfo = (await itemUnitsService.GetAllAsAsync(
            unit => new { unit.UnitId, unit.ItemId, unit.TransferRate, unit.IsPrimary },
            unit => selectedItems.Contains(unit.ItemId) && selectedUnits.Contains(unit.UnitId)
        )).Items;
        var totalItemCounts = invoiceDetails
            .Join(selectedUnitInfo,
                  detail => new { detail.UnitId, detail.ItemId },
                  unit => new { unit.UnitId, unit.ItemId },
                  (detail, unit) => detail.Quantity * unit.TransferRate)
            .Sum();
        //
        //  SAFE
        var safeInfo = await safeService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == invoice.SafeId);
        if (safeInfo is null) invoice.SafeId = null;
        else safeInfoAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(safeInfo.AccountId));
        if (safeInfo is null && PeymentType.Cash == invoice.PeymentType)
            throw new InvalidInputException("Please Select Safe"); // TODO: Localization
        //
        //  PURCHES
        var purchesAccountId = await configService.GetOneAsAsync(x => x.PurchaseInvoicesAccount);
        if (purchesAccountId is null)
            throw new InvalidInputException("PurchaseInvoicer account not ready"); // TODO: Localization
        purchaseBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(purchesAccountId));
        if (purchaseBaseAccount is null)
            throw new InvalidInputException("PurchaseInvoicer account not ready"); // TODO: Localization
        //
        var discountAccountAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
        if (discountAccountAccountConfig is null) throw new Exception("DiscountAccount not found");
        var discountAccountAccount = await accountService
           .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountAccountConfig));
        if (discountAccountAccount is null) throw new Exception("DiscountAccount not found.");
        //
        //  
        //  
        var bankService = _service.MakeChildService<DtoBank, TBank>();
        var bankInfo = await bankService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == invoice.BankId);
        if (bankInfo is null) invoice.BankId = null;
        if (bankInfo is null && PeymentType.BankServices == invoice.PeymentType)
            throw new InvalidInputException("يرجى أختيار بنك."); // TODO: Localization
        
        if (invoice.SupplierId.IsNullOrEmpty()) invoice.SupplierId = null;
        var generalAccountId = await configService.GetOneAsAsync(x => x.GeneralSupplierAccountAccount);
        if (generalAccountId is null && invoice.SupplierId.IsNullOrEmpty())
            throw new InvalidInputException("General supplier account not ready"); // TODO: Localization
        //
        var supplierInfo = await supplierService.GetOneAsAsync(s => new { s.AccountId }, w => w.Id == invoice.SupplierId);
        if (supplierInfo is null && !invoice.SupplierId.IsNullOrEmpty())
            throw new InvalidInputException("supplier account not exists"); // TODO: Localization
        Identifier supplierAccount = invoice.SupplierId.IsNullOrEmpty() ? Identifier.Parse(generalAccountId!) : supplierInfo!.AccountId;
        //
        //  expenseInfoAccount
        if (PeymentType.Cash == invoice.PeymentType || totalCosts > 0)
        {
            if (totalCosts > 0)
            {
                var expenseInfoAccountConfig = await configService.GetOneAsAsync(x => x.ExpenseAccount);
                if (expenseInfoAccountConfig is null)
                    throw new InvalidInputException("Expense Account not ready"); // TODO: Localization
                expenseInfoAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(expenseInfoAccountConfig));
                if (expenseInfoAccount is null)
                    throw new InvalidInputException("Expense Account not exists"); // TODO: Localization
            }
        }
        //
        //
        invoice.TotalCost = totalCosts;
        invoice.TotalAmount = totalPurchasers;
        invoice.SafeId = invoice.SafeId?.Value == Guid.Empty ? null : invoice.SafeId;
        var costPerItem = totalCosts / totalItemCounts;
        long invoiceNumber = 0;
        //
        //
        async Task<bool> FinacialProcess(decimal finalTotalArgmnt, decimal actualTotalArgmnt, bool softDelete = false)
        {
            var countedTotalCosts = totalCosts;
            //var countedTotalPurchasers = totalPurchasers;
            var discount = actualTotalArgmnt - finalTotalArgmnt;

            if (countedTotalCosts > 0)
            {
                var finacialCostTransaction = accountingFinacialService
                    .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                        OprationTypeEnum.PurchasesCost,
                        PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated,
                        invoice.Id,
                        invoiceNumber,
                       "");
                finacialCostTransaction.AppendEntry(expenseInfoAccount!.Id, safeInfoAccount!.Id, countedTotalCosts);
                await finacialCostTransaction.ComplateTransaction();
            }
            //  1
            var finacialPurchersTransaction = accountingFinacialService
              .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                  OprationTypeEnum.Purchases,
                  PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated,
                  invoice.Id,
                  invoiceNumber,
                 "");
            // Set the invoice deption for the supplier
            var reelTotal = invoice.IsCostOnSupplier ? finalTotalArgmnt + countedTotalCosts : finalTotalArgmnt;
            finacialPurchersTransaction.AppendEntry(purchaseBaseAccount.Id, supplierAccount, reelTotal);
            if (discount > 0)
            {
                finacialPurchersTransaction.AppendEntry(purchaseBaseAccount.Id, discountAccountAccount.Id, discount);
            }

            switch (invoice.PeymentType)
            {
                case PeymentType.Cash:
                    // NOTE: if the safe has `loss percent` ?
                    finacialPurchersTransaction.AppendEntry(supplierAccount, safeInfoAccount.Id, reelTotal);
                    break;
                case PeymentType.Deferred:
                    break;
                case PeymentType.BankServices:
                    finacialPurchersTransaction.AppendEntry(supplierAccount, bankInfo.AccountId, reelTotal);
                    break;
            }

            await finacialPurchersTransaction.ComplateTransaction();
            
            return true;
        }
        //
        async Task InventiryProcess(IEnumerable<DtoPurchaseInvoiceDetail> invoiceDetails, bool IsRoleback = false, bool softDelete = true)
        {
            foreach (var item in invoiceDetails)
            {
                //
                var prp = new Shared.Modules.Store.Dtos.Extra.StorageItemProperties();
                if (item.ExpireDate != null) prp.ExpireDate = item.ExpireDate;
                //
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var qun = item.Quantity;
                var strId = invoice.StoreId;
                //
                var itemInfo = await itemService
                    .GetOneAsAsync(s => new { s.Name }, w => w.Id == itmId);
                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                var selectedOnePrimary = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId && w.IsPrimary);
                if (selectedOne is null || selectedOnePrimary is null)
                {
                    if (selectedOne is null)
                    {
                        Logger.LogWarning("selectedOne check failed");
                    }
                    else if (selectedOnePrimary is null)
                    {
                        Logger.LogWarning("selectedOnePrimary check failed");
                    }
                    
                    Logger.LogWarning("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }
                //var selectedOne = selectedUnitInfo
                //    .First(w => w.UnitId == item.UnitId & w.ItemId == item.ItemId);
                //var quantityInfo = await storageModuleService
                //    .GetItemQuntity(item.ItemId, item.UnitId, invoice.StoreId);
                //
                var cost = costPerItem + item.Price;
                var totalQuntity = selectedOne.TransferRate * qun;

                var dtoItem = await itemService.GetAsync(w => w.Id == itmId);
                var oldQuntityInfo = await storageModuleService.GetItemQuntityOrginalItemQuntity(dtoItem.Id, selectedOnePrimary.UnitId, invoice.StoreId);
                var costPerUnit = cost / selectedOne.TransferRate;
                ProductCostAcounter productCostAcounter = new(oldQuntityInfo.Quantity, dtoItem.Cost);
                productCostAcounter.UpdateCost(totalQuntity, costPerUnit);
                dtoItem.Cost = productCostAcounter.CostPerUnit;
                await itemService.Update(dtoItem);

                await storageModuleService.RegainOrLoseItem(
                    itmId, untId, strId, qun, totalQuntity,

                    cost: costPerItem,
                    avrageCost: cost, // TODO: change 
                    avragePiceCost: costPerUnit,
                    price: item.Price,
                    sellPrice: 0m,

                    relatedId: invoice.Id,
                    relatedInventory: relatedInventory,
                    IsRoleback,
                    softDelete, prp);
            }
        }
        //
        //
        if (q.IsNew())
        {
            invoice.Date = DateTime.Now;
            //
            var lastInvoiceNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastInvoiceNumber + 1;
            invoice.Number = invoiceNumber;
            await _service.Add(invoice);

            if (invoiceDetails is not null && invoiceDetails.Count != 0)
            {
                await InventiryProcess(invoiceDetails);
            }

            //  Finacial
            status = await FinacialProcess(finalTotal, actualTotal);
        }
        else
        {
            invoiceNumber = invoice.Number;


            //NEW 
            Logger.LogTrace("removing details {0} for id {1}", nameof(DtoPurchaseInvoice), invoice.Id);
            await invoiceSrviceDetails.Remove(w => w.PurchaseInvoiceId == invoice.Id);
            await storageModuleService.LossAllEver(invoice.Id, relatedInventory);
            await entryService.RemoveAllRelated(invoice.Id, OprationTypeEnum.Purchases, PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
            await entryService.RemoveAllRelated(invoice.Id, OprationTypeEnum.PurchasesCost, PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated);
            //
            await _service.Update(invoice);
            Logger.LogTrace("updated {0} with id {1}", nameof(DtoPurchaseInvoice), invoice.Id);
            invoice.Details.ForEach(f => f.PurchaseInvoiceId = invoice.Id);
            await invoiceSrviceDetails.AddRange(invoice.Details);
            var idsArray = invoice.Details.Select(s => s.Id).ToArray();
            var ids = string.Join(", ", idsArray);
            Logger.LogTrace("saved details {0} with for id {1} with ids: [{2}]", nameof(DtoPurchaseInvoiceDetail), invoice.Id, ids);
            //
            //  Items
            await InventiryProcess(invoice.Details);
            //  Financial
            await FinacialProcess(finalTotal, actualTotal);
            //
        }

        status = _service.CommitTransaction();
        if (!status)
            throw new Exception("cannot proccesed the sale opration");

        //var entryService = _service.MakeChildService<IEntryService>();
        var invoiceDb = await _service.GetById(invoice.Id);
        if (supplierInfo?.AccountId is not null)
        {
            var multiQuery = await entryService.GetBalance(supplierInfo.AccountId.Value, null, null);
            invoiceDb.Balance = multiQuery;
        }
        return invoiceDb;
    }

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        =>
        GetSelectableList<DtoItem, TItem>(x => $"{x.Name} - {x.Barcode}");

    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));

    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores([FromQuery] StoreQueryRequest request)
        =>
        GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("Supplier")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSuppliers([FromQuery] SupplierQueryRequest request)
        =>
        GetSelectableList<DtoSupplier, TSupplier>(x => x.Name);

    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes([FromQuery] SafeQueryRequest request)
        =>
        GetSelectableList<DtoSafe, TSafe>(x => x.Name);

    [HttpPost("ItemInfo")]
    public ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> GetItemInfoQuery([FromBody] GetItemInfoQueryRequest queryRequest)
    =>
        TryOne<ItemInfoQueryResponse>(async opt =>
        {
            var request = this.Request;
            var storageService = _service.MakeChildService<IStorageModuleService>();

            var inventoryInfo = await storageService.GetItemQuntity(queryRequest.ItemId, queryRequest.UnitId, queryRequest.StoreId);

            return opt.Success(new ItemInfoQueryResponse()
            {
                Price = 0,
                Quntity = inventoryInfo.Quantity,
                AdditionalQuntity = inventoryInfo.AdditionalQuantity,
            });

        });

    [HttpGet("Banks")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupBanks([FromQuery] BankQueryRequest request)
        => GetSelectableList<DtoBank, TBank>(x => x.Name);
    
    [HttpGet("Number")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber()
   =>
       TryOne<long>(async (opt) =>
       {
           var next = await _service.NextNumber(x => x.Number);
           return opt.Success(next);
       });

    [NonAction]
    public virtual void Dispose()
    {
        OnCreateUpdate -= PurchaseInvoiceController_OnCreateUpdate; ;
    }

}