using FluentValidation;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Maintenance.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.Validations;

public class MaintenanceInvoiceCreateUpdateRequestValidation : AbstractValidator<MaintenanceInvoiceCreateUpdateRequest>
{
    public MaintenanceInvoiceCreateUpdateRequestValidation(
    )
    {
        RuleFor(prp => prp)
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var xx = cntxt.InstanceToValidate;
                if (xx.ClientId.IsNullOrEmpty())
                {
                    cntxt.AddFailure("رجاءً قم بتحديد الزبون. أذا ادخلت أسم زبون غير موجود في النظام, يمكنك الضغط على 'أضافة زبون' لأضافة الزبون ألى النظام تلقائيًا."); // Localized
                }
                
                if (!xx.Details.Any())
                {
                    cntxt.AddFailure("يرجى أختيار عناصر الصيانة."); // Localized
                }
                
                if (xx.Discount < 0 || xx.Details.Any(w => w.Discount < 0))
                {
                    cntxt.AddFailure("قيمة التخفيض غير صحيحة"); // Localized
                }

                if (xx.Details.Any(w => w.UnitId.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يبدو أن أحد الأصناف التي تم اختيارها لم يتم تحديد الوحدة الخاصة به."); // Localized
                }

                if (xx.Details.Sum(s => s.Quantity * s.Price) < 0)
                {
                    cntxt.AddFailure("لا يمكن اتمام الشراء لإجمالي الفاتورة الخاصة بك اقل من 0"); // Localized
                }

                if (xx.PaymentType == PeymentType.Cash && (!xx.SafeId.HasValue || xx.SafeId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الخزينة عند اختيار نوع الدفع - نقدي"); // Localized
                }

                if (xx.PaymentType == PeymentType.Deferred && (!xx.ClientId.HasValue || xx.ClientId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الزبون عند اختيار نوع الدفع - آجل"); // Localized
                }
            });
    }
}