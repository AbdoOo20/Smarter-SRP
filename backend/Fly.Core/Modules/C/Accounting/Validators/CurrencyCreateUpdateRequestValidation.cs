using FluentValidation;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using LinqKit;

namespace Fly.Core.Modules.C.Accounting.Validators;

public class CurrencyCreateUpdateRequestValidation : AbstractValidator<CreateUpdateCurrencyRequest>
{
    public CurrencyCreateUpdateRequestValidation(IServiceBase<DtoCurrency, TCurrency> service)
    {
        RuleFor(prp => prp.Name)
            .NotEmpty()
            .NotNull()
            .CustomAsync(async (name, cntxt, cans) =>
            {
                var id = cntxt.InstanceToValidate.Id;
                var isNEw = cntxt.InstanceToValidate.IsNew();
                var w = service.Build();
                if (!isNEw) w = w.And(w => w.Id != id);
                w = w.And(w => w.Name.Contains(name));
                var isAny = await service.AnyAsync(w);
                if (isAny) cntxt.AddFailure("please chose other name");
            })
        ;
    }
}
