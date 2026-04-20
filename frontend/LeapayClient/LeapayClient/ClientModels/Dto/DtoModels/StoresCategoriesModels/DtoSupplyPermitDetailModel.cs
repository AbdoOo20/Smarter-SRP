using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.StoresCategoriesModels;

public class DtoSupplyPermitDetailModel: DtoSupplyPermitDetail, IDetailsModel
{
    public new string? UnitName { get; set; }
    
    public new string? ItemName { get; set; }

    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());

    public new DateTime ExpireDate { get; set; } = DateTime.Now;
}