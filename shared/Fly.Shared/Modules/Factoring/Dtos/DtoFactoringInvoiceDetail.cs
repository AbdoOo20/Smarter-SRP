using System.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Factoring.Dtos;

public class DtoFactoringInvoiceDetail : DtoBase, IDetailsModel
{
    public Identifier ItemId { get; set; }

    public string? ItemName { get; set; }
    
    public Identifier UnitId { get; set; }

    public string? UnitName { get; set; }
    
    public decimal Price { get; set; }

    public decimal Quantity { get; set; } = 1;
    
    public decimal Discount { get; set; }
    
    public Identifier FactoringInvoiceId { get; set; }
    
    [ClientInvisible]
    public Identifier? SourceCompositionId { get; set; }

    [ClientInvisible]
    public Identifier CompositionDetailBindingId { get; set; }

    
    [DisplayName("ItemCompositionName")]
    public string? SourceCompositionName { get; set; } = "مادة خام بدون تركيبة";

    [DisplayName("CompositionNumber")]
    public int SourceCompositionIndex { get; set; } = 0;
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public decimal CountPrice(int mode)
    {
        var realPrice = Math.Round(Price * Quantity, 3);
        decimal final = realPrice;
        if (mode == (int)DiscountType.Percent)
        {
            final = realPrice - realPrice * (Discount / 100);
        }
        else if (mode == (int)DiscountType.Static)
        {
            final = realPrice - Discount;
        }
        return Math.Round(final, 3);
    }
}