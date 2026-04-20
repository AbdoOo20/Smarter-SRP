using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Accounting.Requests;

public class CreateUpdateCurrencyRequest : CreateUpdateRequestBase
{
    public bool IsPrimary { get; set; }
    public decimal TransfereRate { get; set; }
    public string Name { get; set; } = string.Empty;
}