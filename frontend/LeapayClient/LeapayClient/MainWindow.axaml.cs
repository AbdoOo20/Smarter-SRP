using System.Net.Sockets;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Leapay.Client.Features.Theming;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Reports;
using Leapay.Client.Services;
using Leapay.Client.UdpDiscovery;
using Leapay.Client.UI.Scaling;
using Leapay.Client.UserInterface.Authentication;
using Leapay.Client.UserInterface.Routing;
using Leapay.Client.UserInterface.Sales.SalesPoint;
using Leapay.Client.Wrappers;
using SmartErpWin.Core;
using SmartErpWin.Core.Globals;
using SmartErpWin.Core.Helpers;
using SmartErpWin.Core.HttpServices;
using SukiUI.Controls;
using SukiUI.Enums;
using SukiUI.Models;
using Svg;
using Velopack;
using Velopack.Locators;

namespace Leapay.Client;

public partial class MainWindow : ScalableWindow
{
    public MainWindow() { } // designer
    
    private readonly LoginWindow _loginWindow;

    public MainWindowViewModel ViewModel { get; set; }
    
    private bool hasDiscoveryAlreadyStarted = false;
    
    public MainWindow(MainWindowViewModel viewModel)
    {
        _loginWindow = App.GetService<LoginWindow>();
        ViewModel = viewModel;
        ViewModel.Clipboard = Clipboard;
        
        InitializeComponent();
        UsernameTextBox.AttachedToVisualTree += (_, _) => ContentHelper.ForceFocus(this, UsernameTextBox);
        
        DataContext = this;
        
        Closing += OnClosing;
        SetupEvents();
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        PreferencesService.Set(s => s.IsMenuExpanded = MainSideMenu.IsMenuExpanded);
    }

