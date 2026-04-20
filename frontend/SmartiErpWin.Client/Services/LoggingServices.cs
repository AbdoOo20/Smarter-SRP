using Serilog;
using Serilog.Events;

namespace SmartiErpWin.Client.Services;

public static class LoggingServices
{
    private const string FileName = "win-.log";
    
    public static void SetupLogger()
    {
        Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File($@"{LogsDirectory}\debug\{FileName}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Debug)
            .WriteTo.File($@"{LogsDirectory}\verbose\{FileName}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Verbose)
            .WriteTo.File($@"{LogsDirectory}\info\{FileName}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.File($@"{LogsDirectory}\warning\{FileName}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Warning)
            .WriteTo.File($@"{LogsDirectory}\error\{FileName}",
                    rollingInterval: RollingInterval.Day, 
                    restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.File($@"{LogsDirectory}\fatal\{FileName}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Fatal)
            .WriteTo.File($@"{LogsDirectory}\all\{FileName}",
                rollingInterval: RollingInterval.Day)
            .WriteTo.Conditional(f =>
            {
                AdjustConsoleLogEvent(f);
                
                return true;
            }, c => c.Console(restrictedToMinimumLevel: LogEventLevel.Verbose))
            .CreateLogger();
    }

    private static void AdjustConsoleLogEvent(LogEvent logEvent)
    { 
        // ignored
    }
}