using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;
using System.Data;

namespace Fly.Core.Modules.F.Marketers.Services.GlobalServices;


public interface IMarketerModuleService
{

    ValueTask<bool> CanLoseItemQuntity(Identifier itemId, Identifier unitId, Identifier marketerId, decimal qun);
    ValueTask<(bool CanLoost, DtoMarketerInventory? Info)> CanLoseItemQuntityInfo(Identifier itemId, Identifier unitId, Identifier marketerId, decimal qun, Identifier? ignoredRelated = null);
    ValueTask<IEnumerable<LookupOf<string, DateTime>>> GetItemExpireDates(Identifier itemId, Identifier marketerId);

    //  Methods
    ValueTask<DtoMarketerInventory?> GetItemQuntity(Identifier itemId, Identifier unitId, Identifier marketerId, Identifier? ignoredRealted = null);
    ValueTask<DtoMarketerInventory?> LoseItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null);
    Task LossAllEver(Identifier relatedId, RelatedInventory relatedInventory);
    ValueTask<DtoMarketerInventory?> LossEver(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool softDelete = false, StorageItemProperties? storageItemProperties = null);
    ValueTask<DtoMarketerInventory?> RegainItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null);

    ValueTask<DtoMarketerInventory?> RegainOrLoseItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool isLose = false, bool softDelete = false, StorageItemProperties? storageItemProperties = null);
    void SetTransaction(IDbTransaction dbTransaction);
}