using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules.C.Accounting;

//public class AccountingService
//{

//    private readonly DbApplicationContext _context;

//    public AccountingService(DbApplicationContext context)
//    {
//        _context = context;
//    }

//    public void CreateAccount(CreateAccountRequest request)
//    {

//    }

//    public async ValueTask<IEnumerable<DtoAccount>> GetAllAccountsByParent(Guid parentId)
//    {
//        var all = _context.Accounts
//           .AsNoTracking()
//           .Where(w => w.ParentId == parentId)
//           .ProjectToType<DtoAccount>().ToListAsync()
//       ;
//        return await all;
//    }

//    public async ValueTask<IEnumerable<DefaultLookup>> GetLookupAccounts(AccountQueryRequest request)
//    {
//        var q = _context.Accounts;

//        if (!string.IsNullOrEmpty(request.SearchText))
//        {
//            q.Where(w => w.Name.Contains(request.SearchText));
//        }

//        if (request.Liimit > 0 && request.Page >= 0)
//        {
//            var skip = request.Page * request.Liimit;
//            q.Skip(skip.Value).Take(request.Liimit.Value);
//        }

//        return await q.Select(s => new DefaultLookup() { Value = s.Guid, Display = s.Name }).ToListAsync();
//    }
//}

internal static class AccountingModuleInterfaceExtentions
{

    internal static IAccountingModuleBuilder ConfigureAccounting(this ServiceCollection services)
    {
        IAccountingModuleBuilder? builer = null;
        return builer;
    }

    internal static void AppendAccount(this IAccountingModuleBuilder accountingModuleBuilder, IConfiguration configuration)
    {

    }

}