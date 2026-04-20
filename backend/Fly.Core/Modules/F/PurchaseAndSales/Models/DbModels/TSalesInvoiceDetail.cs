using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TSalesInvoiceDetail : TEntity
{
    public Identifier SalesInvoiceId { get; set; }
    public TSalesInvoice? SalesInvoice { get; set; }

    public TUnit? Unit { get; set; }
    public Identifier UnitId { get; set; }

    public TItem? Item { get; set; }
    public Identifier ItemId { get; set; }

    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public decimal Discount { get; set; }


    public DateTime? ExpireDate { get; set; }
}