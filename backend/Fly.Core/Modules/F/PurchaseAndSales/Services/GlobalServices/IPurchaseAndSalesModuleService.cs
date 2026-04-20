using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;

public interface IPurchaseAndSalesModuleService
{

    IStorageModuleService _storageModuleService { get; }

    ValueTask<bool> CanLoseItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null);
    ValueTask<(bool CanLoost, DtoInventoryCost? Info)> CanLoseItemQuntityInfo(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null);
    Task<bool> FinacialPurchesProcess(Identifier invoiceId, long invoiceNumber, PeymentType invoicePeymentType, Identifier? expenseInfoAccountId, Identifier? safeInfoAccountId, Identifier purchaseBaseAccountId, Identifier supplierAccountId, bool IsRoleback = false, decimal oldTotalCosts = 0, decimal oldTotalPurchasers = 0, bool softDelete = false);
    Task<bool> FinacialSaleProcess(

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
        , bool softDelete = false);
    ValueTask<IEnumerable<DtoInventoryCost>> GetDetails(Identifier? storeId = null, Identifier? itemId = null, Identifier? unitId = null, DateTime? from = null, DateTime? to = null, StorageItemProperties? storageItemProperties = null);

    //  Methods
    ValueTask<DtoInventoryCost?> GetItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, StorageItemProperties? storageItemProperties = null);
    ValueTask<List<UnitSaleDetailsQueryResponse>> GetItemUnitsQuantity(IEnumerable<DtoInventory> inventory, List<DtoItemUnit> itemUnits, Identifier storeId, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoInventory?> GetItemQuntityOrginalItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, StorageItemProperties? storageItemProperties = null);
    ValueTask<IEnumerable<DtoInventoryCost>> GetMovements(Identifier? itemId = null, Identifier? storeId = null, Identifier[]? relatedId = null, RelatedInventory? relatedIndex = null, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoInventoryCost?> LoseItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? avragePiceCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null);
    Task LossAllEver(Identifier relatedId, RelatedInventory relatedInventory);
    Task LossAllEver(Identifier relatedId, RelatedInventory[] relatedInventory);
    ValueTask<DtoInventoryCost?> LossEver(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool softDelete = false, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoInventoryCost?> RegainItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? avragePiceCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoInventoryCost?> RegainOrLoseItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, decimal cost, decimal? avrageCost = null, decimal? avragePiceCost = null, decimal? price = null, decimal? sellPrice = null, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool isLose = false, bool softDelete = false, StorageItemProperties? storageItemProperties = null);
}