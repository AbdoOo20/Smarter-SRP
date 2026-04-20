using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;


//[MapTo]
//public class TDiscountingItem : TEntity
//{

//    public Identifier RelatedId { get; set; }

//    public TUnit? Unit { get; set; }
//    public Identifier? UnitId { get; set; }
//    public TItem? Item { get; set; }
//    public Identifier ItemId { get; set; }

//    public decimal Discount { get; set; }
//    public decimal Qauntity { get; set; }
//    public decimal TotalQuantity { get; set; }
//}

[MapTo]
public class TBank : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }

    public Identifier AccountId { get; set; }
    public TAccount? Account { get; set; }
}