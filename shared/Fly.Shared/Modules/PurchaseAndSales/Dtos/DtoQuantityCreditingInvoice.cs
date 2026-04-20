using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Framework.Shared.Common.Attributes;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos;

public class DtoQuantityCreditingInvoice : DtoBase
{
    public long SerialNumber { get; set; }
    public long Number { get; set; }

    [ClientInvisible]
    public DateTime Date { get; set; }

    public string TimwFormated => Date.ToString("HH:mm") ?? "";
    public string DateFormated => Date.ToString("yyyy.MM.dd") ?? "";



    [ClientInvisible]
    public string? ClientPhone { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier? RepresentativeId { get; set; }
    public string? RepresentativeName { get; set; } = string.Empty;
    public decimal RepresentativePercentage { get; set; }

    [ClientInvisible]
    public decimal RepresentativePhone { get; set; }

    [ClientInvisible]
    public Identifier? StoreId { get; set; }

    [ClientInvisible]
    public Identifier? ImportItemId { get; set; }
   
    [ClientInvisible]
    public Identifier? ImportUnitId { get; set; }
    
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

    [ClientInvisible]
    public decimal ImportQuantity { get; set; }
    
    [ClientInvisible]
    public decimal ImportPrice { get; set; }
    
    [ClientInvisible]
    public PeymentType PeymentType { get; set; }
    public string PeymentTypeString => this.PeymentType switch
    {
        PeymentType.Cash => "نقدا",
        PeymentType.Deferred => "آجل",
        _ => ""
    };


    [ClientInvisible]
    public virtual ICollection<DtoQuantityCreditingInvoiceDetail> Details { get; set; } = [];

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

}
