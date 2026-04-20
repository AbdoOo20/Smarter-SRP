using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

public class DtoItemUnitModel: DtoItemUnit, IModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}