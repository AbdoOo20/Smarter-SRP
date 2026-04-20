using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;

namespace Fly.Core.Modules.C.Accounting.Services;

public interface IAccountModuleService : IServiceBase<DtoAccount, TAccount>
{
    ValueTask UpdateOpeningBalance(Identifier accountId, decimal openingDebtor, decimal openingCreditor);
}