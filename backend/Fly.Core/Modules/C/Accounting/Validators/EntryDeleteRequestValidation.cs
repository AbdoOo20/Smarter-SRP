using FluentValidation;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Core.Modules.C.Accounting.Validators;

public class EntryDeleteRequestValidation : AbstractValidator<EntryDeleteRequest>
{
    public EntryDeleteRequestValidation(IServiceBase<DtoEntry, TEntry> service)
    {
        RuleFor(prp => prp.Id)
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var isNEw = cntxt.InstanceToValidate.Id.IsNullOrEmpty();
                if (!isNEw)
                {
                    var ent = await service.GetAsync(w => w.Id == id);
                    if (ent is null)
                    {
                        cntxt.AddFailure("لم يتم العثور على القيد");
                        return;
                    }
                    var allowed = new List<OprationTypeEnum>() {
                        OprationTypeEnum.Normal,
                        OprationTypeEnum.AccountFinancial,
                        OprationTypeEnum.ClientFinancial,
                        OprationTypeEnum.EmployeeFinancial,
                        OprationTypeEnum.ExpenseFinancial,
                        OprationTypeEnum.SupplierFinancial,
                        OprationTypeEnum.MarketerFinancial,
                        OprationTypeEnum.DiscountFinancial,
                        OprationTypeEnum.DiscountingFinancial,
                        OprationTypeEnum.DiscountByItemFinancial,
                        OprationTypeEnum.DiscountingItemFinancial,
                        OprationTypeEnum.RepresentativePaymentReceipt,
                    };
                    if (/*ent.OprationType is not null &&*/ !allowed.Contains(ent.OprationType.Value) /*&& !cntxt.InstanceToValidate.IgnoreType*/)
                    {
                        // cntxt.AddFailure("تم إنشاء هذا القيد بطريقة تلقائية، في حالة كنت تتحمل مسؤولية تعديل هذا القيد اضغط على خانة تجاهل منع التعديل على القيود التلقائية");
                        cntxt.AddFailure("تم إنشاء هذا القيد بطريقة تلقائية، لا يمكن حذفه إلا من خلال المعاملة التي أنشئ من خلالها");
                    }
                }
            })
            ;
    }
}