using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

public class DtoDisbursementPermitDetailModel: DtoDisbursementPermitDetail, IModel
{
    public new string? UnitName { get; set; }
    
    public new string? ItemName { get; set; }

    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public new DateTime ExpireDate { get; set; } = DateTime.Now;
}