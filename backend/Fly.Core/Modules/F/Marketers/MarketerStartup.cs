using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Core.Modules.F.Marketers.Services.InternalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules.F.Marketers;

public static class MarketerStartup
{
    public static IServiceCollection InjectMarketerSModule
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMarketerModuleService, MarketerModuleService>();
        services.AddScoped<IMarketerInventoryService, MarketerInventoryService>();

        return services;
    }

    public static WebApplication UseMarketerModule(this WebApplication app)
    {
        return app;
    }
}