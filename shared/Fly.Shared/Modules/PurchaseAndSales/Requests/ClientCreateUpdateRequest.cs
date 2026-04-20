using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class ClientCreateUpdateRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }


    public decimal MaxDebt { get; set; }
    public string? Address { get; set; }
}

public class ClientDeleteRequest : DeleteRequestBase
{
}
public class ClientQueryRequest : QueryRequestBase
{
    public Identifier? LinkedId { get; set; }
    public Identifier? MarketerId { get; set; }
}