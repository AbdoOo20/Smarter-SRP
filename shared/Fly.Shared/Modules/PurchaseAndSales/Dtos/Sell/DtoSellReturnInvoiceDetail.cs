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

    /// <summary>
    /// Quantity and line discount of the sales invoice line this return line comes
    /// from. Not stored; the backend fills them from the linked invoice so a partial
    /// return gets only its share of the line discount.
    /// </summary>
    [ClientInvisible]
    public decimal SourceQuantity { get; set; }

    [ClientInvisible]
    public decimal SourceDiscount { get; set; }

    /// <summary>
    /// The line discount in money. As on the sales invoice, a line discount is a
    /// fixed amount for the whole line; returning part of the line takes the same
    /// share of it.
    /// </summary>
    public decimal LineDiscount()
    {
        if (SourceQuantity <= 0)
            return Discount;

        var share = Math.Min(1m, Quantity / SourceQuantity);
        return decimal.Round(SourceDiscount * share, 2);
    }

    public decimal TotalAmount => Price * Quantity - LineDiscount();

    /// <summary>
    /// Line total. mode -100 = before any discount; any other value = after the
    /// line discount. (The line discount used to be read as a percentage when the
    /// invoice discount was a percentage, unlike the sales invoice.)
    /// </summary>
    public decimal CountingPrince(int mode)
    {
        var realPrice = Price * Quantity;
        return mode == -100 ? realPrice : realPrice - LineDiscount();
    }

    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}