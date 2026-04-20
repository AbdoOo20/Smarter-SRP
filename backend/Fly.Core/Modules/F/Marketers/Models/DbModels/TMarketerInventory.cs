using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerInventory : TEntity
{
    public Identifier ItemId { get; set; }
        
    public TItem? Item { get; set; }
        
    public Identifier UnitId { get; set; }
        
    public TUnit? Unit { get; set; }

    public Identifier MarketerId { get; set; }
        
    public TMarketer? Marketer { get; set; }

    public int? Size { get; set; } = null;
        
    public int? Color { get; set; } = null;
        
    public DateTime? ExpireDate { get; set; } = null;

    public bool Ingress { get; set; }
        
    public decimal Quantity { get; set; }
        
    public decimal TotalQuantity { get; set; }

    public Identifier? RelatedId { get; set; }
        
    public RelatedInventory RelatedIndex { get; set; }
}