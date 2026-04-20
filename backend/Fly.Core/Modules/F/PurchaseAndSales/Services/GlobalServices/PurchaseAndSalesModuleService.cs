using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;

public class PurchaseAndSalesModuleService : GlobalModuleService, IPurchaseAndSalesModuleService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountingFinacial _accountingFinacialService;
    private readonly IServiceBase<DtoInventoryCost, TInventoryCost> _inventoryCostService;

    public PurchaseAndSalesModuleService(ILogger<PurchaseAndSalesModuleService> logger, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapsterMapper, IServiceProvider serviceProvider,
        AccountingFinacial accountingFinacialService
    ) : base(logger, mapper, mapsterMapper, serviceProvider)
    {
        _storageModuleService = serviceProvider.GetRequiredService<IStorageModuleService>();
        _inventoryCostService = serviceProvider.GetRequiredService<IServiceBase<DtoInventoryCost, TInventoryCost>>();
        _accountingFinacialService = accountingFinacialService;
        _serviceProvider = serviceProvider;
    }

    public IStorageModuleService _storageModuleService { get; }

    public async ValueTask<bool> CanLoseItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _storageModuleService.CanLoseItemQuntity(itemId, unitId, storeId, qun, storageItemProperties);
        return inventory;
    }

    public async ValueTask<(bool CanLoost, DtoInventoryCost? Info)> CanLoseItemQuntityInfo(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null)
    {
        var inventoryInfo = await _storageModuleService.CanLoseItemQuntityInfo(itemId, unitId, storeId, qun, storageItemProperties);
        if (inventoryInfo.Info is null) return (false, null);

        var inventoryCost = await _inventoryCostService.GetAsync(w => w.TInventoryId == inventoryInfo.Info.Id);
        return (inventoryInfo.Info.Quantity >= qun, inventoryCost);
    }

    public async ValueTask<DtoInventoryCost?> GetItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _storageModuleService.GetItemQuntity(itemId, unitId, storeId, storageItemProperties);
        if (inventory is null) return null;

        //var inventoryCost = await _inventoryCostService.GetAsync(w => w.TInventoryId == inventory.Id);
        //return inventoryCost;

        return new DtoInventoryCost()
        {
            Inventory = inventory,
        };
    }

    public ValueTask<List<UnitSaleDetailsQueryResponse>> GetItemUnitsQuantity(IEnumerable<DtoInventory> inventory, List<DtoItemUnit> itemUnits, Identifier storeId, StorageItemProperties? storageItemProperties = null)
    {
        return _storageModuleService.GetItemUnitsQuantity(inventory, itemUnits, storeId, storageItemProperties);
    }


    public async ValueTask<DtoInventory?> GetItemQuntityOrginalItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _storageModuleService.GetItemQuntity(itemId, unitId, storeId, storageItemProperties);
        return inventory;
    }

    public class VInvetoryDetails
    {
        public VInvetoryDetails()
        {

        }
    }


    public async ValueTask<DtoInventoryCost?> RegainItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? avragePiceCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _storageModuleService.RegainItem(itemId, unitId, storeId, quantity, totalQuantity, relatedId, relatedInventory, storageItemProperties);

        //avrageCost ??= cost / quantity;
        var inventoryCost = await _inventoryCostService.Add(new DtoInventoryCost
        {
            Cost = cost,
            Price = price,
            SellPrice = sellPrice,
            TInventoryId = inventory!.Id,
            AvrageCost = avrageCost ?? 0m,
            AvragePiceCost = avragePiceCost,
        });
        return inventoryCost;
    }


    public async ValueTask<DtoInventoryCost?> LoseItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? avragePiceCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _storageModuleService.LoseItem(itemId, unitId, storeId, quantity, totalQuantity, relatedId, relatedInventory, storageItemProperties);
        //avrageCost ??= cost / quantity;
        var inventoryCost = await _inventoryCostService.Add(new DtoInventoryCost
        {
            Cost = cost,
            Price = price,
            SellPrice = sellPrice,
            AvragePiceCost = avragePiceCost,
            TInventoryId = inventory!.Id,
            AvrageCost = avrageCost ?? 0m,
        });
        return inventoryCost;
    }


    public ValueTask<DtoInventoryCost?> RegainOrLoseItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? avragePiceCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool isLose = false, bool softDelete = false, StorageItemProperties? storageItemProperties = null)
    {
        return isLose
                ? softDelete
                    ? LoseItem(itemId, unitId, storeId, quantity, totalQuantity, cost, avrageCost, avragePiceCost, price, sellPrice, relatedId, relatedInventory, storageItemProperties)
                    : LossEver(itemId, unitId, storeId, quantity, quantity, quantity, quantity, quantity, quantity, relatedId, relatedInventory, storageItemProperties: storageItemProperties)
                : RegainItem(itemId, unitId, storeId, quantity, totalQuantity, cost, avrageCost, avragePiceCost, price, sellPrice, relatedId, relatedInventory, storageItemProperties)
            ;
    }


    public async ValueTask<DtoInventoryCost?> LossEver(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool softDelete = false, StorageItemProperties? storageItemProperties = null)
    {
        //  todo: is this in same transaction
        var inv = await _storageModuleService.LossEver(itemId, unitId, storeId, quantity, totalQuantity, relatedId, relatedInventory, softDelete, storageItemProperties);
        if (inv == null) return null;

        var re = await _inventoryCostService.GetAsync(w => w.TInventoryId == inv.Id);
        if (re == null) return null;
        await _inventoryCostService.Remove(re.Id, softDelete);
        return re;
    }


    public async Task LossAllEver(Identifier relatedId, RelatedInventory relatedInventory)
    {
        //  todo: is this in same transaction
        await _inventoryCostService.Remove(
            w =>
                w.Inventory.RelatedId == relatedId && w.Inventory.RelatedIndex == relatedInventory
        );
        await _storageModuleService.LossAllEver(relatedId, relatedInventory);
    }
    public async Task LossAllEver(Identifier relatedId, RelatedInventory[] relatedInventory)
    {
        foreach (var item in relatedInventory)
        {
            await LossAllEver(relatedId, item);
        }
    }


    public async ValueTask<IEnumerable<DtoInventoryCost>> GetDetails(Identifier? storeId = null, Identifier? itemId = null, Identifier? unitId = null, DateTime? from = null, DateTime? to = null, StorageItemProperties? storageItemProperties = null)
    {
        var w = _inventoryCostService.Build();

        if (storeId is not null)
        {
            if (!storeId.Value.IsNullOrEmpty())
                w = w.And(w => w.Inventory.StoreId == storeId);
        }
        if (itemId is not null)
            if (!itemId.Value.IsNullOrEmpty())
                w = w.And(w => w.Inventory.ItemId == itemId);

        if (unitId is not null)
            if (!unitId.Value.IsNullOrEmpty())
                w = w.And(w => w.Inventory.UnitId == unitId);

        if (from is not null && to is not null)
            w = w.And(w => w.CreateDate >= from && to >= w.CreateDate);

        if (storageItemProperties is not null && storageItemProperties.ExpireDate is not null)
            w = w.And(w => w.ExpireDate == storageItemProperties.ExpireDate);

        var inventory = await _inventoryCostService.GetAllAsync(w);
        return inventory.Items;
    }


    public async ValueTask<IEnumerable<DtoInventoryCost>> GetDetails2(Identifier? storeId = null, Identifier? itemId = null, Identifier? unitId = null, StorageItemProperties? storageItemProperties = null)
    {
        var w = _inventoryCostService.Build();

        if (storeId is not null)
        {
            if (!storeId.Value.IsNullOrEmpty())
                w = w.And(w => w.Inventory.StoreId == storeId);
        }
        if (itemId is not null)
            if (!itemId.Value.IsNullOrEmpty())
                w = w.And(w => w.Inventory.ItemId == itemId);

        if (unitId is not null)
            if (!unitId.Value.IsNullOrEmpty())
                w = w.And(w => w.Inventory.UnitId == unitId);


        if (storageItemProperties is not null && storageItemProperties.ExpireDate is not null)
            w = w.And(w => w.ExpireDate == storageItemProperties.ExpireDate);

        var inventory = await _inventoryCostService.GetAllAsync(w);
        return inventory.Items;
    }


    public async ValueTask<decimal> GetAvrageCost(Identifier itemId, Identifier? unitId = null, Identifier? storeId = null)
    {
        var w = _inventoryCostService.Build();
        w = w.And(w => w.Inventory!.ItemId == itemId);
        if (unitId is not null) w = w.And(w => w.Inventory!.UnitId == unitId);
        if (storeId is not null) w = w.And(w => w.Inventory!.StoreId == storeId);


        var all = await _inventoryCostService.GetAllAsync(w);
        var items = all.Items.SkipWhile(w => w.AvrageCost == 0).ToList();

        var last = items.LastOrDefault(w => w.AvrageCost != 0);

        var indx = last != null ? 1 : 0;
        var cost = last != null ? last.AvrageCost : 0;
        var avrageCost = 0m;
        items.ForEach(f =>
        {
            indx++;
            cost += f.Cost;
            avrageCost = cost / indx;
            f.AvrageCost = avrageCost;

        });
        if (items.Any())
        {
            await _inventoryCostService.UpdateRange(items);
        }

        return cost;
    }


    public async ValueTask<IEnumerable<DtoInventoryCost>> GetMovements(Identifier? itemId = null, Identifier? storeId = null, Identifier[]? relatedId = null, RelatedInventory? relatedIndex = null, StorageItemProperties? storageItemProperties = null)
    {
        if (itemId.HasValue && itemId.Value.IsNullOrEmpty()) itemId = null;
        if (storeId.HasValue && storeId.Value.IsNullOrEmpty()) storeId = null;
        if (relatedId?.Any() == false) relatedId = null;

        var w = _inventoryCostService.Build();
        if (itemId is not null) w = w.And(w => w.Inventory!.ItemId == itemId);
        if (storeId is not null) w = w.And(w => w.Inventory!.StoreId == storeId);
        if (relatedId is not null) w = w.And(w => relatedId.Contains(w.Inventory!.RelatedId.Value));
        if (relatedIndex is not null) w = w.And(w => w.Inventory!.RelatedIndex == relatedIndex);
        if (storageItemProperties?.ExpireDate is not null) w = w.And(w => w.Inventory!.ExpireDate == storageItemProperties.ExpireDate);

        var all = await _inventoryCostService.GetAllAsync(w);
        return all.Items;
    }

    public void CalculateCostAndPrice(int initialUnits, int initialCostPerUnit, int additionalUnits, int additionalCostPerUnit, out int totalCost, out double pricePerUnit)
    {
        totalCost = initialUnits * initialCostPerUnit + additionalUnits * additionalCostPerUnit;
        pricePerUnit = (double)totalCost / (initialUnits + additionalUnits);
    }


    public async Task<bool> FinacialSaleProcess(

        Identifier saleId
        , decimal total
        , long invoiceNumber
        , Identifier saleInvoiceBaseAccount
        , PeymentType saleInvoicePeymentType
        , Identifier clientId

        , Identifier? safeAccountId
        , Identifier? generalGlobalShareAccount
        , Identifier? representativeId
        , decimal? representativePercentage
        , bool IsRoleback = false
        , bool softDelete = false)
    {
        //  1
        var finacialTransaction = _accountingFinacialService
            .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                OprationTypeEnum.Sales,
                PurchaseAndSalesConstants.TransactionOprationType__SalesRelated,
                saleId,
                invoiceNumber,
                "");
        //
        finacialTransaction.AppendEntry(clientId.Value, saleInvoiceBaseAccount, total);
        if (representativeId is not null && generalGlobalShareAccount is not null)
        {
            var sharedAmount = total * (representativePercentage / 100);
            finacialTransaction.AppendEntry(
                representativeId.Value, generalGlobalShareAccount.Value, sharedAmount.Value);
        }

        switch (saleInvoicePeymentType)
        {
            case PeymentType.Cash:
                // NOTE: if the safe has `loss percent` ?
                finacialTransaction.AppendEntry(safeAccountId.Value, clientId, total);
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

    public async Task<bool> FinacialPurchesProcess(
        Identifier invoiceId
        , long invoiceNumber
        , PeymentType invoicePeymentType
        , Identifier? expenseInfoAccountId
        , Identifier? safeInfoAccountId
        , Identifier purchaseBaseAccountId
        , Identifier supplierAccountId


        , bool IsRoleback = false, decimal oldTotalCosts = 0, decimal oldTotalPurchasers = 0, bool softDelete = false)
    {
        var countedTotalCosts = oldTotalCosts;
        var countedTotalPurchasers = oldTotalPurchasers;


        if (countedTotalCosts > 0)
        {
            var finacialCostTransaction = _accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    IsRoleback ? OprationTypeEnum.PurchasesCost_Return : OprationTypeEnum.PurchasesCost,
                    PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated,
                    invoiceId,
                    invoiceNumber,
                    "");
            finacialCostTransaction.AppendEntry(expenseInfoAccountId.Value, safeInfoAccountId.Value, countedTotalCosts);
            if (IsRoleback)
            {
                await finacialCostTransaction.RevertTransaction(note: "", softDelete);
            }
            else
            {
                await finacialCostTransaction.ComplateTransaction();
            }
        }
        //  1
        var finacialPurchersTransaction = _accountingFinacialService
            .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                OprationTypeEnum.Purchases,
                PurchaseAndSalesConstants.TransactionOprationType__PurchasesRelated,
                invoiceId,
                invoiceNumber,
                "");
        // Set the invoice deption for the supplier
        finacialPurchersTransaction.AppendEntry(purchaseBaseAccountId, supplierAccountId, countedTotalPurchasers);

        switch (invoicePeymentType)
        {
            case PeymentType.Cash:
                // NOTE: if the safe has `loss percent` ?
                finacialPurchersTransaction.AppendEntry(supplierAccountId, safeInfoAccountId.Value, countedTotalPurchasers);
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

}