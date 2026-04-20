using Fly.Framework.Server.Persistence.Contexts;
using MySqlConnector;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Fly.Framework.Server.Common.Helpers;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Fly.Framework.Server.Persistence.Mysql;

public class MySqlInfrastructureInjectNorrmal
{

}

public static class MySqlInfrastructureInject
{
    private static string? conStr = null;
    public static IServiceCollection MySqlInjection<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbCustomeContext
    {
        AddServices(services);
        AddProvider<TContext>(services, configuration);

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
    }

    private static void AddProvider<TContext>(IServiceCollection services, IConfiguration configuration)
        where TContext : DbCustomeContext
    {
        services.AddDbContextFactory<TContext>((provider, options) =>
        {
            int rep = 0;
            var mac = Environment.MachineName;
            conStr = DataHelper.Shutdown(configuration.GetConnectionString($"MySqlDefaultConnection"));

            DatabaseSettings settings = new();
            configuration.GetSection(nameof(DatabaseSettings)).Bind(settings);
            var logger = provider.GetRequiredService<ILogger<MySqlInfrastructureInjectNorrmal>>();
            var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder(conStr);
            var selectedLang = "";
            if (settings is not null && settings.Databases.Count != 0)
            {
                var dataseSwitcher = provider.GetRequiredService<IDataseSwitcher>();
                //mySqlConnectionStringBuilder.Database += dataseSwitcher.CurrentDatabase;
                var selected = settings.Databases.SingleOrDefault(w => w.DatabasesName == dataseSwitcher.CurrentDatabase);
                if (selected is null)
                {
                    selected = settings.Databases.Single(w => w.DatabasesName == settings.PrimaryDatabase);
                }
                selectedLang = selected.DatabasesName;
                mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder(selected.DatabasesConnection);
            }

            // how could i access the `GetRequiredService`
            // GetRequiredService<IDataseSwitcher>();

            conStr = mySqlConnectionStringBuilder.ToString();
            //  todo: checkk
            //logger.LogWarning("selected lang in sql is {0}", selectedLang);
            var ver = ServerVersion.AutoDetect(conStr);
            options.UseMySql(conStr, ver);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
#if DEBUG
            logger.LogWarning("Database : {0}", mySqlConnectionStringBuilder.Database);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
#endif
            //  TODO: Check
            //  NOTE: Check
        }, ServiceLifetime.Transient);

    }

