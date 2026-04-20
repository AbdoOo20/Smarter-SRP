using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
public partial class DtoPurchaseReturnInvoice : DtoBase
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
    public Identifier? SupplierId { get; set; }
    public string? SupplierName { get; set; } = string.Empty;

    [ClientInvisible]
    public string? SupplierPhone { get; set; } = string.Empty;


    [ClientInvisible]
    public Identifier? InvoiceId { get; set; }
    public long? InvoiceNumber { get; set; }


    [ClientInvisible]
    public Identifier? StoreId { get; set; }

    [ClientInvisible]
    public StorageType StorageType { get; set; }

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
    public virtual List<DtoPurchaseReturnInvoiceDetail> Details { get; set; } = [];

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
    public DtoPurchaseInvoice? PurchaseInvoice { get; set; } = null;


    public decimal NetAmount { get; set; }
    public decimal DiscountAmount { get; set; }
}
