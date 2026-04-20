using Fly.Core.Common.Attributes;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.Factoring.ManufacturingFacilities.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Factoring.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;

[MapTo]
public class TFactoringInvoice : TEntity, INumberEntity
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

    public Identifier? StoreId { get; set; }
    
    public TSafe? Safe { get; set; }
    
    public Identifier? SafeId { get; set; }
    
    public TBank? Bank { get; set; }
    
    public Identifier? BankId { get; set; }
    
    public TManufacturingFacility? ManufacturingFacility { get; set; }
    
    public Identifier? ManufacturingFacilityId { get; set; }

    public decimal Discount { get; set; }
    
    public decimal CollectionRate { get; set; }

    public string? Notes { get; set; } 
        
    public bool ReceivedFeeInAdvance { get; set; }

    public DiscountType DiscountType { get; set; }
    
    public FactoringStatus FactoringStatus { get; set; }
    
    public FactoringItemsStatus FactoringItemsStatus { get; set; }

    public PeymentType PaymentType { get; set; }
    
    public virtual ICollection<TFactoringInvoiceDetail> Details { get; set; } = [];
    
    public virtual ICollection<TFactoringInvoiceCost> Costs { get; set; } = [];
    
    public virtual ICollection<TFactoringInvoiceComposition> Compositions { get; set; } = [];
}