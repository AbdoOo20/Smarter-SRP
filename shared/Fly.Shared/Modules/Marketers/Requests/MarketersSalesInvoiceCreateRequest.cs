using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Marketers.Requests;

public class MarketerSalesInvoiceCreateRequest : CreateUpdateRequestBase
{
    public DateTime Date { get; set; }
    public long Number { get; set; }
    public bool IsPaid { get; set; }

    public PeymentType PeymentType { get; set; }


    public int DiscountType { get; set; }
    public decimal Discount { get; set; }

    public Identifier? ClientId { get; set; }
    public Identifier? MarketerId { get; set; }

    public ICollection<MarketerSalesInvoiceDetail> Details { get; set; } = [];
}

public record MarketerSalesInvoiceDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, decimal Price, decimal Discount, DateTime? ExpireDate);



public class MarketerDeleteSalesInvoiceRequest : DeleteRequestBase
{

}

public class MarketerSalesInvoiceQueryRequest : QueryRequestBase
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public long Number { get; set; }
}


public class MarketerDisbursementPermitCreateUpdateRequest : CreateUpdateRequestBase
{
    public DateTime Date { get; set; }
    public long Number { get; set; }
    public long SerialNumber { get; set; }
    public bool IsPaid { get; set; }

    public PeymentType PeymentType { get; set; }



    public int DiscountType { get; set; }
    public decimal Discount { get; set; }
    public Identifier? MarketerId { get; set; }
    public Identifier StoreId { get; set; }
    public Identifier? ClientId { get; set; }

    public IEnumerable<MarketerSalesInvoiceDetail> Details { get; set; } = [];
    public string? Notes { get; set; }
}

public record MarketerDisbursementPermitDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, decimal Price, decimal Discount);



public class MarketerFailingPermitCreateUpdateRequest : CreateUpdateRequestBase
{
    public DateTime Date { get; set; }
    public long Number { get; set; }
    public long SerialNumber { get; set; }
    public Identifier? MarketerId { get; set; }

    public ICollection<MarketerFailingPermitDetail> Details { get; set; } = [];
    public string? Notes { get; set; }
}
public record MarketerFailingPermitDetail(Identifier UnitId, Identifier ItemId, decimal Quantity, DateTime? ExpireDate);



public class MarketerDisbursementPermitDeleteRequestBase : DeleteRequestBase
{

}

public class MarketerDisbursementPermitQueryRequest : QueryRequestBase
{

}



//public class GetItemInfoQueryRequest : QueryRequestBase
//{
//    public Identifier StoreId { get; set; }
//    public Identifier ItemId { get; set; }
//    public Identifier UnitId { get; set; }
//    public Identifier BomId { get; set; }
//}

//public class ItemInfoQueryResponse
//{
//    public decimal Price { get; set; }
//    public decimal Quntity { get; set; }
//    public decimal AdditionalQuntity { get; set; }
//}