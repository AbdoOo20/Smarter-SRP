using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Accounting.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.AccountingModels;

public class DtoEntryDetailModel: DtoEntryDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}