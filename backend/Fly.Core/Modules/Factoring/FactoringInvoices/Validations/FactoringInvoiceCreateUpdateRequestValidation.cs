using FluentValidation;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Factoring.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Validations;

public class FactoringInvoiceCreateUpdateRequestValidation : AbstractValidator<FactoringInvoiceCreateUpdateRequest>
{
    public FactoringInvoiceCreateUpdateRequestValidation()
    {
        RuleFor(prp => prp)
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var model = cntxt.InstanceToValidate;
                if (model.ClientId.IsNullOrEmpty())
                {
                    cntxt.AddFailure("رجاءً قم بتحديد الزبون. أذا ادخلت أسم عميل غير موجود في النظام, يمكنك الضغط على 'أضافة زبون' لأضافة الزبون ألى النظام تلقائيًا."); // Localized
                }
                
                if (!model.Details.Any())
                {
                    cntxt.AddFailure("يرجى أختيار عناصر التصنيع."); // Localized
                }
                
                if (model.Discount < 0 || model.Details.Any(w => w.Discount < 0))
                {
                    cntxt.AddFailure("قيمة التخفيض غير صحيحة"); // Localized
                }

                if (model.Details.Any(w => w.UnitId.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يبدو أن أحد الأصناف التي تم اختيارها لم يتم تحديد الوحدة الخاصة به."); // Localized
                }

                if (model.Details.Sum(s => s.Quantity * s.Price) < 0)
                {
                    cntxt.AddFailure("لا يمكن اتمام التصنيع لإجمالي الفاتورة الخاصة بك اقل من 0"); // Localized
                }

                if (model.PaymentType == PeymentType.Cash && (!model.SafeId.HasValue || model.SafeId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الخزينة عند اختيار نوع الدفع - نقدي"); // Localized
                }

                if (model.PaymentType == PeymentType.Deferred && (!model.ClientId.HasValue || model.ClientId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الزبون عند اختيار نوع الدفع - آجل"); // Localized
                }
            });
    }
}