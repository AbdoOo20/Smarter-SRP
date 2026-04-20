using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Maintenance.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Maintenance.Requests;

public class MaintenanceInvoiceCreateUpdateRequest : CreateUpdateRequestBase
{
    public DateTime Date { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public long Number { get; set; }
    
    public bool IsPaid { get; set; }
    
    public bool IsInternal { get; set; }
    
    public decimal CollectionRate { get; set; }

    public Identifier? ClientId { get; set; }
    
    public Identifier? StoreId { get; set; }
    
    public Identifier? SafeId { get; set; }
    
    public Identifier? BankId { get; set; }
    
    public Identifier? MaintenanceSectorId { get; set; }

    public decimal Discount { get; set; }
    
    public DiscountType DiscountType { get; set; }

    public MaintenanceStatus MaintenanceStatus { get; set; }
    
    public MaintenanceItemsStatus MaintenanceItemStatus { get; set; }

    public PeymentType PaymentType { get; set; }

    public string? Notes { get; set; }

    public bool ReceivedFeeInAdvance { get; set; }
    
    public virtual ICollection<MaintenanceInvoiceDetailCreateUpdateRequest> Details { get; set; } = [];
    
    public virtual ICollection<MaintenanceInvoiceCostCreateUpdateRequest> Costs { get; set; } = [];
}

public class MaintenanceInvoiceCreateRequest: MaintenanceInvoiceCreateUpdateRequest
{
        
}

public class MaintenanceInvoiceQueryRequest : QueryRequestBase
{ }

public class MaintenanceInvoiceLookupRequest : MaintenanceInvoiceQueryRequest
{ }

public class MaintenanceInvoiceDeleteRequest : DeleteRequestBase
{ }