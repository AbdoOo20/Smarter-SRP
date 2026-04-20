using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Factoring.Dtos;

public partial class DtoFactoringInvoiceComposition : DtoBase, IDetailsModel
{
    public Identifier? ItemCompositionId { get; set; }

    public string? ItemCompositionName { get; set; }
    
    [ObservableProperty, ClientInvisible]
    public partial decimal Price { get; set; }

    [ClientInvisible]
    public decimal Discount { get; set; }
    
    public decimal Quantity { get; set; } = 1;

    [ObservableProperty, DisplayName("CompositionNumber")]
    public partial int CompositionIndex { get; set; } = 1;
    
    public Identifier FactoringInvoiceId { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public Identifier CompositionDetailBindingId { get; set; }
    
    public decimal CountPrice()
    {
        var realPrice = Math.Round(Price * Quantity, 3);
        return Math.Round(realPrice, 3);
    }
}