using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Factoring.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Factoring.Dtos;

public partial class DtoFactoringInvoice : DtoBase
{
    [ObservableProperty]
    public partial DateTime Date { get; set; }
    
    [ObservableProperty]
    public partial DateTime EndDate { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public long Number { get; set; }
    
    public bool IsPaid { get; set; }

    public bool IsInternal { get; set; } = true;

    public string? ClientName { get; set; }
    
    public Identifier? ClientId { get; set; }
    
    public string? StoreName { get; set; }
    
    public Identifier? StoreId { get; set; }
    
    public string? SafeName { get; set; }
    
    public Identifier? SafeId { get; set; }
    
    public string? BankName { get; set; }
    
    public Identifier? BankId { get; set; }
    
    [DisplayName("PaymentStatus")]
    public FactoringStatus FactoringStatus { get; set; }
    
    public FactoringItemsStatus FactoringItemsStatus { get; set; }

    public string? ManufacturingFacilityName { get; set; } = string.Empty;
    
    [DisplayName("UpfrontClientCost")]
    public decimal CollectionRate { get; set; } 
   
    public Identifier? ManufacturingFacilityId { get; set; }
    
    public string? Notes { get; set; }

    public decimal Discount { get; set; }
    
    public DiscountType DiscountType { get; set; }
    
    public PeymentType PaymentType { get; set; }
    
    public bool ReceivedFeeInAdvance { get; set; }

    [ClientInvisible]
    public virtual ICollection<DtoFactoringInvoiceDetail> Details { get; set; } = [];
    
    [ClientInvisible]
    public virtual ICollection<DtoFactoringInvoiceCost> Costs { get; set; } = [];
    
    [ClientInvisible]
    public virtual ICollection<DtoFactoringInvoiceComposition> Compositions { get; set; } = [];
    
    /// <summary>
    /// total = lines before discounts + extra costs.
    /// discountedTotal = what the customer pays: lines after their discounts, plus
    /// the extra costs, minus the invoice discount (taken on that whole amount).
    /// discount = total - discountedTotal.
    /// Extra costs used to be left out of the amount charged and counted as
    /// discount instead.
    /// </summary>
    public (decimal total, decimal discountedTotal, decimal discount) Count()
    {
        var costs = Costs.Sum(c => c.Price);
        var realTotal = Details.Sum(s => s.CountPrice(-100)) + costs;
        var beforeInvoiceDiscount = Details.Sum(s => s.CountPrice(0)) + costs;
        var net = DiscountType switch
        {
            DiscountType.Static => beforeInvoiceDiscount - Discount,
            DiscountType.Percent => beforeInvoiceDiscount - beforeInvoiceDiscount * Discount / 100,
            _ => beforeInvoiceDiscount,
        };
        net = Math.Round(net, 3);
        return (realTotal, net, realTotal - net);
    }

    /// <summary>Extra costs (المصاريف) of the invoice.</summary>
    public decimal CostsTotal => Costs.Sum(c => c.Price);
}
