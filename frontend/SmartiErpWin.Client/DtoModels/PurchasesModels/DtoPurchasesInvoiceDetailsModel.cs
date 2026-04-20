using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace SmartiErpWin.Client.DtoModels.PurchasesModels;

public class DtoPurchasesInvoiceDetailsModel: DtoPurchaseInvoiceDetail, IModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}