using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class SalesInvoiceCreateRequest : CreateUpdateRequestBase
{
    public DateTime Date { get; set; }
    public long Number { get; set; }
    public long SerialNumber { get; set; }
    public bool IsPaid { get; set; }

    public PeymentType PeymentType { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; }

    public int DiscountType { get; set; }
    public decimal Discount { get; set; }

    public int TaxType { get; set; }
    public decimal Tax { get; set; }
    
    public Identifier? ClientId { get; set; }
    public Identifier? StoreId { get; set; }
    public StorageType StorageType { get; set; }
    public Identifier? MarketerStorageId { get; set; }
    public Identifier? SafeId { get; set; }
    
    public Identifier? BankId { get; set; }
    public Identifier? RepresentativeId { get; set; }

    public List<SalesInvoiceDetail> Details { get; set; } = [];

    public string? Notes { get; set; }


    public decimal CountingPrince(int mode)
    {
        var all = Details.ToList();
        var ggTotal = 0m;
        all.ForEach(f =>
        {
            var realPrice = f.Price * f.Quantity;
            if (mode == (int)Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType.Percent)
            {
                ggTotal += realPrice - (realPrice * (Discount / 100));
            }
            else if (mode == (int)Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType.Static)
            {
                ggTotal += realPrice - Discount;
            }
            else
            {
                ggTotal += realPrice;
            }
        });
        return ggTotal;
    }
}

public record SalesInvoiceDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, decimal Price, decimal Discount, DateTime? ExpireDate);



public class DeleteSalesInvoiceRequest : DeleteRequestBase
{

}

public class SalesInvoiceQueryRequest : QueryRequestBase
{
    public long Number { get; set; }
}

public class GetItemInfoQueryRequest : QueryRequestBase
{
    public Identifier StoreId { get; set; }
    public Identifier ItemId { get; set; }
    public Identifier UnitId { get; set; }
    public Identifier BomId { get; set; }
}

public class ItemInfoQueryResponse
{
    public decimal Price { get; set; }
    
    public decimal Quntity { get; set; }
    
    public decimal AdditionalQuntity { get; set; }
    
    public bool IsExpirable { get; set; }
    
    public decimal MinPrice { get; set; }
    
    public decimal BulkPrice { get; set; }
    
    public decimal RegionalPrice { get; set; }
}