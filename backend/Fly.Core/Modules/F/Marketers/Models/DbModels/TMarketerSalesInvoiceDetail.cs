using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerSalesInvoiceDetail : TEntity
{
    public Identifier MarketerSalesInvoiceId { get; set; }
    public TMarketerSalesInvoice? MarketerSalesInvoice { get; set; }

    public TUnit? Unit { get; set; }
    public Identifier UnitId { get; set; }

    public TItem? Item { get; set; }
    public Identifier ItemId { get; set; }

    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public decimal Discount { get; set; }
}