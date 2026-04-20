using Fly.Core.Common.Attributes;
using Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Maintenance.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;

[MapTo]
public class TMaintenanceInvoice : TEntity, INumberEntity
{
    public DateTime Date { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public long Number { get; set; }
    
    public bool IsPaid { get; set; }
    
    public bool IsInternal { get; set; }

    public TClient? Client { get; set; }
    
    public Identifier? ClientId { get; set; }
    
    public TStore? Store { get; set; }

    public Identifier StoreId { get; set; }
    
    public TSafe? Safe { get; set; }
    
    public Identifier? SafeId { get; set; }
    
    public TBank? Bank { get; set; }
    
    public Identifier? BankId { get; set; }
    
    public TMaintenanceSector? MaintenanceSector { get; set; }

    public Identifier? MaintenanceSectorId { get; set; }

    public decimal Discount { get; set; }
    
    public decimal CollectionRate { get; set; }
    
    public string? Notes { get; set; }
    
    public DiscountType DiscountType { get; set; }
    
    public MaintenanceStatus MaintenanceStatus { get; set; }
    
    public MaintenanceItemsStatus MaintenanceItemsStatus { get; set; }

    public PeymentType PaymentType { get; set; }

    public bool ReceivedFeeInAdvance { get; set; }
    
    public virtual ICollection<TMaintenanceInvoiceDetail> Details { get; set; } = [];
    
    public virtual ICollection<TMaintenanceInvoiceCost> Costs { get; set; } = [];
}