using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Global.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Global.Service;

public class UserService : ServiceBase<DtoUser, TUser>, IUserService
{
    public UserService(ILogger<UserService> logger, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapsterMapper, IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {
    }


}