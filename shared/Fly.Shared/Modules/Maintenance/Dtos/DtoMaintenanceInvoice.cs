using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Maintenance.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Maintenance.Dtos;

public partial class DtoMaintenanceInvoice : DtoBase
{
    [ObservableProperty]
    public partial DateTime Date { get; set; }
    
    [ObservableProperty]
    public partial DateTime EndDate { get; set; }

    public decimal TotalAmount => Count().total;
    
    public long Number { get; set; }

    public bool IsPaid => MaintenanceStatus is MaintenanceStatus.FullyPaid;

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
    public MaintenanceStatus MaintenanceStatus { get; set; }
    
    public MaintenanceItemsStatus MaintenanceItemsStatus { get; set; }

    public bool ReceivedFeeInAdvance { get; set; }
    
    public string? MaintenanceSectorName
    {
        get
        {
            if (IsInternal)
                return "جهة صيانة داخلية";
            return field;
        }
        set => field = value;
    }

    [DisplayName("UpfrontClientCost")]
    public decimal CollectionRate { get; set; } 
   
    public Identifier? MaintenanceSectorId { get; set; }
    
    public decimal Discount { get; set; }
    
    public DiscountType DiscountType { get; set; }
    
    public PeymentType PaymentType { get; set; }
    
    public string? Notes { get; set; }

    [ClientInvisible]
    public virtual ICollection<DtoMaintenanceInvoiceDetail> Details { get; set; } = [];
    
    [ClientInvisible]
    public virtual ICollection<DtoMaintenanceInvoiceCost> Costs { get; set; } = [];
    
    public (decimal total, decimal discountedTotal, decimal discount) Count()
    {
        var realTotal = Details.Sum(s => s.CountPrice(-100)) + Costs.Sum(c => c.Price);           //  $1,000
        var total = Details.Sum(s => s.CountPrice((int)DiscountType));  //    $900
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