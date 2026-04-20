using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Maintenance.Requests;

public class MaintenanceInvoiceCostCreateUpdateRequest : CreateUpdateRequestBase
{
    public Identifier ItemCompositionId { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Discount { get; set; }
    
    public Identifier MaintenanceInvoiceId { get; set; }
    
    public string? Notes { get; set; }
}

public class MaintenanceInvoiceCostCreateRequest
{
        
}

public class MaintenanceInvoiceCostQueryRequest : QueryRequestBase
{ }

public class MaintenanceInvoiceCostLookupRequest : MaintenanceInvoiceCostQueryRequest
{ }

public class MaintenanceInvoiceCostDeleteRequest : DeleteRequestBase
{ }