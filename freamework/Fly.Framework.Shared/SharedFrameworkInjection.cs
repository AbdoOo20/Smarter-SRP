using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Framework.Shared;

public static class SharedFrameworkInjection
{
    public static IServiceCollection InjectFrameworkShared(this IServiceCollection services, IConfiguration configuration)
    {

        //var settings = new JsonSerializerSettings();
        //settings.Converters.Add(new IdentifierConverter());

        return services;
    }
}