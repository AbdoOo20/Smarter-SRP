using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.QuantityCreditorModels;

public class DtoQuantityCreditingDetailsModel: DtoQuantityCreditingInvoiceDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    [ClientInvisible]
    public Identifier? TemporaryStoreId { get; set; }

    public string CalculatedPrice => (Price * Quantity).ToString("N1");
    
    public bool CanSendQueryRequest()
    {
        return TemporaryStoreId != Identifier.Empty
               && ItemId != Identifier.Empty
               && UnitId != Identifier.Empty;
    }
}