using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo(typeof(DtoItemCompositionDetail))]
public class TItemCompositionDetail : TEntity
{
    public TUnit? Unit { get; set; }
    public Identifier UnitId { get; set; }
    
    public TItem? Item { get; set; }
    public Identifier ItemId { get; set; }
    
    public TItemComposition? ItemComposition { get; set; }
    public Identifier ItemCompositionId { get; set; }
    
    public TItemVariance? ItemVariance { get; set; }
    
    public Identifier? ItemVarianceId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
    
    public int CompositionIndex { get; set; }
}