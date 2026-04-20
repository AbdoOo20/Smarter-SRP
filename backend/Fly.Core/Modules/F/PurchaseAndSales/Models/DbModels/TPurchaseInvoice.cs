using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TPurchaseInvoice : TEntity, INumberEntity
{
    public long SerialNumber { get; set; }
    public long Number { get; set; }
    public bool IsPaid { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalAmount { get; set; }

    public DateTime Date { get; set; }

    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }

    public TSupplier? Supplier { get; set; }
    public Identifier? SupplierId { get; set; }

    public TStore? Store { get; set; }
    public Identifier StoreId { get; set; }

    public TSafe? Safe { get; set; }
    public Identifier? SafeId { get; set; }

    public TBank? Bank { get; set; }
    public Identifier? BankId { get; set; }
    
    public PeymentType PeymentType { get; set; }


    public virtual ICollection<TPurchaseInvoiceCost> Costs { get; set; } = [];
    public virtual ICollection<TPurchaseInvoiceDetail> Details { get; set; } = [];

    public string? Notes { get; set; }
    public bool IsCostOnSupplier { get; set; }

    public decimal NetAmount { get; set; }
    public decimal DiscountAmount { get; set; }
}