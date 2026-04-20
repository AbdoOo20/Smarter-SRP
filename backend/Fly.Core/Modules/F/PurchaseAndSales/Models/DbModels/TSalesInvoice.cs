using Fly.Core.Common.Attributes;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;


[MapTo]
public class TSalesInvoice : TEntity, INumberEntity
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public long SerialNumber { get; set; }
    public long Number { get; set; }
    public bool IsPaid { get; set; }

    public TMarketer? Representative { get; set; }
    public Identifier? RepresentativeId { get; set; }
    public decimal RepresentativePercentage { get; set; }

    public TClient? Client { get; set; }
    public Identifier? ClientId { get; set; }

    public TStore? Store { get; set; }
    public Identifier? StoreId { get; set; }

    public TMarketer? MarketerStorage { get; set; }
    public Identifier? MarketerStorageId { get; set; }

    public StorageType StorageType { get; set; }

    public TSafe? Safe { get; set; }
    public Identifier? SafeId { get; set; }
    
    public TBank? Bank { get; set; }

    public Identifier? BankId { get; set; }

    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; }

    public DiscountType DiscountType { get; set; }
    public TaxType TaxType { get; set; }

    public PeymentType PeymentType { get; set; }


    public virtual ICollection<TSalesInvoiceDetail> Details { get; set; } = [];
    public string? Notes { get; set; }

    public decimal NetAmount { get; set; }
    public decimal DiscountAmount { get; set; }
}