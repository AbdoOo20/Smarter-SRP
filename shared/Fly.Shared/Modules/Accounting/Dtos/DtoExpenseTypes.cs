using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Accounting.Dtos;

public class DtoExpenseTypes : DtoNamesBase
{
    [ClientInvisible]
    public Identifier? AccountId { get; set; }

    [ClientInvisible]
    public string? AccountName { get; set; } = string.Empty;
}