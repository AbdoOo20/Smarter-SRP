using Fly.Core.Common.Attributes;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerSalesInvoice : TEntity
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public long Number { get; set; }
    public bool IsPaid { get; set; }

    public TClient? Client { get; set; }
    public Identifier ClientId { get; set; }

    public TMarketer? Marketer { get; set; }
    public Identifier MarketerId { get; set; }

    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }

    public PeymentType PeymentType { get; set; }

    public virtual ICollection<TMarketerSalesInvoiceDetail> Details { get; set; } = [];
}