using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

public class DtoBankBranch : DtoBase
{
    public string Name { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier BankId { get; set; }
    public string BankName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public long AccountNumber { get; set; }
    public string? Note { get; set; }

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }
}