using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;


[MapTo]
public class TQuantityCreditingInvoice : TEntity
{
    public DateTime Date { get; set; }
    public long SerialNumber { get; set; }
    public long Number { get; set; }

    public TStore? Store { get; set; }
    public Identifier? StoreId { get; set; }

    public virtual ICollection<TQuantityCreditingInvoiceDetail> Details { get; set; } = [];
    public string? Notes { get; set; }
}