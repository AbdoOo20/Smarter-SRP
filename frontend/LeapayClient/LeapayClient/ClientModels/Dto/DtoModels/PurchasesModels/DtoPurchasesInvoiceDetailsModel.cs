using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.PurchasesModels;

public class DtoPurchasesInvoiceDetailsModel: DtoPurchaseInvoiceDetail, IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}