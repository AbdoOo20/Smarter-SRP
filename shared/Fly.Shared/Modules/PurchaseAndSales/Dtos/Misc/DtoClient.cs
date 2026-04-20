using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

public class DtoClient : DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    [ClientInvisible]
    public string Identifier { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public long AccountNumber { get; set; }

    public decimal MaxDebt { get; set; }

    public string? Address { get; set; }
    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }
}