    //  TODO: Featrue need ne be checked and tested.
    private static void BackupDatabase(this WebApplication app, IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<MySqlInfrastructureInjectNorrmal>>();
        logger.LogWarning("Takeing backup ...");
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
#if DEBUG
        var mac = Environment.MachineName;
        conStr = configuration.GetConnectionString($"MySqlDefaultConnection");
#else
        conStr = configuration.GetConnectionString("MySqlDefaultConnection");
#endif
        try
        {
            var rootDire = configuration.GetValue<string>("BackupFolder");
            var connectionString = conStr;
            if (string.IsNullOrEmpty(rootDire) || !Directory.Exists(rootDire))
            {
                logger.LogWarning("No backup folder specified or it does not exist");
                rootDire = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (string.IsNullOrEmpty(rootDire))
            {
                logger.LogWarning("`MyDocuments` connot be found");
                rootDire = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            if (string.IsNullOrEmpty(rootDire))
            {
                logger.LogWarning("`Desktop` connot be found");
                rootDire = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents");
            }
            if (string.IsNullOrEmpty(rootDire))
            {
                logger.LogWarning("Cannot take backup");
                return;
            }
            
            var backupDire = Path.Combine(
                rootDire,
                "MMTECH", "SmartiERP", "Backups");
            
            logger.LogWarning("Using backup folder {0} with constr {1}", backupDire, connectionString);

            Directory.CreateDirectory(backupDire);
            var filePrint = DateTime.UtcNow.ToString("yyyy_MM_dd__HH_mm");
            var backupFilePath = Path.Combine(backupDire, "backup_" + filePrint + ".sql");
            TakeBackup1(connectionString, app, backupFilePath);
            logger.LogInformation("Using connection string {0}", connectionString);
            //TakeBackup2(connectionString, app, backupFilePath);

        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Backup database error.");
        }
    }

    private static void TakeBackup1(string connectionString, WebApplication app, string backupFilePath)
    {
        if (OperatingSystem.IsLinux() || OperatingSystem.IsWindows())
            return;
        
        var u = connectionString.Split(';').First(s => s.StartsWith("Uid=")).Split('=')[1];
        var p = connectionString.Split(';').First(s => s.StartsWith("Pwd=")).Split('=')[1];
        var d = connectionString.Split(';').First(s => s.StartsWith("Database=")).Split('=')[1];
        var com = "mysqldump";
        ProcessStartInfo processInfo = new("cmd.exe",
            @$"/K {com} --add-drop-database --add-drop-table --user={u} --password={p} --databases {d} > {backupFilePath} & exit")
            {
                UseShellExecute = true,
                CreateNoWindow = true
            };

        Process px = Process.Start(processInfo);
        px.Start();
        px.WaitForExit();
    }
    private static void TakeBackup2(string connectionString, WebApplication app, string backupFilePath)
    {
        var u = connectionString.Split(';').First(s => s.StartsWith("Uid=")).Split('=')[1];
        var p = connectionString.Split(';').First(s => s.StartsWith("Pwd=")).Split('=')[1];
        var d = connectionString.Split(';').First(s => s.StartsWith("Database=")).Split('=')[1];
        var args = $"-u {u} -p {d} > {backupFilePath}";

        //FileName = "D:\\servers\\laragon\\bin\\mysql\\mariadb-11.4.2-winx64\\bin\\mysqldump", 
        // -u app -p mmtech__smartierp__20241126_test > C:\Users\islam\Documents\MMTECH\SmartiERP\backup_2024_12_27__07_33.sql

        //  "D:\\servers\\laragon\\bin\\mysql\\mariadb-11.4.2-winx64\\bin\\mysqldump"
        var sqlDumpBinDire = Environment.GetEnvironmentVariable("mt_sqlbin");

        var procInfo = new ProcessStartInfo
        {
            FileName = sqlDumpBinDire, //"D:\\servers\\laragon\\bin\\mysql\\mariadb-11.4.2-winx64\\bin\\mysqldump",
            WorkingDirectory = app.Environment.WebRootPath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        var process = new Process
        {
            StartInfo = procInfo,
        };

        process.Start();
        Thread.Sleep(1 * 1000);
        process.StandardInput.WriteLine(connectionString.Split(';').First(s => s.StartsWith("Pwd=")).Split('=')[1]);
        process.WaitForExit();
        //process.StandardInput.Close();
        //logger.LogWarning("Backup taken ...");
    }

    private static bool _startupTime = true;
    
    public static WebApplication UseMySqlServerPersistence(this WebApplication app)
    {

        //  TODO: Take a backup for mysql database before start.
        var serviceProvider = app.Services;

        if (_startupTime)
        {
            app.BackupDatabase(serviceProvider);
        }

        var logger = serviceProvider.GetRequiredService<ILogger<MySqlInfrastructureInjectNorrmal>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var hours = configuration.GetValue<int?>("BackupHours");
        hours ??= 3;
                
        if (hours is 0)
        {
            hours = 1;
        }
        
        var timer = new Timer(TimeSpan.FromHours(hours.GetValueOrDefault()))
        {
            Enabled = true,
            AutoReset = true
        };
        
        logger.LogWarning("Backup timer interval set to {a} hours", hours);

        timer.Elapsed += (sender, args) =>
        {
            logger.LogInformation("Taking a frequent backup");
            BackupDatabase(app, serviceProvider);
        };
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
