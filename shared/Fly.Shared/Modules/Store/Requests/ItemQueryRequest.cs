using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemQueryRequest : QueryRequestBase
{
    public Identifier? ItemId { get; set; }
}

public class ItemQueryResponse
{
    public byte[] ItemImage { get; set; }
}

public class ChangePriceRequest
{
    public Identifier ItemId { get; set; }
    public decimal Price { get; set; }
}
public class ExpireDateQueryRequest : QueryRequestBase
{
    public Identifier ItemId { get; set; }
    public Identifier StoreId { get; set; }
    public StorageType StorageType { get; set; }
    public Identifier? LinkedFile { get; set; }
}