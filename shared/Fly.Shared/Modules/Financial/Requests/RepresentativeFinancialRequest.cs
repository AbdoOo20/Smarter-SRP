using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Shared.Modules.Financial.Requests;

public class RepresentativeFinancialRequest : RSimpleRequest
{
    public Identifier MarketerId { get; set; }
    public Identifier ClientId { get; set; }
    public Identifier SafeId { get; set; }
    public decimal MarketerShare { get; set; }
}
