using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Accounting.Enums;

namespace Fly.Shared.Modules.Financial.Dtos;

public class DtoStatmentInfo
{
    public decimal Credit { get; set; }
    public decimal Debit { get; set; }

    public decimal Balance => Debit - Credit;

    public string Name { get; set; } = string.Empty;
    public Guid AccountId { get; set; }

}
public class DtoStatment
{

    public string FileName { get; set; } = string.Empty;
    public string Text64BaseString { get; set; } = string.Empty;

    public long Number { get; set; }

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;

    [ClientInvisible]
    public DateTime StartDate { get; set; }

    [ClientInvisible]
    public DateTime EndDate { get; set; }

    public string EntryDateString { get; set; }

    public decimal Credit { get; set; }
    public decimal Debit { get; set; }

    [ClientInvisible]
    public decimal Balance { get; set; }
    public string Notes { get; set; } = string.Empty;

    public long DisplayNumber { get; set; }

    [ClientInvisible]
    public OprationTypeEnum? OprationType { get; set; }

    public string OprationTypeString => OprationType switch
    {
        OprationTypeEnum.None => "بدون",
        OprationTypeEnum.Purchases => "فاتورة مشتريات",
        OprationTypeEnum.Sales => "فاتورة مبيعات",
        OprationTypeEnum.Marketer => "مسوق",
        OprationTypeEnum.AccountFinancial => "معاملة مع حساب",
        OprationTypeEnum.ClientFinancial => "معاملة مع حساب زبون",
        OprationTypeEnum.EmployeeFinancial => "معاملة مع حساب موظف",
        OprationTypeEnum.ExpenseFinancial => "معاملة مع حساب مصروف",
        OprationTypeEnum.SupplierFinancial => "معاملة مع حساب مورد",
        OprationTypeEnum.PurchasesCost => "تكاليف مشتريات",
        OprationTypeEnum.DiscountingFinancial => "تخفيض",
        OprationTypeEnum.DiscountingItemFinancial => "تخفيض بسبب صنف",
        OprationTypeEnum.Normal => "عادي",
        OprationTypeEnum.PurchasesCost_Return => "إرجاع تكاليف مشتريات",
        OprationTypeEnum.MarketerFinancial => "معاملة مع حساب مسوق",
        OprationTypeEnum.DiscountFinancial => "تخفيض",
        OprationTypeEnum.DiscountByItemFinancial => "تخفيض بسبب صنف",
        OprationTypeEnum.OpeningBalance => "رصيد افتتاحي",
        OprationTypeEnum.CountingTotal => "إجمالي احتساب",
        OprationTypeEnum.Sales_Return => "ارجاع مبيعات",
        OprationTypeEnum.RepresentativePaymentReceipt => "مقبوضات مندوب",
        OprationTypeEnum.SalaryConting => "احتساب المرتبات",
        OprationTypeEnum.Purchases_Return => "ارجاع مشتريات",
        OprationTypeEnum.Factoring => "فاتورة تصنيع",
        OprationTypeEnum.Maintenance => "فاتورة صيانة",
        _ => "غير معروف",
    };

    [ClientInvisible]
    public bool CollecttTheQuntitiesBeforTime { get; set; }


    [ClientInvisible]
    public string? RemainText { get; set; }

    [ClientInvisible]
    public decimal? RemainAmount { get; set; }
}

//public class DtoItemDiscountStatment : DtoStatment
//{

//    public DtoItemDiscountStatment(DtoDiscountingItem? item)
//    {
//        if (item is null) return;
//        this.ItemName = item.ItemName;
//        this.UnitName = item.UnitName;
//        this.Discount = item.Discount;
//        this.Qauntity = item.Qauntity;
//    }

//    public string UnitName { get; set; } = string.Empty;
//    public string ItemName { get; set; } = string.Empty;
//    public decimal Discount { get; set; }
//    public decimal Qauntity { get; set; }
//}

//public class DtoDiscountingItem : DtoBase
//{

//    public Identifier UnitId { get; set; }
//    public Identifier ItemId { get; set; }
//    public Identifier RelatedId { get; set; }

//    public string UnitName { get; set; } = string.Empty;
//    public string ItemName { get; set; } = string.Empty;
//    public decimal Discount { get; set; }
//    public decimal Qauntity { get; set; }
//}

public class DtoDiscountitedItem : DtoBase
{
    public DateTime Date { get; set; }
    public DateTime ExpireDate { get; set; }

    public long Number { get; set; }
    public decimal Ammount { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalQuantity { get; set; }

    public Identifier ClientId { get; set; }
    public string? ClientName { get; set; }

    public Identifier ItemId { get; set; }
    public string? ItemName { get; set; }
    public int? ItemNumber { get; set; }

    public Identifier UnitId { get; set; }
    public string? UnitName { get; set; }


    public Identifier ReleatedId { get; set; }
}