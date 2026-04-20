using FluentValidation;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;

namespace Fly.Core.Modules.F.Marketers.Validations;

public class MarketerSalesInvoiceCreateRequestValidation : AbstractValidator<MarketerSalesInvoiceCreateRequest>
{
    public MarketerSalesInvoiceCreateRequestValidation(IServiceBase<DtoMarketer, TMarketer> serviceBase)
    {
        RuleFor(prp => prp.Id)
            .CustomAsync(async (id, cnxt, cancellationToken) =>
            {
                var saleInvoice = cnxt.InstanceToValidate;
                var isNew = saleInvoice.Id.IsNullOrEmpty();

                var clientSelected = !saleInvoice.ClientId.IsNullOrEmpty();
                if (!clientSelected)
                {
                    cnxt.AddFailure("رجاء قم بتحديد العميل"); // TODO: localize
                }
                if (saleInvoice.PeymentType
                    != Shared.Modules.PurchaseAndSales.Enums.PeymentType.Deferred
                    & saleInvoice.PeymentType
                    != Shared.Modules.PurchaseAndSales.Enums.PeymentType.Cash)
                {
                    cnxt.AddFailure("الرجاء تحديد طريقة الدفع"); // TODO: localize
                }

                //var totalSum = saleInvoice.Details.Sum(w => w.Quantity * (w.Price - w.Discount));
                //if (totalSum <= 0)
                //{
                //    cnxt.AddFailure("اجمالي سعر الفاتورة لا يبدو منطقيا"); // TODO: localize
                //}

                await Task.CompletedTask;
            });
    }
}