using Fly.Core.Common.Attributes;
using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TInventoryCost : TEntity
{

    public bool IsReturn { get; set; }
    public decimal Cost { get; set; }
    public decimal? Price { get; set; }
    public decimal AvrageCost { get; set; }
    public decimal? AvragePiceCost { get; set; }
    public decimal? SellPrice { get; set; }

    public Identifier TInventoryId { get; set; }
    public TInventory? Inventory { get; set; }
}