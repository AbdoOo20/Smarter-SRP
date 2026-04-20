using Fly.Framework.Server.Authentications;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Security.Algorithms;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fly.Framework.Server;

public interface IFlyServerFrameworkMarker { }

public static class ServerFrameworkInjection
{
    public static IServiceCollection InjectFrameworkServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.InjectAuthentication(configuration);
        services.InjectServerPersistence(configuration);
        services.AddTransient<ICustomEncryptionAlgorithm, Security.Algorithms.v1.CustomEncryptionAlgorithm>();
        //  https://stackoverflow.com/questions/41267606/getting-asp-net-core-shutdown-triggering-applicationstopping-event-in-iisexpress
        //services.AddHostedService<DatabaseBackupManger>();

        services.AddSingleton<ILocalizationService, LocalizationService>();
        return services;
    }

    public static WebApplication UseBaseFramework(this WebApplication app)
    {
        app.UseServerPersistence();
        app.UseAppAuthentication();

        app.Use(async (context, next) =>
        {
            var serviceProvider = context.RequestServices.GetRequiredService<IServiceProvider>();
            var logger = context.RequestServices.GetRequiredService<ILogger<IFlyServerFrameworkMarker>>();
            using (var scope = serviceProvider.CreateScope())
            {
                var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();
                string? acLang = context.Request.Headers.AcceptLanguage.Count > 0 ? context.Request.Headers.AcceptLanguage[0] : "ar";
                //  todo: check
                //logger.LogWarning("AcceptLanguage: {0}", acLang);
                localizationService.SetCurrentLanguage(acLang);
                await next.Invoke();
            }
            // NOTE: Check
            //if (context.User.Identity is not null)
            //{
            //    // my be not authenticated
            //    if (context.User.Identity.IsAuthenticated)
            //    {
            //        var algorithm = context.RequestServices.GetRequiredService<ICustomEncryptionAlgorithm>();
            //        // TODO: must be changed
            //        var keyClaim = context.User.FindFirst(CustomClaims.EncryptKey) ?? throw new Exception("key not found"); // TODO: remove Exception
            //        algorithm.SetKey(keyClaim.Value);
            //    }
            //}
        });

        return app;
    }
}

//public class DatabaseBackupManger(ILogger<DatabaseBackupManger> logger) : IHostedService, IDisposable
//{
//    private int executionCount = 0;
//    private readonly ILogger<DatabaseBackupManger> _logger = logger;
//    private Timer? _timer = null;

//    public Task StartAsync(CancellationToken stoppingToken)
//    {
//        _logger.LogInformation("Timed Hosted Service running.");

//        _timer = new Timer(DoWork, null, TimeSpan.Zero,
//            TimeSpan.FromHours(4));

//        return Task.CompletedTask;
//    }

//    private void DoWork(object? state)
//    {
//        var count = Interlocked.Increment(ref executionCount);

//        _logger.LogInformation(
//            "Timed Hosted Service is working. Count: {Count}", count);
//    }

//    public Task StopAsync(CancellationToken stoppingToken)
//    {
//        _logger.LogInformation("Timed Hosted Service is stopping.");

//        _timer?.Change(Timeout.Infinite, 0);

//        return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//        _timer?.Dispose();
//    }
//}