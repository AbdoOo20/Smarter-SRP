using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Avalonia.Dialogs;
using Avalonia.Threading;
using Leapay.Client.Features.Theming;
using Leapay.Client.Services;
using ShowMeTheXaml;
using SmartErpWin.Core.Exceptions;
using SmartErpWin.Core.Globals;
using Velopack;
using Velopack.Logging;

namespace Leapay.Client;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        VelopackApp.Build()
            .OnFirstRun((v) => { })
            .SetLogger(new ConsoleVelopackLogger())
            .Run();

        try
        {
            LoggingServices.SetupLogger();
            
            // To handle msix packaging...

            if (IsRunningAsMsix())
            {
                var activationArgs = AppInstance.GetActivatedEventArgs();
                if (activationArgs is LaunchActivatedEventArgs launchArgs)
                {
                    StringGlobals.Arguments = [launchArgs.Arguments];
                    StringGlobals.IsMsix = true;
                }
            }
            else
            {
                StringGlobals.Arguments = args;
            }

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
            Task.Delay(-1);
        }
        catch (Exception exception)
        {
            OnUnhandledException(exception);
        }
        finally
        {
            CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var app = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseXamlDisplay();

        if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            app.UseManagedSystemDialogs();
        }

        return app;
    }
    
    public static void OnUnhandledException(Exception? e)
    {
        if (e is not { } exc)
        {
            _ = DialogueServices.ShowMessageAsync("Error",
                "An unknown error occurred.",
                "Ok");
            return;
        }

        Error(exc, "Unhandled error occurred");

        var locale = App.GetServiceOrNull<LocalisationService>() ?? new();

        if (e is LoggedException logEx)
        {
            /*if (SkippedErrors.Any(m => logEx.Message.Contains(m)))
            {
                Information("Silencing error: {0} from user interface", logEx.Message);
                return;
            }*/
            var msg = logEx.ErrorCode switch
            {
                //400 => locale["SystemErrorContactTechSupport"],
                401 => Logout(),
                500 => string.Format(locale["SystemErrorContactTechSupportWithErrorMessage"], logEx.Message),
                400 => string.Format(locale["SupportWithErrorMessage"], logEx.Message),
                _ => logEx.Message
            };

            TryForceSignOut(logEx);

            _ = DialogueServices.ShowMessageAsync("Error", msg, locale["OK"]);
            return;
        }

        _ = DialogueServices.ShowMessageAsync(locale["Error"], exc.Message, locale["OK"]);
    }

    public static string Logout()
    {
        var settings = App.GetService<ThemingViewModel>();
        settings?.LogoutCommand.Execute(null);

        return "You are unauthorised to do this action. You will be logged out, and " +
               "your session deemed expired.";
    }
    
    public static async void TryForceSignOut(Exception exception)
    {
        if (exception.Message.Contains("Unauthorized"))
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await DialogueServices.ShowMessageAsync("SorryTitle", "YouMustSignOut");
            });
            Logout();
        }
    }
    
    public static bool IsRunningAsMsix()
    {
        try
        {
            var pkg = Package.Current;
            return true;
        }
        catch
        {
            return false;
        }
    }
}