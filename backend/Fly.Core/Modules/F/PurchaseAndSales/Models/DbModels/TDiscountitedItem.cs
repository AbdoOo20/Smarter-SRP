using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TDiscountitedItem : TEntity
{
    public DateTime Date { get; set; }
    public DateTime ExpireDate { get; set; }

    public long Number { get; set; }
    public decimal Ammount { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalQuantity { get; set; }

    public Identifier ClientId { get; set; }
    public TClient? Client { get; set; }

    public Identifier ItemId { get; set; }
    public TItem? Item { get; set; }

    public Identifier UnitId { get; set; }
    public TUnit? Unit { get; set; }


    public Identifier ReleatedId { get; set; }
}