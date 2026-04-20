using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Factoring.Requests;

public class FactoringInvoiceDetailCreateUpdateRequest : CreateUpdateRequestBase
{
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    public decimal Discount { get; set; }
    
    public Identifier? ItemId { get; set; }
    
    public Identifier? UnitId { get; set; }
    
    public Identifier FactoringInvoiceId { get; set; }
    
    public Identifier? SourceCompositionId { get; set; }

    public string? SourceCompositionName { get; set; }

    public int SourceCompositionIndex { get; set; } = 0;
    
    public Identifier CompositionDetailBindingId { get; set; }
}

public class FactoringInvoiceDetailCreateRequest
{
        
}

public class FactoringInvoiceDetailQueryRequest : QueryRequestBase
{ }

public class FactoringInvoiceDetailLookupRequest : FactoringInvoiceDetailQueryRequest
{ }

public class FactoringInvoiceDetailDeleteRequest : DeleteRequestBase
{ }