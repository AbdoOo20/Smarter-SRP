using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Factoring.Requests;

public class FactoringInvoiceCompositionCreateUpdateRequest : CreateUpdateRequestBase
{
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    public decimal Discount { get; set; }
    
    public Identifier ItemCompositionId { get; set; }
    
    public Identifier FactoringInvoiceId { get; set; }
    
    public int CompositionIndex { get; set; }
    
    public Identifier CompositionDetailBindingId { get; set; }
}

public class FactoringInvoiceCompositionCreateRequest
{
        
}

public class FactoringInvoiceCompositionQueryRequest : QueryRequestBase
{ }

public class FactoringInvoiceCompositionLookupRequest : FactoringInvoiceCompositionQueryRequest
{ }

public class FactoringInvoiceCompositionDeleteRequest : DeleteRequestBase
{ }