using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateItemCompositionDetailRequest
{
    public Identifier UnitId { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public Identifier? ItemVarianceId { get; set; }
    
    public Identifier ItemCompositionId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
    
    public int CompositionIndex { get; set; } = 1;
}