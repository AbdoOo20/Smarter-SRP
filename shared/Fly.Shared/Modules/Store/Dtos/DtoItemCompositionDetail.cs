using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Dtos;

public partial class DtoItemCompositionDetail : DtoBase, IDetailsModel
{
    public string? ItemName { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public string? UnitName { get; set; }
    
    public Identifier UnitId { get; set; }
    
    public string? ItemVarianceName { get; set; }
    
    public Identifier? ItemVarianceId { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public Identifier ItemCompositionId { get; set; }

    public int Quantity { get; set; } = 1;
    
    [ObservableProperty]
    public partial decimal Price { get; set; }

    [ClientInvisible, ObservableProperty]
    public partial int CompositionIndex { get; set; } = 1;
    
    public decimal CountPrice()
    {
        var realPrice = Math.Round(Price * Quantity, 3);
        decimal final = realPrice;
        return Math.Round(final, 3);
    }
}