    private void SetupEvents()
    {
        Dispatcher.UIThread.UnhandledException += (_, e) =>
        {
            Program.OnUnhandledException(e.Exception);
            e.Handled = true;
        };

        Global.OnHttpError += (_, e) => Program.OnUnhandledException(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => Program.OnUnhandledException(e.ExceptionObject as Exception);
        //TaskScheduler.UnobservedTaskException += (_, e) => Program.OnUnhandledException(e.Exception);
        
        // This has to be here before Loaded.
        BackgroundStyle = PreferencesService.Get(b => b.BackgroundStyle);

        Loaded += (_, _) =>
        {
            ContentHelper.SetGridResolution(this, MainGrid);
            CheckThemeSettings();
        
            ReportRunner.PreoarwReportRunner();
            ReportRunnerHelper.RegisterEvents();
            
            CheckAuthorisationAsync();
        };
        
        ViewModel.SignInComplete += (_, _) => CheckAuthorisationAsync();
        Closing += (_, _) => AuthenticationStorageService.DeleteToken();
    }

    private async void CheckAuthorisationAsync(bool waitingForDiscovery = false)
    {
        if (!waitingForDiscovery)
        {
            await CheckForUpdatesAsync();
        
            _ = DatabaseHelper.TakeBackupAsync();
            EventHelper.SyncSubscribers();
        }

        if (!hasDiscoveryAlreadyStarted)
        {
            _ = BackendDiscovery.StartRoutineDiscoveryAsync();
            hasDiscoveryAlreadyStarted = true;
        }

        if (StringGlobals.Arguments.FirstOrDefault() == BackendSetup &&
            PreferencesService.Get(s => !s.IsBackendFullySetup))
        {
            ViewModel.InitializeBackendSetup();
            ViewModel.SwitchToBackendSetup();

            return;
        }
        
        if (PreferencesService.Get(s => !s.HasUserSelectedSystemType))
        {
            ViewModel.SwitchToBackendSetup();
            return;
        }
        
        if (ViewModel.IsUserSelectingBackend)
        {
            // Return here. Let the other page do the reset.
            return;
        }
        
        ViewModel.SetRefreshState(true);
        ViewModel.SwitchToLoading();

        // Keep retrying for auto backend selection. If BackendDiscovery decides the user has
        // to select a backend, stop this method and let the backend selection page do the rest.
        if (StringGlobals.ActiveConnection is null)
        {
            ViewModel.SetInfo(ViewModel["ConnectingToServer"]);
            await Task.Delay(2000);
            CheckAuthorisationAsync(true);

            return;
        }
        
        try
        {
            var check = AuthenticationStorageService.TryGetToken(out var token);

            if (!check)
                throw new();

            var request = await BackendService.EnsureTokenIsValidAsync(token);

            if (!request)
                throw new();
            
            ViewModel.AuthenticateRequestsForFeatureBase(token);
        }
        catch (Exception x)
        {
            if (x is HttpRequestException { InnerException: SocketException } )
            {
                await RetryAsync();
                return;
            }
            
            StartSignIn();
            
            //CheckAuthorisationAsync();
            return;
        }

        SetMenuItems();
        ContentHelper.RefreshFavouritesInMainMenu();
        
        await NavigateToScreenArgsAsync();
        
        ViewModel.SetRefreshState(false);
    }

    private async Task CheckForUpdatesAsync()
    {
        ViewModel.SetInfo(ViewModel["CheckingForUpdates"]);
        
        const string updateUrl = "https://lmmp26.blob.core.windows.net/$web/leap-front-velo";

        var mgr = new UpdateManager(updateUrl, new UpdateOptions
        {
            AllowVersionDowngrade = true,
            MaximumDeltasBeforeFallback = 100
        });
        var version = mgr.CurrentVersion;

        if (!mgr.IsInstalled)
            return;
        ViewModel.ApplicationTitle = "LeaPay" + " v" + version?.ToFullString();
        
        var update = await mgr.CheckForUpdatesAsync();

        if (update is null)
            return;

        ViewModel.IsUpdating = true;
        
        ViewModel.SwitchToUpdating();
        ViewModel.UpdateInfo = ViewModel["UpdatesUnderway"];

        await mgr.DownloadUpdatesAsync(update, i =>
        {
            ViewModel.UpdatePercentage = i;
        });

        await Task.Delay(500);
        
        mgr.ApplyUpdatesAndRestart(null);
    }
    
    private async Task NavigateToScreenArgsAsync()
    {
        var args = StringGlobals.Arguments.FirstOrDefault();

        if (string.IsNullOrEmpty(args))
            return;
        
        var uri = new Uri(args);

        var parse = CommonTextHelpers.ParseQuery(uri.Query);
        
        Debug("Got launch args: {0}", string.Join("|", parse.Select(s => $"{s.Key}: {s.Value}")));
        if (!parse.TryGetValue(ScreenStartupArg, out var screen) || !ContentHelper.TryGetView(screen, out var view))
        {
            return;
        }
        
        if (!view?.Item.IsVisible ?? true)
            return;
        
        var add = parse.TryGetValue(ScreenStartupAddArg, out var addStr)
                  && bool.TryParse(addStr.ToLower(), out var addBol) && addBol;
        
        await LaunchArgs(view, add);
    }

    private async Task HandleNonMsixBasedArgs()
    {
        var args = CommonTextHelpers.ParseArgs(StringGlobals.Arguments);

        if (!args.TryGetValue(ScreenStartupArg, out var screen) || !ContentHelper.TryGetView(screen, out var view))
        {
            return;
        }

        if (!view?.Item.IsVisible ?? true)
            return;

        var add = args.TryGetValue(ScreenStartupAddArg, out var addStr)
                  && bool.TryParse(addStr.ToLower(), out var addBol) && addBol;
        await LaunchArgs(view, add);
    }

    private async Task LaunchArgs(NavigationViewItemWrapper view, bool add)
    {
        ViewModel.MutateSideMenu(m => m.OpenPaneLength = 0);
        ViewModel.ApplicationTitle = view?.CardModel?.CardName ?? ViewModel.ApplicationTitle;
        
        await Task.Delay(500); // artificial delay for the menu UI to load
        
        ViewModel.PageNavigationService.RequestNavigation(view?.PageType);

        //await Task.Delay(500);

        if (add && view?.Item.PageContent is UserControlBase ucb)
        {
            ucb.SynchronisationModel?.OnSynchronisationRequestedAsync(CodeBook.AddNew);
        }
    }

    private void CheckThemeSettings()
    {
        ViewModel.Theming = App.GetService<ThemingViewModel>();

        ViewModel.AnimationsEnabled = PreferencesService.Get(b => b.BackgroundAnimations);
        
        ViewModel.Theming.BackgroundStyleChanged += style =>
        {
            BackgroundStyle = style;
            ViewModel.BackgroundStyle = style;
        };
        
        ViewModel.Theming.BackgroundAnimationsChanged += enabled => ViewModel.AnimationsEnabled = enabled;
        ViewModel.Theming.CustomBackgroundStyleChanged += shader => ViewModel.CustomShaderFile = shader;
        ViewModel.Theming.BackgroundTransitionsChanged += enabled => ViewModel.TransitionsEnabled = enabled;
    }

    private void SetMenuItems()
    {
        ViewModel.SwitchToUi();
        ViewModel.SetMenuItems(MainSideMenu);
    }
    
    private void ThemeMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;
        if (e.Source is not MenuItem mItem) return;
        if (mItem.DataContext is not SukiColorTheme cTheme) return;
        vm.ChangeTheme(cTheme);
    }

