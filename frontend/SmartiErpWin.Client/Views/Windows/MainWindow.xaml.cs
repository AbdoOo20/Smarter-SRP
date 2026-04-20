using System.Net.Sockets;
using SmartiErpWin.Client.ViewModels.Windows;
using SmartiErpWin.Client.WinHelpers;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.Views.Windows;

public partial class MainWindow: INavigationWindow
{
    public MainWindowViewModel ViewModel { get; }
    
    private IServiceProvider _serviceProvider;

    private AuthenticationStorageService _authService;
    
    private LoginWindow _loginWindow;

    private SessionService _sessionService;

    private BackendService _backendService;

    private LocalisationService _localisationService;
    
    public MainWindow(
        MainWindowViewModel viewModel,
        INavigationService navigationService,
        IServiceProvider serviceProvider,
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService,
        AuthenticationStorageService authService,
        LoginWindow loginWindow,
        BackendService backendServices,
        SessionService sessionService,
        LocalisationService localisationService
    )
    {
        _serviceProvider = serviceProvider;
        _authService = authService;
        _loginWindow = loginWindow;
        _sessionService = sessionService;
        _backendService = backendServices;
        _localisationService = localisationService;
        
        ViewModel = viewModel;
        
        DataContext = this;
        
        CheckAuthorisationAsync();
        StartInitialRefreshAsync();
        InitializeComponent();
        
        navigationService.SetNavigationControl(NavigationView);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(RootContentDialog);

        NavigationView.SetServiceProvider(serviceProvider);
    }

    INavigationView INavigationWindow.GetNavigation()
    {
        return NavigationView;
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        
    }
    
    public INavigationView GetNavigation() => NavigationView;

    public bool Navigate(Type pageType) => NavigationView.Navigate(pageType);

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
        NavigationView.SetPageProviderService(navigationViewPageProvider);
    
    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    private async void StartInitialRefreshAsync()
    {
        ViewModel.SynchronizeAllBase();
    }
    
    private async void CheckAuthorisationAsync()
    {
        if (!await CredentialManager.PoseConsentExternAsyncOp())
        {
            Application.Current.Shutdown();
            return;
        }
        
        ViewModel.SetRefreshState(true);

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
            
            Visibility = Visibility.Collapsed;
            
            var result = _loginWindow.ShowDialog() ?? false;

            if (!result)
            {
                Application.Current.Shutdown();
                return;
            }
            
            Visibility = Visibility.Visible;
            ViewModel.SetRefreshState(true);
            await Task.Delay(2000);
            ViewModel.SetRefreshState(false);
        }

        ViewModel.ShowMenu(NavigationView);
        StartInitialRefreshAsync();

        Visibility = Visibility.Visible;
        await Task.Delay(1000);
        ViewModel.SetRefreshState(false);

        //await CreateJson.StartImportAsync();
    }
    
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Application.Current.Shutdown();
    }

    private void MainWindowOnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ViewModel.WindowSizeChanged(e.NewSize);
    }
    
    private int _retries = 1;
    
    private async Task RetryAsync()
    {
        var s = _retries * 5;
        
        for (var i = 0; i < s; i++)
        {
            await Task.Delay(1000);
            ViewModel.LoadingInfo = string.Format(
                _localisationService["ConnectionErrorRetryMessage"],
                s - i);
        }
        
        _retries++;
        
        CheckAuthorisationAsync();
    }
}