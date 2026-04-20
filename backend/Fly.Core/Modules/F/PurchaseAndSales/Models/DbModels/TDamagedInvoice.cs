using Fly.Core.Common.Attributes;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;


namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;


[MapTo]
public class TDamagedInvoice : TEntity, INumberEntity
{

    public long Number { get; set; }
    public DateTime Date { get; set; }
    public long SerialNumber { get; set; }

    public decimal TotalCost { get; set; }
    public decimal TotalAmount { get; set; }


    public TMarketer? MarketerStore{ get; set; } 
    public Identifier? MarketerStoreId { get; set; }
    public TStore? Store { get; set; }
    public Identifier? StoreId { get; set; }


    public virtual ICollection<TDamagedInvoiceDetail> Details { get; set; } = [];

    public string? Notes { get; set; }
}