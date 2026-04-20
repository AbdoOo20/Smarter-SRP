using Fly.Core.Modules.Global.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Fly.Core.Modules.Global.Service.ITokenService;

namespace Fly.Core.Modules.Global;

public static class Startup
{
    public static IServiceCollection InjectGlobalModule
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IConfigService, ConfigService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }

    public static WebApplication UseGlobalModule(this WebApplication app)
    {
        return app;
    }
}