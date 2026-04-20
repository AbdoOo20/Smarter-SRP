using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;
using System.Data;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Core.Modules.E.Store.Services.ModuleService;

public interface IStorageModuleService
{
    ValueTask<bool> CanLoseItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null);
    ValueTask<(bool CanLoost, DtoInventory? Info)> CanLoseItemQuntityInfo(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null);
    //  Methods
    ValueTask<DtoInventory?> GetItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, StorageItemProperties? storageItemProperties = null);

    ValueTask<List<UnitSaleDetailsQueryResponse>> GetItemUnitsQuantity(IEnumerable<DtoInventory> inventory, List<DtoItemUnit> itemUnits, Identifier storeId,
        StorageItemProperties? storageItemProperties = null);
    ValueTask<IEnumerable<LookupOf<string, DateTime>>> GetItemExpireDates(Identifier itemId, Identifier storeId);
    ValueTask<DtoInventory?> LoseItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null);
    Task LossAllEver(Identifier relatedId, RelatedInventory relatedInventory);
    ValueTask<DtoInventory?> LossEver(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool softDelete = false, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoInventory?> RegainItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoInventory> RegainOrLoseItem(Identifier itmId, Identifier untId, Identifier strId, decimal qun, decimal allOnes, Identifier relatedId, RelatedInventory relatedInventory, bool isLose, bool softDelete, StorageItemProperties? storageItemProperties = null);
    ValueTask<MoveItemsRequest> TransferItemsBetweenStoragesAsync(MoveItemsRequest request);
    ValueTask<DamageItemsRequest> MarkItemsAsDamagedAsync(DamageItemsRequest request);
    void SetTransaction(IDbTransaction dbTransaction);
    ValueTask<IEnumerable<DtoInventory>> GetMovements(Identifier? itemId = null, Identifier? storeId = null, Identifier[]? relatedId = null, RelatedInventory? relatedIndex = null, StorageItemProperties? storageItemProperties = null);
}