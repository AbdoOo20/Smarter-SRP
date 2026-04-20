using FluentValidation;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
namespace Fly.Core.Modules.F.PurchaseAndSales.Validators;
//  PurchaseInvoiceCreateRequest

public class PurchaseInvoiceCreateRequestValidation : AbstractValidator<PurchaseInvoiceCreateRequest>
{
    public PurchaseInvoiceCreateRequestValidation(
        IServiceBase<DtoPurchaseInvoice, TPurchaseInvoice> service
        , IServiceBase<DtoPurchaseReturnInvoice, TPurchaseReturnInvoice> returnService)
    {
        RuleFor(prp => prp.Id)
            //.NotEmpty()
            //.NotNull()
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var isNew = id.IsNullOrEmpty();
                var xx = cntxt.InstanceToValidate;
                if (xx.SupplierId.IsNullOrEmpty() & xx.PeymentType == PeymentType.Deferred)
                {
                    cntxt.AddFailure("رجاءً قم بتحديد المورد"); // Localized
                }
                if (xx.Details.Sum(s => s.Quantity * s.Price) <= 0)
                {
                    cntxt.AddFailure("لا يمكن اتمام الشراء لإجمالي الفاتورة الخاصة بك اقل من 0"); // Localized
                }
                if (xx.StoreId.IsNullOrEmpty())
                {
                    cntxt.AddFailure("رجاءً قم بتحديد المخزن"); // Localized
                }
                if (xx.PeymentType == PeymentType.Cash && (!xx.SafeId.HasValue || xx.SafeId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الخزينة عند تحديد نوع الدفع - نقدي"); // Localized
                }
                if (!id.IsNullOrEmpty() && await returnService.AnyAsync(w => w.InvoiceId == id))
                {
                    cntxt.AddFailure("لا يمكن تعديل فاتورة لها مرتجع"); // Localized
                }
            });
    }
}