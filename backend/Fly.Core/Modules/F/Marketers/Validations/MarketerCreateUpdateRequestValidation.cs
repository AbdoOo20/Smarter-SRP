using FluentValidation;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;
using LinqKit;

namespace Fly.Core.Modules.F.Marketers.Validations;

public class MarketerCreateUpdateRequestValidation : AbstractValidator<MarketerCreateUpdateRequest>
{
    public MarketerCreateUpdateRequestValidation(IServiceBase<DtoMarketer, TMarketer> serviceBase)
    {

        RuleFor(r => r.Phone)
            .NotNull().WithMessage("لا يمكن ترك رقم الهاتف فارغا") // localize
            .NotEmpty().WithMessage("لا يمكن ترك رقم الهاتف فارغا") // localize
            .CustomAsync(async (phone, cnxt, cancellationToken) =>
            {
                var isNew = cnxt.InstanceToValidate.Id == Identifier.Empty;
                if (!string.IsNullOrEmpty(phone))
                {
                    var buldr = serviceBase.Build();
                    buldr = buldr.And(w => w.Phone == phone);
                    if (!isNew)
                    {
                        buldr = buldr.And(w => w.Id != cnxt.InstanceToValidate.Id);
                    }

                    var exsits = await serviceBase.AnyAsync(buldr);
                    if (exsits)
                    {
                        cnxt.AddFailure("رقم الهاتف مستخدم من قبل مسوق آخر"); // Localization
                    }
                }
            });

        RuleFor(r => r.Email)
            .CustomAsync(async (email, cnxt, cancellationToken) =>
            {
                var isNew = cnxt.InstanceToValidate.Id == Identifier.Empty;
                if (!string.IsNullOrEmpty(email))
                {
                    var buldr = serviceBase.Build();
                    buldr = buldr.And(w => w.Email == email);
                    if (!isNew)
                    {
                        buldr = buldr.And(w => w.Id != cnxt.InstanceToValidate.Id);
                    }

                    var exsits = await serviceBase.AnyAsync(buldr);
                    if (exsits)
                    {
                        cnxt.AddFailure("البريد مستخدم من قبل مسوق آخر"); // Localization
                    }
                }
            });


    }
}