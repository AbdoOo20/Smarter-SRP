using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Core.Modules.E.Store.Models.DbModels;

[MapTo]
public class TInventory : TEntity
{

    public Identifier ItemId { get; set; }
    public TItem? Item { get; set; }

    public Identifier UnitId { get; set; }
    public TUnit? Unit { get; set; }

    public Identifier StoreId { get; set; }
    public TStore? Store { get; set; }

    public int? Size { get; set; } = null;
    public int? Color { get; set; } = null;
    public DateTime? ExpireDate { get; set; } = null;

    public bool Ingress { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalQuantity { get; set; }
    
    public Identifier? RelatedId { get; set; }
    public RelatedInventory RelatedIndex { get; set; }

}