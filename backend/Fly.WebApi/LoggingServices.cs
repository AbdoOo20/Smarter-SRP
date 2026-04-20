using Serilog;
using Serilog.Events;

namespace Fly.WebApi;

public class LoggingServices
{
    private const string FileName = "win-.log";
    
    public static void SetupLogger()
    {
        Logger = new LoggerConfiguration().MinimumLevel.Verbose().CreateLogger();
        
        var logsDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
        Directory.CreateDirectory(logsDirectory);
        
        Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File($@"{logsDirectory}\debug\{FileName}",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .WriteTo.File($@"{logsDirectory}\verbose\{FileName}",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Verbose)
            .WriteTo.File($@"{logsDirectory}\info\{FileName}",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.File($@"{logsDirectory}\warning\{FileName}",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Warning)
            .WriteTo.File($@"{logsDirectory}\error\{FileName}",
                rollingInterval: RollingInterval.Day, 
                restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.File($@"{logsDirectory}\fatal\{FileName}",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Fatal)
            .WriteTo.File($@"{logsDirectory}\all\{FileName}",
                rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();
    }
}