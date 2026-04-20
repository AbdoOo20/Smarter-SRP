using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Marketers.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.MarketingModels;

public class DtoMarketerDisbursementPermitDetailsModel: DtoMarketerDisbursementPermitDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());

    [ClientInvisible]
    public Identifier? TemporaryStoreId { get; set; }

}