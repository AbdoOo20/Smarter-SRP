using Fly.Framework.Server.Persistence.Implementations;
using Fly.Framework.Server.Persistence.Mysql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Framework.Server.Persistence;

public class FlyFramework
{

}

public class DatabaseSettings
{
    public class DatabaseConfig
    {
        public string DatabasesName { get; set; } = string.Empty;
        public string DatabasesConnection { get; set; } = string.Empty;
    }

    public bool ManyDatabases { get; set; }
    public string PrimaryDatabase { get; set; } = string.Empty;
    public List<DatabaseConfig> Databases { get; set; } = [];
}

public interface IDataseSwitcher
{
    string CurrentDatabase { get; }
    void SetCurrentDatabase(string database);
}

public static class PersistenceServiceInjection
{
    public static IServiceCollection InjectServerPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        //  https://poe.com/s/VTt0Af4Metl9Fm6Y0BRV
        //  TODO: encrypt `appsettings`
        services.AddScoped<IRepositryBase, EfRepositryBase>();
        services.AddScoped(typeof(IRepositryBase<>), typeof(EfRepositryBase<>));
        services.AddScoped(typeof(IServiceBase<,>), typeof(ServiceBase<,>));

        //services.MySqlInjection(configuration);
        //services.PostgresInjection(configuration);
        return services;
    }


    public static WebApplication UseServerPersistence(this WebApplication app)
    {

        app.UseMySqlServerPersistence();


        //app.Use(async (contex, next) =>
        //{
        //    var dbContext = contex.RequestServices.GetRequiredService<DbCustomeContext>();
        //    var logger = contex.RequestServices.GetRequiredService<ILogger<FlyFramework>>();

        //    //var isCreated = await dbContext.Database.EnsureCreatedAsync();
        //    //if (isCreated)
        //    //{
        //    var migrations = await dbContext.Database.GetPendingMigrationsAsync();
        //    if (migrations.Any())
        //    {
        //        logger.LogWarning("Applying migrations [{migrations}] ...", migrations.Count());
        //        await dbContext.Database.MigrateAsync();
        //    }
        //    //}

        //    await next.Invoke();
        //});
        return app;
    }
}
