using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using MediatR;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.AccoutHandlers;

internal class GetAccountBalanceHandler : IRequestHandler<GetAccountBalance, GetAccountBalanceResponse>
{
    //private readonly IValidator<GetAccountBalance> _validator;
    private readonly IServiceBase<DtoEntry, TEntry> _entryService;
    private readonly IServiceBase<DtoAccount, TAccount> _accountService;
    private readonly IServiceBase<DtoEntryDetail, TEntryDetail> _entryDetailServic;

    public GetAccountBalanceHandler(IServiceBase<DtoAccount, TAccount> accountService,
        IServiceBase<DtoEntry, TEntry> entryService,
        IServiceBase<DtoEntryDetail, TEntryDetail> entryDetailServic
        /*,IValidator<GetAccountBalance> validator*/)
    {
        //_validator = validator;
        _entryService = entryService;
        _accountService = accountService;
        _entryDetailServic = entryDetailServic;
    }

    public async Task<GetAccountBalanceResponse> Handle(GetAccountBalance request, CancellationToken cancellationToken)
    {
        var validator = new GetAccountBalanceValidator(_accountService, _entryService, _entryDetailServic);
        var validateStatus = await validator.ValidateAsync(request, cancellationToken);
        if (!validateStatus.IsValid)
        {
            var errors = string.Join(",", validateStatus.Errors.Select(s => s.ErrorMessage));
            throw new Exception(errors);
        }

        var det = await _entryDetailServic.GetAllAsync(d => d.AccountId == request.AccountId);
        var debit = det.Items.Select(s => s.Debit).Sum();
        var credit = det.Items.Select(s => s.Credit).Sum();
        var total = credit - debit;
        return new GetAccountBalanceResponse()
        {
            Balance = total
        };
    }
}