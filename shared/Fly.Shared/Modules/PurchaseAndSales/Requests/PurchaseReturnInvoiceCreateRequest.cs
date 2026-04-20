using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class PurchaseReturnInvoiceCreateRequest : CreateUpdateRequestBase
{

    public long? InvoiceNumber { get; set; }

    public DateTime Date { get; set; }
    public long Number { get; set; }
    public long SerialNumber { get; set; }
    public bool IsPaid { get; set; }
    public bool IsPartial { get; set; }


    public int DiscountType { get; set; }
    public decimal Discount { get; set; }


    public Identifier? StoreId { get; set; }
    public Identifier? SupplierId { get; set; }
    public Identifier? SafeId { get; set; }
    public PeymentType PeymentType { get; set; }

    public IEnumerable<PurchaseReturnInvoiceDetail> Details { get; set; } = [];

    public string? Notes { get; set; }

}

public record PurchaseReturnInvoiceDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, decimal Price, decimal Discount, DateTime? ExpireDate);



public class DeletePurchaseReturnInvoiceRequest : DeleteRequestBase
{

}

public class PurchaseReturnInvoiceQueryRequest : QueryRequestBase
{
    public long Number { get; set; }
}