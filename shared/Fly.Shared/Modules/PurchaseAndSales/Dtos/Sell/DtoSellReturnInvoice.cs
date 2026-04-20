using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

public partial class DtoSellReturnInvoice : DtoBase
{
    public long SerialNumber { get; set; }
    public long Number { get; set; }
    
    [ClientInvisible]
    [ObservableProperty]
    public partial DateTime Date { get; set; }

    public string TimwFormated => Date.ToString("HH:mm") ?? "";
    public string DateFormated => Date.ToString("yyyy.MM.dd") ?? "";

    public decimal TotalAmount { get; set; }

    [ClientInvisible]
    public bool IsPaid { get; set; }

    [ClientInvisible]
    public Identifier? ClientId { get; set; }
    public string? ClientName { get; set; } = string.Empty;

    [ClientInvisible]
    public string? ClientPhone { get; set; } = string.Empty;


    [ClientInvisible]
    public Identifier? InvoiceId { get; set; }
    public long? InvoiceNumber { get; set; }


    [ClientInvisible]
    public Identifier? StoreId { get; set; }

    [ClientInvisible]
    public StorageType StorageType { get; set; }

    [ClientInvisible]
    public Identifier? MarketerStorageId { get; set; }

    public string StoreName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier? SafeId { get; set; }
    public string? SafeName { get; set; }

    [ClientInvisible]
    public DiscountType DiscountType { get; set; }

    [ClientInvisible]
    public decimal Discount { get; set; }

    public PeymentType PeymentType { get; set; }

    [ClientInvisible]
    public virtual List<DtoSellReturnInvoiceDetail> Details { get; set; } = [];

    [ClientInvisible]
    public string DetailsString
    {
        get
        {
            var def = "";


            if (Details?.Any() == true)
            {
                var tt = new List<string>();

                foreach (var item in Details)
                {
                    tt.Add($"[{item.Quantity} {item.UnitName} {item.ItemName}]");
                }
                def = string.Join(", ", tt);
            }


            return def;
        }
    }

    public string? Notes { get; set; }

    [ClientInvisible]
    public decimal Balance { get; set; }

    [ClientInvisible]
    public DtoSalesInvoice? SellInvoice { get; set; } = null;


    public decimal CountingPrice()
    {
        // it's a sum items times quantity with items discounts,
        // then add invoice discount
        // then add any extra amounts.
        // for now, no extra amount.
        var total = Details.Sum(s => s.CountingPrince((int)DiscountType));

        return DiscountType switch
        {
            DiscountType.Static => total - Discount,
            DiscountType.Percent => total - (total * Discount / 100),
            _ => total,
        };
    }
    public decimal CountingDiscount()
    {
        var total = Details.Sum(s => s.CountingPrince((int)DiscountType));
        var tot = DiscountType switch
        {
            DiscountType.Static => total - Discount,
            DiscountType.Percent => total - (total * Discount / 100),
            _ => total,
        };
        return total - tot;
    }
    public decimal CountingRealPrice()
    {
        // it's a sum items times quantity without discount,
        // then add any extra amounts.
        // for now, no extra amount.

        var total = Details.Sum(s => s.CountingPrince(-100));
        return total;
    }
    public (decimal total, decimal discountedTotal, decimal discount) Count()
    {
        // the invoice-discount, it must be counted on real-items-price or discounted-items-price ??
        //  for now i'll count it on => discounted-items-price for (lose less)

        //    Example:
        //      • Real - items - price(before any discount): $1,000
        //      • Item - level discount(10 % on total items): -$100
        //      • Discounted - items - price: $900
        //      • Invoice discount(5 % on what ?)
        //          • If on real - items - price: 5 % of $1,000 = $50 (Total discount: $100 + $50 = $150)
        //          • If on discounted - items - price: 5 % of $900 = $45 (Total discount: $100 + $45 = $145)
        //  By applying the invoice discount after the item discount, your total discount is smaller ($145 instead of $150), so you lose less money.

        var realTotal = Details.Sum(s => s.CountingPrince(-100));           //  $1,000
        var total = Details.Sum(s => s.CountingPrince((int)DiscountType));  //    $900
        var tot = DiscountType switch                                       //    $900 - $45 = $855
        {
            DiscountType.Static => total - Discount,
            DiscountType.Percent => total - (total * Discount / 100),
            _ => total,
        };
        var discount = realTotal - tot;                                     //  $1,000 - $855 = $145
        return (realTotal, tot, discount);
    }

}