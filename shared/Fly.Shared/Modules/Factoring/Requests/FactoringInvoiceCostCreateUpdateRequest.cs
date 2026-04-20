using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Factoring.Requests;

public class FactoringInvoiceCostCreateUpdateRequest : CreateUpdateRequestBase
{
    public Identifier ItemCompositionId { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Discount { get; set; }
    
    public Identifier FactoringInvoiceId { get; set; }
    
    public string? Notes { get; set; }
}

public class FactoringInvoiceCostCreateRequest
{
        
}

public class FactoringInvoiceCostQueryRequest : QueryRequestBase
{ }

public class FactoringInvoiceCostLookupRequest : FactoringInvoiceCostQueryRequest
{ }

public class FactoringInvoiceCostDeleteRequest : DeleteRequestBase
{ }