using Microsoft.Extensions.Hosting;
using SmartiErpWin.Client.Views.Pages;
using SmartiErpWin.Client.Views.Windows;
using Wpf.Ui;

namespace SmartiErpWin.Client.Services;

public class ApplicationHostService(IServiceProvider serviceProvider) : IHostedService
{
    private INavigationWindow? _navigationWindow;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
    
    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            _navigationWindow = (serviceProvider.GetService(typeof(MainWindow)) as INavigationWindow)!;
            _navigationWindow.ShowWindow();

            _ = _navigationWindow.Navigate(typeof(DashboardPage));
        }

        await Task.CompletedTask;
    }
    
    /*private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            var navigationWindow = serviceProvider.GetRequiredService<MainWindow>();
            navigationWindow.Loaded += OnNavigationWindowLoaded;
            navigationWindow.Show();
        }
    }*/

    private static void OnNavigationWindowLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not MainWindow navigationWindow)
        {
            return;
        }

        navigationWindow.NavigationView.Navigate(typeof(DashboardPage));
    }
}