using Fly.Framework.Desktop.LicenseValidation;
using Fly.Framework.Desktop.Toos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Framework.Desktop;

public static class DesktopInjection
{
    public static IServiceCollection InjecDesktop(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BasicSubscriber>();
        services.AddScoped<ISubscriberModule, LicenseValidation.SubscriberModule>();

        return services;
    }

}