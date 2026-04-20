using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Global.Dtos;

namespace Fly.Core.Modules.Global.Service;

public interface IUserService : IServiceBase<DtoUser, TUser>
{

}