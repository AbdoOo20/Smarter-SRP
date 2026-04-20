using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Maintenance.Requests;

public class MaintenanceInvoiceDetailCreateUpdateRequest : CreateUpdateRequestBase
{
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    public decimal Quantity { get; set; } 

    public decimal Discount { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public Identifier UnitId { get; set; }
    
    public Identifier? ItemVarianceId { get; set; }
    
    public Identifier MaintenanceInvoiceId { get; set; }
}

public class MaintenanceInvoiceDetailCreateRequest
{
        
}

public class MaintenanceInvoiceDetailQueryRequest : QueryRequestBase
{ }

public class MaintenanceInvoiceDetailLookupRequest : MaintenanceInvoiceDetailQueryRequest
{ }

public class MaintenanceInvoiceDetailDeleteRequest : DeleteRequestBase
{ }