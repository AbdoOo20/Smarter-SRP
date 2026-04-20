namespace Leapay.Client.Services;

public static class LoggingServices
{
    private const string FileName = "win-.log";
    
    public static void SetupLogger()
    {
        Logger = new LoggerConfiguration().MinimumLevel.Verbose().CreateLogger();

        if (!OperatingSystem.IsWindows())
            return;
        
        var logsDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
        
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