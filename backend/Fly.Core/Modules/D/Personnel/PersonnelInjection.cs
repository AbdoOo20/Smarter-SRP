using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules.D.Personnel;

public static class PersonnelInjection
{
    public static IServiceCollection InjectPersonnelModule
        (this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    public static WebApplication UsePersonnelModule(this WebApplication app)
    {
        return app;
    }
}


