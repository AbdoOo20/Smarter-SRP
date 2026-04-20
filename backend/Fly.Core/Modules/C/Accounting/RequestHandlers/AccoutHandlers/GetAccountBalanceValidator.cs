using FluentValidation;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.AccoutHandlers;

internal class GetAccountBalanceValidator : AbstractValidator<GetAccountBalance>
{
    public GetAccountBalanceValidator(IServiceBase<DtoAccount, TAccount> accountService,
        IServiceBase<DtoEntry, TEntry> entryService,
        IServiceBase<DtoEntryDetail, TEntryDetail> entryDetailService)
    {
        RuleFor(prp => prp.AccountId)
            .NotEmpty()
            .NotNull()
            .CustomAsync(async (accId, cntxt, cancel) =>
            {
                var isExists = await accountService.AnyAsync(c => c.Id == accId);
                if (!isExists)
                {
                    cntxt.AddFailure("This account is not exists");
                    return;
                }
            });
    }
}