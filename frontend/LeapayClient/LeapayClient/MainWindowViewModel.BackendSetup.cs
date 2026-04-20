using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Avalonia.Collections;
using Avalonia.Threading;
using Leapay.Client.ClientModels.Configuration;
using Leapay.Client.Features.Theming;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.ServerModeBootstrap;
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
    public partial string SetupInfoText { get; set; }
    
    [ObservableProperty]
    public partial bool IsCurrentlySettingUpBackend { get; set; }
    
    [ObservableProperty]
    public partial bool IsCreateOwnBackendVisible { get; set; }
    
    public void SwitchToBackendSetup()
    {
        ShouldShowUi = false;
        ShouldShowLoading = false;
        ShouldShowLogin = false;
        ShouldShowUpdating = false;
        IsUserSelectingBackend = false;
        ShouldShowSetup = true;

        LoginInfoText = string.Empty;
    }

    [RelayCommand]
    private void SelectBackendType(object type)
    {
        var ty = type.ToString();
        var full = ty == "full";
        
        PreferencesService.Set(s =>
        {
            s.HasUserSelectedSystemType = true;
            s.SystemType = full ? SystemType.FrontendAndBackend : SystemType.Frontend;
        });

        ProceedWithSetup();
    }

    private void ProceedWithSetup()
    {
        if (PreferencesService.Get(s => s.SystemType == SystemType.FrontendAndBackend))
        {
            InitializeBackendSetup();
            return;
        }

        ConnectionInfoText = this["ConnectToABackendOrCreateOwnBackend"];
        SwitchToBackendSelection();
    }

    public void InitializeBackendSetup()
    {
        var opt = new BootstrapOptions(
            MySql: new MySqlOptions(
                ManagedInstanceRootDir: @"C:\ProgramData\MyApp\mysql",
                Version: "8.0.36",
                ZipDownloadUrl: new Uri("https://dev.mysql.com/get/Downloads/MySQL-8.0/mysql-8.0.36-winx64.zip"),
                Port: 53162,
                ServiceName: "LeaPayMySql",
                DatabaseName: "LeaPayErp",
                AppUserName: "LeaPayBackend",
                AppUserPassword: Convert.ToHexString(RandomNumberGenerator.GetBytes(32)),
                BindOnLan: false,
                OpenMySqlPortOnLan: false
            ),
            BackendExePath: Path.Combine(Environment.CurrentDirectory, "Back", "Fly.WebApi.exe"),
            BackendServiceName: "LeaPayBackend",
            BackendServiceDisplayName: "LeaPay Backend",
            BackendListenPort: 5000,
            BackendExeArguments: null,
            BackendPath: Path.Combine(Environment.CurrentDirectory, "Back")
        );
        
        PreferencesService.Set(s => s.IsBackendFullySetup = true);
        IsCreateOwnBackendVisible = false;
    }

    [RelayCommand]
    private async Task OnInitializeBackendSetupPrompt()
    {
        if (!await DialogueServices.ConfirmAsync("CreateBackendConfirmTitle", "CreateBackendConfirmMessage"))
            return;
        
        SwitchToBackendSetup();
        InitializeBackendSetup();
    }
}