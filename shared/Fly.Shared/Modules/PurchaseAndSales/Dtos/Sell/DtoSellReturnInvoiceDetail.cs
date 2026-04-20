using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

public class DtoSellReturnInvoiceDetail : DtoBase, IDetailsModel
{
    [ClientInvisible]
    public Identifier ReturnInvoiceId { get; set; }

    [ClientInvisible]
    public long ReturnInvoiceNumber { get; set; }

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemBarcode { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }

    public decimal Discount { get; set; }

    public DateTime? ExpireDate { get; set; }

    public decimal TotalAmount => Price * Quantity - Discount;

    public decimal CountingPrince(int mode)
    {
        var realPrice = Price * Quantity;
        if (mode == (int)DiscountType.Percent)
        {
            return realPrice - realPrice * (Discount / 100);
        }
        else if (mode == (int)DiscountType.Static)
        {
            return realPrice - Discount;
        }
        else
        {
            return realPrice;
        }
    }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}