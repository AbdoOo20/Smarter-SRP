using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos;

public class DtoPurchaseInvoiceCost : DtoBase, IDetailsModel
{
    [ClientInvisible]
    public Identifier PurchaseInvoiceId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public decimal Ammount { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}