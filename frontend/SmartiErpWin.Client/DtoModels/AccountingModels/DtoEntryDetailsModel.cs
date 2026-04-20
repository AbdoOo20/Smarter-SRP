using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.DtoModels.AccountingModels;

public class DtoEntryDetailModel: DtoEntryDetail, IModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}