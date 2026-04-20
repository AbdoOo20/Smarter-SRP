using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using AsyncImageLoader;
using AsyncImageLoader.Loaders;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Leapay.Client.Common;
using Leapay.Client.Features.CustomTheme;
using Leapay.Client.Features.Dashboard;
using Leapay.Client.Features.Splash;
using Leapay.Client.Features.Theming;
using Leapay.Client.Helpers;
using Leapay.Client.Services;
using Leapay.Client.Styling.Dynamic;
using Leapay.Client.UdpDiscovery;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Dialogs;

namespace Leapay.Client;

public class App : Application
{
    private static ServiceCollection? _services;

    private static ServiceProvider? _provider;
    
    private static IApplicationLifetime? _lifetime;

    private static string cacheFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\LeapayStateCore\";
    
    [DllImport("Kernel32")]
    private static extern void AllocConsole();
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static T GetService<T>() where T : class
    {
        return _provider?.GetService(typeof(T)) as T
               ?? throw new($"Check if {typeof(T).Name} service is registered");
    }

    public static T? GetServiceOrNull<T>() where T : class
    {
        return _provider?.GetService(typeof(T)) as T;
    }
    
    public static void FocusMainWindow()
    {
        foreach (var otherWindows in GetOpenChildWindows())
        {
            otherWindows.WindowState = WindowState.Minimized;
        }

        var mainWindow = GetMainWindow();
        mainWindow.WindowState = WindowState.Maximized;
        // The following brings the main window atop of other external windows too
        mainWindow.Topmost = true;
        mainWindow.Topmost = false;
    }

    
    public static bool TryGetDesktopLifetime([NotNullWhen(true)] out IClassicDesktopStyleApplicationLifetime? lifetime)
    {
        lifetime = null;

        if (_lifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return false;
        
        lifetime = desktop;
        return true;
    }
    
    public static void Shutdown()
    {
        // We can only shut down if the lifetime is of a desktop type
        if (TryGetDesktopLifetime(out var desktop))
        {
            desktop.Shutdown();
        }
    }

    public static void CloseDialogWindows()
    {
        // Since we only host dialog windows, the only non-dialog window is the MainWindow.
        if (!TryGetDesktopLifetime(out var desktop))
            return;
        
        foreach (var window in desktop.Windows.ToList())
        {
            if (window is not MainWindow)
                window.Close();
        }
    }
    
    public static void CloseDialogWindowOfContentType<TContent>() where TContent: UserControl
    {
        if (!TryGetDesktopLifetime(out var desktop))
            return;
        
        foreach (var window in desktop.Windows.ToList())
        {
            if (ContentHelper.DoesWindowHaveContentType<TContent>(window))
                window.Close();
        }
    }
    
    public static Window? GetOpenWindowOfType<TWindowContent>() where TWindowContent: UserControl
    {
        if (!TryGetDesktopLifetime(out var desktop))
            throw new("Lifetime is not set");

        return desktop.Windows.FirstOrDefault(ContentHelper
            .DoesWindowHaveContentType<TWindowContent>);
    }
    
    public static List<Window> GetOpenWindows()
    {
        if (!TryGetDesktopLifetime(out var desktop))
            throw new("Lifetime is not set");

        return desktop.Windows.ToList();
    }
    
    public static List<Window> GetOpenChildWindows()
    {
        if (!TryGetDesktopLifetime(out var desktop))
            throw new("Lifetime is not set");

        return GetOpenWindows().Where(c => c != GetMainWindow()).ToList();
    }
    
    public static Window GetMainWindow()
    {
        if (!TryGetDesktopLifetime(out var desktop))
            throw new("Lifetime is not set");

        return desktop.MainWindow ?? throw new("Main window was closed off");
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        _services = [];
        _provider = ConfigureServices(_services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _services.AddSingleton(desktop);

            var views = ConfigureViews(_services);
            DataTemplates.Add(new ViewLocator(views));
            desktop.MainWindow = GetService<MainWindow>();
            GetService<ThemingViewModel>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var services = new ServiceCollection();
            services.AddSingleton(singleView);
            var views = ConfigureViews(services);
            _provider = ConfigureServices(services);
            DataTemplates.Add(new ViewLocator(views));

            // Ideally, we want to create a MainView that host app content
            // and use it for both IClassicDesktopStyleApplicationLifetime and ISingleViewApplicationLifetime
            singleView.MainView = new SukiMainHost
            {
                Hosts = [
                    new SukiDialogHost
                    {
                        Manager = new SukiDialogManager()
                    }
                ],
                Content = views.CreateView<DialogViewModel>(_provider)
            };
        }

        GlobalStyles.Apply();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var diskCachedWebImageLoader = new DiskCachedWebImageLoader(GetService<HttpClient>(), false, cacheFolder + "images");
        
        ImageLoader.AsyncImageLoader = diskCachedWebImageLoader;
        base.OnFrameworkInitializationCompleted();

        _lifetime = ApplicationLifetime;
        //    Shadcn.Configure(Application.Current, ThemeVariant.Dark);
    }

    private static SukiViews ConfigureViews(ServiceCollection services)
    {
        return new SukiViews()
            .AddView<SplashView, SplashViewModel>(services)
            .AddView<CustomThemeDialogView, CustomThemeDialogViewModel>(services);
    }
    
    private static ServiceProvider ConfigureServices(ServiceCollection services)
    {
        InjectionServices.BeginInjection(services);
        
        return services.BuildServiceProvider();
    }
}