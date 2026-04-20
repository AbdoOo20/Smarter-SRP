using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Threading;
using Leapay.Client.Features.Theming;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Globals;
using SmartErpWin.Core.Models;
using Svg;

namespace Leapay.Client;

public partial class MainWindowViewModel
{
    [ObservableProperty]
    public partial ObservableCollection<ConnectionInfo> Connections { get; set; } = [];

    [ObservableProperty]
    public partial bool IsUserSelectingBackend { get; set; }
    
    [ObservableProperty]
    public partial string ConnectionInfoText { get; set; }
    
    public void SwitchToBackendSelection()
    {
        ShouldShowUi = false;
        ShouldShowLoading = false;
        ShouldShowLogin = false;
        ShouldShowUpdating = false;
        IsUserSelectingBackend = true;
        ShouldShowSetup = false;
        LoginInfoText = string.Empty;
    }

    private void UpdateConnections()
    {
        PreferencesService.Get(s => s.Connections).Observe(Connections);
    }
    
    [RelayCommand]
    private async Task SelectBackend(ConnectionInfo connection)
    {
        if (!connection.IsCurrentlyAvailable)
            return;

        StringGlobals.ActiveConnection = connection;
        SwitchToSignIn();
    }

    private void ConnectionsUpdated(object argsO)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            try
            {
                ConnectionInfoText = this["SelectAServerOrUseLocal"];
                UpdateConnections();

                if (argsO is not string args)
                    return;
                
                if (args == CodeBook.NoData)
                {
                    ConnectionInfoText = this["NoConnectionsAvailableCheckBackend"];
                    SwitchToBackendSelection();
                }
                else if (args == CodeBook.Unavailable)
                {
                    ConnectionInfoText = this["MostRecentConnectionNotAvailableSelectBackend"];
                    SwitchToBackendSelection();
                }
            }
            catch (Exception e)
            {
                Error(e.ToString());
            }
        });
    }
    
    public void EnterBusyModeForServerDisconnected()
    {
        if (IsUserSelectingBackend)
            return;

        if (ShouldShowLogin)
        {
            SwitchToBackendSelection();
            return;
        }
        
        App.FocusMainWindow();
        foreach (var window in App.GetOpenChildWindows())
        {
            window.IsEnabled = false;
        }
        
        BusyText = this["ConnectingToServer"];
        SecondaryBusyText = this["ConnectionToServerWasSevered"];
        CommandBusyText = this["LogoutAndChooseAnotherServer"];
        CommandBusyAction = new RelayCommand(() =>
        {
            App.GetService<ThemingViewModel>().LogoutCommand.Execute(null);
        });
        IsBusy = true;
    }
    
    public void ClearBusyMode()
    {
        foreach (var window in App.GetOpenChildWindows())
        {
            window.IsEnabled = true;
        }
        
        BusyText = string.Empty;
        SecondaryBusyText = string.Empty;
        CommandBusyText = string.Empty;
        CommandBusyAction = null;
        IsBusy = false;
    }

}