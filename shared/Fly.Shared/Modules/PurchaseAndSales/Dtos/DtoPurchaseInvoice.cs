using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos;

public partial class DtoPurchaseInvoice : DtoBase
{
    public long SerialNumber { get; set; }
    public long Number { get; set; }
    
    [ClientInvisible]
    public bool IsPaid { get; set; }

    public decimal TotalCost { get; set; }
    public decimal TotalAmount { get; set; }

    [ClientInvisible]
    [ObservableProperty]
    public partial DateTime Date { get; set; }

    public string TimwFormated => Date.ToString("HH:mm") ?? "";
    public string DateFormated => Date.ToString("yyyy.MM.dd") ?? "";

    [ClientInvisible]
    public decimal Discount { get; set; }
    [ClientInvisible]
    public DiscountType DiscountType { get; set; }

    [ClientInvisible]
    public Identifier? SupplierId { get; set; }
    public string? SupplierName { get; set; } = string.Empty;
    public string? SupplierPhone { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier? SafeId { get; set; }
    public string? SafeName { get; set; }

    [ClientInvisible]
    public Identifier? BankId { get; set; }
    
    public string? BankName { get; set; }
    
    public PeymentType PeymentType { get; set; }

    [ClientInvisible]
    public virtual List<DtoPurchaseInvoiceCost> Costs { get; set; } = [];
    
    [ClientInvisible]
    public virtual List<DtoPurchaseInvoiceDetail> Details { get; set; } = [];

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
    public bool IsCostOnSupplier { get; set; }


    public decimal CountingRealPrice()
    {
        var total = Details.Sum(s => s.CountingPrince(-100));
        return total;
    }
    public decimal CountingPrice()
    {
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
    public (decimal total, decimal discountedTotal, decimal discount) Count()
    {
        var realTotal = Details.Sum(s => s.CountingPrince(-100));
        var total = Details.Sum(s => s.CountingPrince((int)DiscountType));
        var tot = DiscountType switch
        {
            DiscountType.Static => total - Discount,
            DiscountType.Percent => total - (total * Discount / 100),
            _ => total,
        };
        return (total, tot, total - tot);
    }
}