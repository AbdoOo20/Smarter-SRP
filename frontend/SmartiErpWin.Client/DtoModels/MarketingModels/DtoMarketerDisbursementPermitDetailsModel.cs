using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Modules.Marketers.Dtos;

namespace SmartiErpWin.Client.DtoModels.MarketingModels;

public class DtoMarketerDisbursementPermitDetailsModel: DtoMarketerDisbursementPermitDetail, IModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());

    [ClientInvisible]
    public Identifier? TemporaryStoreId { get; set; }

}