    private void BackgroundMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;
        if (e.Source is not MenuItem mItem) return;
        if (mItem.DataContext is not SukiBackgroundStyle cStyle) return;
        vm.BackgroundStyle = cStyle;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        IsMenuVisible = !IsMenuVisible;
    }
    
    private int _retries = 1;
    
    private async Task RetryAsync()
    {
        var s = _retries * 5;
        
        for (var i = 0; i < s; i++)
        {
            await Task.Delay(1000);
            ViewModel.LoadingInfo = string.Format(
                ViewModel["ConnectionErrorRetryMessage"],
                s - i);
        }
        
        _retries++;
        CheckAuthorisationAsync();
    }

    private void StartSignIn()
    {
        ViewModel.SwitchToSignIn();
        ContentHelper.ForceFocus(this, UsernameTextBox);
    }

    private void UsernameOnKeyDown(object? sender, KeyEventArgs e)
    {
        if(e is { KeyDeviceType: KeyDeviceType.Keyboard, Key: Key.Enter })
        {
            PasswordTextBox.Focus();
        }
    }

    private SukiSideMenu GetSideMenu() => MainSideMenu;
    
    private void PasswordOnKeyDown(object? sender, KeyEventArgs e)
    {
        if(e is { KeyDeviceType: KeyDeviceType.Keyboard, Key: Key.Enter })
        {
            ViewModel.LoginCommand.Execute(null);
        }
    }

    public override event EventHandler<PointerPressedEventArgs> BeginResize;
    public override event EventHandler<PointerEventArgs> Resize;
    public override event EventHandler<PointerReleasedEventArgs> EndResize;

    private async void VerifyWindow(object? sender, KeyEventArgs e)
    {
        _ = ViewModel.VerifyConfig(e);

        var tabVm = App.GetService<TabbedNavigationalPageViewModel>();
        if (tabVm.SelectedTab?.ViewName is not "SalesCheckoutTab")
            return;

        var salesPoint = App.GetService<SalesPointPageViewModel>();
        
        switch (e.Key)
        {
            case Key.F8:
                await salesPoint.PayCommand.ExecuteAsync("true");
                break;
            case Key.F7:
                await salesPoint.PutOnHoldCommand.ExecuteAsync(null);
                break;
            case Key.F4:
                await salesPoint.AbortCommand.ExecuteAsync(null);
                break;
            case Key.F9:
                await salesPoint.PayCommand.ExecuteAsync("false");
                break;
            case Key.F10:
                await salesPoint.ReturnSalesCommand.ExecuteAsync(false);
                break;
            case Key.F6:
                await salesPoint.EditSalesCommand.ExecuteAsync(false);
                break;
            case Key.F12:
                await salesPoint.ShowSuspendedCommand.ExecuteAsync(false);
                break;
        }
    }
}