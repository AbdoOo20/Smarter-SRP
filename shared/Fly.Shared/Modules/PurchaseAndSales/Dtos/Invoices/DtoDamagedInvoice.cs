using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Invoices;

public class DtoDamagedInvoice : DtoBase
{
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public long SerialNumber { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string? StoreName { get; set; }
    public Identifier? StoreId { get; set; }

    public string? MarketerStoreName { get; set; }
    public Identifier? MarketerStoreId { get; set; }

    public virtual ICollection<DtoDamagedInvoiceDetail> Details { get; set; } = [];

    public string? Notes { get; set; }
}

public class DtoDamagedInvoiceDetail : DtoBase
{
    public Identifier DamagedInvoiceId { get; set; }

    public string? UnitName { get; set; }
    public Identifier UnitId { get; set; }

    public string? ItemName { get; set; }
    public Identifier ItemId { get; set; }

    public decimal Quantity { get; set; }

    public DateTime? ExpireDate { get; set; }
}
