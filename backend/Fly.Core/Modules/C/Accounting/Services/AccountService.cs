using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.C.Accounting.Services;

public class AccountService : ServiceBase<DtoAccount, TAccount>, IServiceBase<DtoAccount, TAccount>, IAccountModuleService
{
    public AccountService(IRepositryBase<TAccount> repositry,
        ILogger<AccountService> logger,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapsterMapper,
        IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {

    }

    public async ValueTask<PageList<DefaultLookup>> AccountsLookup(AccountQueryRequest queryRequest)
    {
        var w = Build();
        if (queryRequest.SearchText is not null)
        {
            w = w.And(w => w.Name.Contains(queryRequest.SearchText));
        }
        var all = await GetAllAsAsync(slc => new DefaultLookup() { Value = slc.Id, Display = slc.Name }, w);
        return all;
    }

    public async ValueTask UpdateOpeningBalance(Identifier accountId, decimal openingDebtor, decimal openingCreditor)
    {
        var account = await GetById(accountId);
        if (account is null) return;
        account.OpeningCreditor = openingCreditor;
        account.OpeningDebtor = openingDebtor;
        await Update(account);
    }


}