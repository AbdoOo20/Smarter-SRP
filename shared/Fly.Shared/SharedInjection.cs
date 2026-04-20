using Fly.Framework.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Shared;

public interface IShareMarker { }

public static class SharedInjection
{
    public static IServiceCollection InjectShared(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .InjectFrameworkShared(configuration)
            .AddMediatR((opt) =>
            {
                opt.RegisterServicesFromAssemblyContaining<IShareMarker>();
            })
            ;
    }
}