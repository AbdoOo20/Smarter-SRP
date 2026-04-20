using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Avalonia.Collections;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Styling;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;
using Leapay.Client.Features;
using Leapay.Client.Features.CustomTheme;
using Leapay.Client.Features.Theming;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UdpDiscovery;
using Leapay.Client.UserInterface.Common.SysConfigPage.Internals;
using Leapay.Client.UserInterface.Routing;
using Leapay.Client.Utilities;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Globals;
using SukiUI.Controls;
using SukiUI.Dialogs;
using SukiUI.Enums;
using SukiUI.Models;
using SukiUI.Theme.Shadcn;
using SukiUI.Toasts;
using Velopack.Locators;

namespace Leapay.Client;

public partial class MainWindowViewModel : BaseRefreshablePageViewModel<DtoBase>
{
    public PageNavigationService PageNavigationService { get; }

    public IAvaloniaReadOnlyList<SukiColorTheme> Themes { get; }

    public AvaloniaList<string> ContactInformation { get; set; } = [];

    public IAvaloniaReadOnlyList<SukiBackgroundStyle> BackgroundStyles { get; }
    
    public ISukiToastManager ToastManager { get; }
    
    public ISukiDialogManager DialogManager { get; }

    public IClipboard? Clipboard { get; set; }
    
    [ObservableProperty]
    public partial ThemeVariant BaseTheme { get; set; }
    
    [ObservableProperty]
    public partial INavigationPage? ActivePage { get; set; }
    
    [ObservableProperty]
    public partial bool WindowLocked { get; set; }
    
    [ObservableProperty]
    public partial bool TitleBarVisible { get; set; } = true;

    [ObservableProperty]
    public partial SukiBackgroundStyle BackgroundStyle { get; set; } = SukiBackgroundStyle.Bubble;
    
    [ObservableProperty]
    public partial bool AnimationsEnabled { get; set; }

    [ObservableProperty]
    public partial string? CustomShaderFile { get; set; }

    [ObservableProperty]
    public partial string UpdateInfo { get; set; }
    
    [ObservableProperty]
    public partial bool TransitionsEnabled { get; set; }

    [ObservableProperty]
    public partial bool ShowTitleBar { get; set; } = true;

    [ObservableProperty]
    public partial bool ShowBottomBar { get; set; } = true;
    
    [ObservableProperty]
    public partial bool ShouldShowUpdating { get; set; } = false;
    
    [ObservableProperty]
    public partial bool ShouldShowSetup { get; set; } = false;
    
    [ObservableProperty]
    public partial bool IsUpdating { get; set; } = false;

    [ObservableProperty]
    public partial int UpdatePercentage { get; set; } = 0;
    
    public ThemingViewModel Theming { get; set; }

    public void GoBack()
    {
        PageNavigationService.RequestNavigation(PageNavigationService.PreviousPage);
    }
    
    public MainWindowViewModel(PageNavigationService pageNavigationService,
        ISukiToastManager toastManager,
        ISukiDialogManager dialogManager)
    {
        ToastManager = toastManager;
        DialogManager = dialogManager;
        ApplicationTitle = this["ApplicationTitle"];
        
        PageNavigationService = pageNavigationService;
        LoadContactInformation();
        BackgroundStyles = new AvaloniaList<SukiBackgroundStyle>(Enum.GetValues<SukiBackgroundStyle>());
        pageNavigationService.NavigationRequested += OnNavigationRequested;
        pageNavigationService.StandaloneNavigationRequested += OnStandaloneNavigationRequested;
        PreferencesService.Set(s => s.HasUserSelectedSystemType = false);
        PreferencesService.Set(s => s.IsBackendFullySetup = false);
        ConnectionInfoText = this["SelectAServerOrUseLocal"];
        IsCreateOwnBackendVisible = PreferencesService.Get(s => !s.IsBackendFullySetup);
        
        Themes = Theme.ColorThemes;
        BaseTheme = Theme.ActiveBaseTheme;
    }

    private void OnStandaloneNavigationRequested(TabbedViewCardModel tab)
    {
        var tabVm = App.GetService<TabbedNavigationalPageViewModel>();
        tabVm.OnSynchronisationRequestedAsync(tab);
    }

    public void MutateSideMenu(Action<SukiSideMenu> action)
    {
        if (_sideMenu is null)
        {
            Warning("Side menu was null while trying to mutate.");
            return;
        }
        
        action(_sideMenu);
    }
    
    private void OnNavigationRequested(Type pageType)
    {
        if (_sideMenu is null)
        {
            Warning("Requested navigation when side menu is null");
            return;
        }

        /*PageNavigationService.PreviousPage = (_sideMenu.SelectedItem as SukiSideMenuItem)?.PageContent.GetType();
        
        // Below is a static and messy implementation of retrieving the actual page.
        // Should be refined later.
        var page = _sideMenu.Items.OfType<SukiSideMenuItem>()
            .SelectMany(i => i.Items.OfType<SukiSideMenuItem>())
            .FirstOrDefault(s => s.PageContent.GetType() == pageType)
                   ?? _sideMenu.Items.OfType<SukiSideMenuItem>()
            .FirstOrDefault(s => s.PageContent.GetType() == pageType);

        // Do not navigate to the page if it is hidden due to permissions, unless it is an auto hidden
        // page that is independent of permissions.
        if (!page.IsVisible && !_autoHiddenPages.Contains(page.Name))
            return;
        
        if (page is null)
        {
            Warning("Requested navigation to {0} when page is null", pageType);
            return;
        }

        if (ActivePage?.GetType() == pageType) return;*/

        var view = ContentHelper.GetViewByType(pageType);
        _sideMenu.SelectedItem = view?.Item;
    }

