using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoInventory : DtoBase
{
    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal ItemPrice { get; set; }

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;

    public DateTime? OprationDate { get; set; }

    public DateTime? ExpireDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal AdditionalQuantity { get; set; }
    public bool Ingress { get; set; }

    public Identifier? RelatedId { get; set; }
    public RelatedInventory RelatedIndex { get; set; }

}