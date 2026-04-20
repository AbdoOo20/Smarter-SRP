using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Fly.Framework.Shared.Common.Dtos;
using SmartErpWin.Core;
using Wpf.Ui.Appearance;

namespace SmartiErpWin.Client.ViewModels.Pages;

public partial class SettingsPageViewModel : BaseViewModel<DtoBase>
{
    private readonly AuthenticationStorageService _authenticationStorageService;

    [ObservableProperty]
    private ObservableCollection<string> _availableLanguages = new();

    [ObservableProperty]
    private string? _selectedLanguage;
    
    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    [ObservableProperty]
    private string? _selectedDisplayType;
    
    [ObservableProperty]
    private ObservableCollection<string> _displayTypes = new();

    [ObservableProperty]
    private bool _droplistMenu;
    
    public SettingsPageViewModel(HttpClient client, LocalisationService localeService,
        AuthenticationStorageService authenticationStorageService): base(client)
    {
        _authenticationStorageService = authenticationStorageService;
        InitBaseModel(localeService, null, null);

        var config = Config.Get;

        if (string.IsNullOrEmpty(config.Theme))
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
        }
        else
        {
            CurrentTheme = config.Theme == "dark" ? ApplicationTheme.Dark : ApplicationTheme.Light;
            OnChangeTheme($"theme_{config.Theme}");
        }
        
        Init();
    }

    private bool _hasInit;
    
    private void Init()
    {
        var languages = LocaleService?.GetAllLanguageDisplays();
        
        AvailableLanguages.Clear();
        
        foreach (var language in languages ?? [])
        {
            AvailableLanguages.Add(language);
        }

        SelectedLanguage = LocaleService?.FromCodeToLanguage(Config.Get?.Locale);
        DroplistMenu = Config.Get.DroplistMenu;

        _hasInit = true;
        
        DisplayTypes.Clear();
        
        DisplayTypes.Add(LocaleService["Left"]);
        DisplayTypes.Add(LocaleService["Top"]);
        SelectedDisplayType = Config.Get.PaneDisplayType is "left"
            ? LocaleService["Left"]
            : LocaleService["Top"];
    }

    partial void OnDroplistMenuChanged(bool oldValue, bool newValue)
    {
        //Config.Modify(c => c.DroplistMenu = newValue);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (!_hasInit)
            return;
        
        switch (e.PropertyName)
        {
            case nameof(SelectedLanguage):
                LocaleService.UpdateLocale(LocaleService.FromLanguageToCode(SelectedLanguage));
                
                _ = DialogueServices.ShowMessageAsync("LanguageChangedTitle",
                    "LanguageChangedMessage", localeService: LocaleService);
                break;
            case nameof(SelectedDisplayType):
            {
                var config = Config.Get!;
                var type = SelectedDisplayType;
                var @new = type == LocaleService["Left"] ? "left" : "top";

                if (config.PaneDisplayType == @new)
                    return;
                
                config.PaneDisplayType = @new;
                Config.SaveSettings(config);

                _ = DialogueServices.ShowMessageAsync("DisplayTypeChangedTitle", 
                    "DisplayTypeChangedMessage", localeService: LocaleService);

                // can't auto change due to library bug
                //SendSynchroniseToType(typeof(MainWindowPageViewModel));
                
                break;
            }
        }
    }
    
    [RelayCommand]
    public void Logout()
    {
        AuthenticationStorageService.DeleteToken();
        Process.Start(Environment.ProcessPath);
        Application.Current.Shutdown();
    }
    
    [RelayCommand]
    private void OnChangeTheme(string parameter)
    {
        var config = Config.Get;
        
        switch (parameter)
        {
            case "theme_light":
                if (CurrentTheme == ApplicationTheme.Light)
                    break;

                ApplicationThemeManager.Apply(ApplicationTheme.Light);
                CurrentTheme = ApplicationTheme.Light;

                config.Theme = "light";
                
                break;
            default:
                if (CurrentTheme == ApplicationTheme.Dark)
                    break;

                ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                CurrentTheme = ApplicationThemeManager.GetAppTheme();

                config.Theme = "dark";
                
                break;
        }

        Config.SaveSettings(config);
    }
}