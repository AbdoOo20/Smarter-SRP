using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.StoresCategoriesModels;

public class DtoItemUnitModel: DtoItemUnit, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}