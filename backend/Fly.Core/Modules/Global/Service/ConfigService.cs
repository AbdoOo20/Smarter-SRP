using System.Linq.Expressions;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Global.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Global.Service;

public class ConfigService : ServiceBase<DtoConfig, TConfig>, IConfigService
{
    public ConfigService(ILogger<ConfigService> logger, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapsterMapper, IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {
    }

    public async ValueTask<T?> GetConfigValue<T>(Expression<Func<DtoConfig, T>> selector)
    {
        var all = await GetAllAsAsync(selector);
        return all.Items.FirstOrDefault();
    }

}