using FluentValidation;
using Fly.Core.Modules.Factoring.ManufacturingFacilities.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Requests;
using LinqKit;

namespace Fly.Core.Modules.Factoring.ManufacturingFacilities.Validations;

public class ManufacturingFacilityCreateUpdateRequestValidation : AbstractValidator<ManufacturingFacilityCreateUpdateRequest>
{
    public ManufacturingFacilityCreateUpdateRequestValidation(IServiceBase<DtoManufacturingFacility, TManufacturingFacility> serviceBase)
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
                        cnxt.AddFailure("رقم الهاتف مستخدم قبل جهة تصنيع آخرى"); // Localization
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
                        cnxt.AddFailure("البريد مستخدم من قبل جهة تصنيع آخرى"); // Localization
                    }
                }
            });
    }
}