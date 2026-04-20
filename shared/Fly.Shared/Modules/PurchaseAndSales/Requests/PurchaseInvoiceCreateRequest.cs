using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class PurchaseInvoiceCreateRequest : CreateUpdateRequestBase
{

    public long SerialNumber { get; set; }
    public long Number { get; set; }
    public bool IsPaid { get; set; }

    public DateTime Date { get; set; }

    public int DiscountType { get; set; }
    public decimal Discount { get; set; }

    public Identifier? SupplierId { get; set; }
    public Identifier StoreId { get; set; }
    public Identifier? SafeId { get; set; }
    
    public Identifier? BankId { get; set; }

    public PeymentType PeymentType { get; set; }


    public List<PurchaseInvoiceCost> Costs { get; set; } = [];
    public List<PurchaseInvoiceDetail> Details { get; set; } = [];
    public string? Notes { get; set; }
    public bool IsCostOnSupplier { get; set; }
}


public record PurchaseInvoiceCost(string Name, decimal Ammount);


public record PurchaseInvoiceDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, decimal Price, decimal Discount, DateTime? ExpireDate);


public class PurchaseInvoiceQueryRequest : QueryRequestBase
{
    public int Number { get; set; }
}

//  
// STOPPED ON PURCHES INVOICE PROPERTIES