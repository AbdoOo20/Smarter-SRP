using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Financial.Requests;

namespace Fly.Shared.Modules.Reporting.Requests;

public class SalesInvoiceDetailRequest
{
    public Identifier? MarketerId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Identifier? CLientId { get; set; }
}
public class ClientsOverdueDebtsRequest : DynamicFieldRequest
{
    public Identifier? AccountId { get; set; }
    public int? Days { get; set; }
}
public class RepresentativeAmountsRequest : DynamicFieldRequest
{
    public Identifier? AccountId { get; set; }
}