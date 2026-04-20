using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using System.Text;

namespace Fly.Shared.Modules.Accounting.Dtos;

public class DtoEntry : DtoBase
{
    public long Number { get; set; }
    public long SerialNumber { get; set; }

    [ClientInvisible]
    public long DisplayNumber { get; set; }


    public string TimwFormated => Date.ToString("HH:mm:sss");
    public string DateFormated => Date.ToString("yyyy.MM.dd");

    [ClientInvisible]
    public DateTime Date { get; set; }

    [ClientInvisible]
    public List<DtoEntryDetail>? Details { get; set; }

    [ClientInvisible]
    public OprationTypeEnum? OprationType { get; set; }

    [ClientInvisible]
    public int? RelatedIndex { get; set; }

    [ClientInvisible]
    public Identifier? RelatedId { get; set; }
    
    public string OprationTypeString => OprationType switch
    {
        OprationTypeEnum.None => "قيد عادي",
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
        OprationTypeEnum.MarketerFinancial => "قبض مسوق",
        OprationTypeEnum.DiscountFinancial => "سند تخفيض",
        OprationTypeEnum.DiscountByItemFinancial => "سند تخفيض على صنف",
        OprationTypeEnum.Normal => "قيد عادي",
        OprationTypeEnum.Maintenance => "أعمال صيانة",
        OprationTypeEnum.Factoring => "أعمال تصنيع",
        OprationTypeEnum.Sales_Return => "ارجاع مبيعات",
        OprationTypeEnum.CountingTotal => "",
        OprationTypeEnum.OpeningBalance => "رصيد افتتاحي",
        OprationTypeEnum.PurchasesCost_Return => "ارجاع مشتريات",
        OprationTypeEnum.RepresentativePaymentReceipt => "معاملة مندوب عن عميل",
        OprationTypeEnum.Purchases_Return => "ارجاع مشتريات",
        OprationTypeEnum.SalaryConting => "احتساب رواتب",
        //null => throw new NotImplementedException(),
        _ => "غير معروف",
    };
    
    public string DetailsString
    {
        get
        {
            if (Details is null || !Details.Any()) return "";

            var deptors = Details.Where(w => w.Credit == 0);
            var credetors = Details.Where(w => w.Debit == 0);

            var text = new StringBuilder();

            if (deptors.Any())
            {
                foreach (var item in deptors)
                {
                    text.AppendLine("\t" + item.AccountName);
                }
            }
            if (credetors.Any())
            {
                foreach (var item in credetors)
                {
                    text.AppendLine("\t" + item.AccountName);
                }
            }

            return text.ToString();
        }
    }

    public string DetailsString2
    {
        get
        {
            if (Details is null || !Details.Any()) return "";

            var deptors = Details.Where(w => w.Credit == 0);
            var credetors = Details.Where(w => w.Debit == 0);

            var text = new StringBuilder();

            if (deptors.Any())
            {
                text.AppendLine("");
                foreach (var item in deptors)
                {
                    text.AppendLine("" + item.Debit.ToString());
                }
            }
            if (credetors.Any())
            {
                text.AppendLine("");
                foreach (var item in credetors)
                {
                    text.AppendLine("" + item.Credit.ToString());
                }
            }

            return text.ToString();
        }
    }

    public string? Note { get; set; }
    
    [ClientInvisible]
    public TagTypeEnum? Tag { get; set; }

}