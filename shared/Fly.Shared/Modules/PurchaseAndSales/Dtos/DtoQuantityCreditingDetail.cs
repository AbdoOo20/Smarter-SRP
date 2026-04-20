using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos;

public class DtoQuantityCreditingInvoiceDetail : DtoBase, IDetailsModel
{
    [ClientInvisible]
    public Identifier QuantityCreditingInvoiceId { get; set; }

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemBarcode { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public DateTime? ExpireDate { get; set; }

    [ClientInvisible]
    public decimal TotalAmount => Price * Quantity;
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}