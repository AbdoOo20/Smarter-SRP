using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Store.Dtos;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos;

public class DtoInventoryCost : DtoBase
{

    public DtoInventory? Inventory { get; set; }
    public Identifier TInventoryId { get; set; }

    public bool IsReturn { get; set; }

    public decimal Cost { get; set; }
    public decimal? Price { get; set; }
    public decimal AvrageCost { get; set; }
    public decimal? AvragePiceCost { get; set; }
    public decimal? SellPrice { get; set; }

    [ClientInvisible]
    public Identifier? ItemId => Inventory?.ItemId;
    [ClientInvisible]
    public string? ItemName => Inventory?.ItemName;

    [ClientInvisible]
    public Identifier? UnitId => Inventory?.UnitId;
    [ClientInvisible]
    public string? UnitName => Inventory?.UnitName;

    [ClientInvisible]
    public Identifier? StoreId => Inventory?.StoreId;

    [ClientInvisible]
    public string? StoreName => Inventory?.StoreName;

    [ClientInvisible]
    public DateTime? ExpireDate => Inventory?.ExpireDate;
}

