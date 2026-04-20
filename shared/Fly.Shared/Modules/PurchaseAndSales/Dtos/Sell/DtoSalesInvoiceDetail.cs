using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Framework.Shared.Extentions;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

public partial class DtoSalesInvoiceDetail : DtoBase, IDetailsModel
{
    [ClientInvisible]
    public Identifier SalesInvoiceId { get; set; }

    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    public decimal Quantity { get; set; } = 1;
    
    [ObservableProperty]
    public partial decimal Price { get; set; }
    
    public string ItemBarcode { get; set; } = string.Empty;

    public decimal Discount { get; set; }

    public decimal TotalAmount => Price * Quantity - Discount;
    
    [ClientInvisible]
    public DateTime? ExpireDate { get; set; }
    public string? ExpireDateString => ExpireDate?.ToDottedDate();
    
    public decimal CountItemPrice(bool withDiscount)
    {
        var realPrice = Math.Round(Price * Quantity, 3);
        var final = realPrice;

        if (withDiscount)
        {
            final -= Discount;
        }
        
        return Math.Round(final, 3);
    }

    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}