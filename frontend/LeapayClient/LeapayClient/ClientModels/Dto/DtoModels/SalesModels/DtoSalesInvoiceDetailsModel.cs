using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace Leapay.Client.ClientModels.Dto.DtoModels.SalesModels;

public class DtoSalesInvoiceDetailsModel: DtoSalesInvoiceDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    [ClientInvisible]
    public Identifier? TemporaryStoreId { get; set; }

    [ClientInvisible]
    public string? StoreName { get; set; }
    
    [ClientInvisible]
    public string CalculatedPrice => ((Price - Discount) * Quantity).ToString("N1");
    
    [ClientInvisible]
    public bool CanSendQueryRequest()
    {
        return /*TemporaryStoreId != Identifier.Empty && */
               ItemId != Identifier.Empty
               && UnitId != Identifier.Empty;
    }
}

public class DtoSellReturnInvoiceDetailModel : DtoSellReturnInvoiceDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    [ClientInvisible]
    public Identifier? TemporaryStoreId { get; set; }

    public string CalculatedPrice => ((Price - Discount) * Quantity).ToString("N1");

    [ClientInvisible]
    public DateTime? ExpireDate { get; set; }

    public string? ExpireDateString => ExpireDate?.ToDottedDate() ?? "";


    [ClientInvisible]
    public bool CanSendQueryRequest()
    {
        return /*TemporaryStoreId != Identifier.Empty && */
               ItemId != Identifier.Empty
               && UnitId != Identifier.Empty;
    }
}
public class DtoPurchaseReturnlInvoiceDetailModel : DtoPurchaseReturnInvoiceDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    [ClientInvisible]
    public Identifier? TemporaryStoreId { get; set; }

    [ClientInvisible]
    public string CalculatedPrice => ((Price - Discount) * Quantity).ToString("N1");

    [ClientInvisible]
    public DateTime? ExpireDate { get; set; }

    public string? ExpireDateString => ExpireDate?.ToDottedDate() ?? "";


    [ClientInvisible]
    public bool CanSendQueryRequest()
    {
        return /*TemporaryStoreId != Identifier.Empty && */
               ItemId != Identifier.Empty
               && UnitId != Identifier.Empty;
    }
}