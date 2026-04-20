using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Accounting.Dtos;

public class DtoAssets : DtoBase
{
    public string Name { get; set; } = string.Empty;

    public decimal DepreciationRate { get; set; }

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
}