    private void LoadContactInformation()
    {
        ContactInformation.Clear();
        ContactInformation.AddRange([
            TextHelper.FormatWithLtr(this["WebsiteContact"], WebsiteName),
            TextHelper.FormatWithLtr(this["EmailContact"], WebsiteEmail),
            TextHelper.FormatWithLtr(this["CommunicationContact"], CommContact),
            TextHelper.FormatWithLtr(this["CustomerSupportContact"], CustomerSupportContact),
            TextHelper.FormatWithLtr(this["TechnicalSupportContact"], TechnicalSupportContact)/*,
            TextHelper.FormatWithLtr(this["ManagementSupportContact"], ManagementContact)*/]);
    }
    
    [RelayCommand]
    private void ToggleAnimations()
    {
        AnimationsEnabled = !AnimationsEnabled;
    }

    [RelayCommand]
    private void ToggleTransitions()
    {
        TransitionsEnabled = !TransitionsEnabled;
    }

    [RelayCommand]
    private void ToggleBaseTheme() =>
        Theme.SwitchBaseTheme();

    public void ChangeTheme(SukiColorTheme theme) =>
        Theme.ChangeColorTheme(theme);

    [RelayCommand]
    private void ShadCnMode()
    {
        Shadcn.Configure(Application.Current, Application.Current.ActualThemeVariant);
    }

    [RelayCommand]
    private void CreateCustomTheme()
    {
        DialogManager.CreateDialog()
            .WithViewModel(dialog => new CustomThemeDialogViewModel(Theme, dialog))
            .TryShow();
    }

    [RelayCommand]
    private void ToggleWindowLock()
    {
        WindowLocked = !WindowLocked;
    }

    [RelayCommand]
    private void ToggleTitleBackground()
    {
        ShowTitleBar = !ShowTitleBar;
        ShowBottomBar = !ShowBottomBar;
    }

    private int _currentClicks;
    
    [RelayCommand]
    private async Task CopyContactInfoAsync(string contactInfo)
    {
        if (Clipboard is null)
        {
            Warning("Platform-impl clipboard was null while trying to copy " +
                    "a contact info card");
            return;
        }

        if (_currentClicks > 5)
        {
            _currentClicks = 0;
            _ = OpenConfig();
        }
        
        var info = contactInfo.Split(':', StringSplitOptions.TrimEntries);
        var title = info[0];
        var contact = info[1];

        _currentClicks++;
        
        await Clipboard.SetTextAsync(contact);
        ToastManager.CreateSimpleInfoToast().WithTitle(title)
            .WithContent(string.Format(this["CopySuccessfulParameter"], contact))
            .Queue();
    }
    
    [RelayCommand]
    private void ToggleTitleBar()
    {
        TitleBarVisible = !TitleBarVisible;
    }

    [RelayCommand]
    private void ToggleRightToLeft() => Theme.IsRightToLeft = !Theme.IsRightToLeft;

    [RelayCommand]
    private static void OpenUrl(string url) => UrlUtilities.OpenUrl(url);

    partial void OnBackgroundStyleChanged(SukiBackgroundStyle value) =>
        Theming.BackgroundStyle = value;

    partial void OnAnimationsEnabledChanged(bool value) =>
        Theming.BackgroundAnimations = value;

    partial void OnTransitionsEnabledChanged(bool value) =>
        Theming.BackgroundTransitions = value;

    public void AuthenticateRequestsForFeatureBase(string token)
    {
        AuthenticateRequests(token);
    }
    
    public async Task VerifyConfig(KeyEventArgs e)
    {
        if (e.Key == Key.F5 &&
            e.KeyModifiers.HasFlag(KeyModifiers.Control) &&
            e.KeyModifiers.HasFlag(KeyModifiers.Shift) &&
            e.KeyModifiers.HasFlag(KeyModifiers.Alt))
        {
            await OpenConfig();
            e.Handled = true;
        }
        
        if (e.Key == Key.F6 &&
            e.KeyModifiers.HasFlag(KeyModifiers.Control) &&
            e.KeyModifiers.HasFlag(KeyModifiers.Shift) &&
            e.KeyModifiers.HasFlag(KeyModifiers.Alt))
        {
            if (!VelopackLocator.IsCurrentSet)
                return;

            var assets = VelopackLocator.Current.AppContentDir;
            
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = assets,
                UseShellExecute = true
            });
            
            e.Handled = true;
        }
    }

    private static async Task OpenConfig()
    {
        await ContentHelper.ShowCrossPlatformContentAsync(new SysConfigInitialEntryWindow(new(new(), EntryType.Add, typeof(MainWindow))),
            new("SysConfig", false, false, false, false));
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        ConnectionsUpdated(args.FirstOrDefault());
        
        return base.OnSynchronisationRequestedAsync(args);
    }
}