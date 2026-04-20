using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Core.Modules.F.Marketers.Services.InternalServices;

internal interface IMarketerInventoryService : IServiceBase<DtoMarketerInventory, TMarketerInventory>
{
    ValueTask<IEnumerable<DtoMarketerInventory>> GetDetails(Identifier? marketerId = null, Identifier? itemId = null, Identifier? unitId = null, DateTime? from = null, DateTime? to = null);
    ValueTask<IEnumerable<DtoMarketerInventory>> GetMovements(Identifier? itemId = null, Identifier? marketerId = null, Identifier[]? relatedId = null, RelatedInventory? relatedIndex = null, StorageItemProperties? storageItemProperties = null);
}