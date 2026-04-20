using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.PurchaseAndQuantityCrediting.Requests;


public class QuantityCreditingInvoiceCreateRequest : CreateUpdateRequestBase
{


    public DateTime Date { get; set; }
    public long Number { get; set; }
    public long InvoiceNumber { get; set; }
    public long SerialNumber { get; set; }




    public int DiscountType { get; set; }
    public decimal Discount { get; set; }

    public Identifier? StoreId { get; set; }

    public ICollection<QuantityCreditingInvoiceDetail> Details { get; set; } = [];

    public string? Notes { get; set; }

}

public record QuantityCreditingInvoiceDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, decimal Price, decimal Discount, DateTime? ExpireDate);



public class DeleteQuantityCreditingInvoiceRequest : DeleteRequestBase
{

}

public class QuantityCreditingInvoiceQueryRequest : QueryRequestBase
{

}

