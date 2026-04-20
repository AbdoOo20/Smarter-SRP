using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Framework.Shared.Common.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

public partial class DtoSalesInvoice : DtoBase
{
    [ClientInvisible]
    public long SerialNumber { get; set; }
    public long Number { get; set; }

    [ClientInvisible, ObservableProperty]
    public partial DateTime Date { get; set; }

    public string TimwFormated => Date.ToString("HH:mm") ?? "";
    public string DateFormated => Date.ToString("yyyy.MM.dd") ?? "";

    [ClientInvisible]
    public bool IsPaid { get; set; }

    [ClientInvisible]
    public Identifier? ClientId { get; set; }
    public string? ClientName { get; set; } = string.Empty;

    public string? ClientPhone { get; set; } = string.Empty;

    public string? ClientAddress { get; set; } = string.Empty;

    [ClientInvisible]
    public string? ClientEmail { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier? RepresentativeId { get; set; }

    public string? RepresentativeName { get; set; } = string.Empty;

    [ClientInvisible]
    public decimal RepresentativePercentage { get; set; }

    [ClientInvisible]
    public decimal RepresentativePhone { get; set; }

    [ClientInvisible]
    public Identifier? StoreId { get; set; }

    [ClientInvisible]
    public StorageType StorageType { get; set; }

    [ClientInvisible]
    public Identifier? MarketerStorageId { get; set; }
    [ClientInvisible]
    public string? MarketerStorageName { get; set; }

    [ClientInvisible]
    public string? StoreName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier? SafeId { get; set; }

    [ClientInvisible]
    public Identifier? BankId { get; set; }
    
    [ClientInvisible]
    public string? BankName { get; set; }
    
    [ClientInvisible]
    public string? SafeName { get; set; }

    public string Storage
    {
        get
        {
            if (StorageType == StorageType.Normal)
            {
                return StoreName ?? "";
            }
            else
            {
                return MarketerStorageName ?? "";
            }
        }
    }

    [ClientInvisible]
    public DiscountType DiscountType { get; set; }

    [ClientInvisible]
    public decimal Discount { get; set; }

    [ClientInvisible]
    public TaxType TaxType { get; set; }

    [ClientInvisible]
    public decimal Tax { get; set; }
    
    public PeymentType PeymentType { get; set; }

    public InvoiceStatus InvoiceStatus { get; set; }
    
    [ClientInvisible]
    public virtual ICollection<DtoSalesInvoiceDetail> Details { get; set; } = [];

    [ClientInvisible]
    public string DetailsString
    {
        get
        {
            var def = "";
            return def;

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


    /// <summary>
    /// the total amount after discount
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// the total amount without discount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// the total discount amount = invoice discount + items discounts
    /// </summary>
    public decimal DiscountAmount { get; set; }
    public string DiscountInvoiceString
    {
        get
        {
            var tot = DiscountType switch
            {
                DiscountType.Percent => "%",
                _ => "",
            };
            return $"{Discount}{tot}";
        }
    }

    [NotMapped, ClientInvisible]
    public IReadOnlyCollection<DtoSellReturnInvoiceDetail> ReturnInvoices { get; set; } = [];

    private decimal CountTax(decimal total)
    {
        if (Tax == 0)
            return total;

        return TaxType switch
        {
            TaxType.Value => total + Tax,
            TaxType.Percent => total + (total * Tax / 100),
            _ => total
        };
    }

    public (decimal total, decimal discountedTotal, decimal discount) Count()
    {
        var realTotal = Details.Sum(s => s.CountItemPrice(false));           //  $1,000
        var total = Details.Sum(s => s.CountItemPrice(true));  //    $900
        var tot = DiscountType switch
        {
            DiscountType.Static => total - Discount,
            DiscountType.Percent => total - (total * Discount / 100),
            _ => total,
        };
        var discount = realTotal - tot;                                     //  $1,000 - $855 = $145
        return (realTotal, CountTax(tot), discount);
    }
}
