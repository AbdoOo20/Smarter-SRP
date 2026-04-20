using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Collections;
using Avalonia.Styling;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SukiUI;
using SukiUI.Controls;
using SukiUI.Enums;
using SukiUI.Models;
using SukiUI.Theme.Shadcn;
using Colors = Avalonia.Media.Colors;

namespace Leapay.Client.Features.Theming;

public partial class ThemingViewModel : BasePageViewModel<DtoBase>
{
    public Action<SukiBackgroundStyle>? BackgroundStyleChanged { get; set; }
    public Action<bool>? BackgroundAnimationsChanged { get; set; }
    public Action<bool>? BackgroundTransitionsChanged { get; set; }
    public Action<string?>? CustomBackgroundStyleChanged { get; set; }

    [ObservableProperty]
    public partial StaticComboVm<SukiBackgroundStyle> BackgroundStyles { get; set; } = new();
    
    public IAvaloniaReadOnlyList<SukiColorTheme> AvailableColors { get; }
    public IAvaloniaReadOnlyList<SukiBackgroundStyle> AvailableBackgroundStyles { get; }
    
    public IAvaloniaReadOnlyList<string> CustomShaders { get; } = new AvaloniaList<string> { "Space", "Weird", "Clouds" };

    private readonly SukiTheme _theme = SukiTheme.GetInstance();

    [ObservableProperty]
    public partial bool IsLightTheme { get; set; }

    [ObservableProperty]
    public partial SukiBackgroundStyle BackgroundStyle { get; set; }
    
    [ObservableProperty]
    public partial bool BackgroundAnimations { get; set; }

    [ObservableProperty]
    public partial bool BackgroundTransitions { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<string> AvailableLanguages { get; set; } = new();

    [ObservableProperty]
    public partial string? SelectedLanguage { get; set; }

    private string? _customShader = null;
    
    public ThemingViewModel()
    {
        Information("Using Preference service...");
        BackgroundStyles.Set(PreferencesService.Get(s => s.BackgroundStyle),
            OnBackgroundStyleChanged);
        Information("Used Preference service...");

        AvailableBackgroundStyles = new AvaloniaList<SukiBackgroundStyle>(Enum.GetValues<SukiBackgroundStyle>());
        _theme.AddColorTheme(new SukiColorTheme("Brown", Colors.SaddleBrown, Colors.Beige));
        //Shadcn.Configure(Application.Current, Application.Current.ActualThemeVariant, false);
        AvailableColors = _theme.ColorThemes;
        
        /*_theme.OnBaseThemeChanged += variant =>
            IsLightTheme = variant == ThemeVariant.Light;*/
        
        SetProperties("Theming", MaterialIconKind.PaletteOutline);

        Init();
    }

    private static bool IsLightThemeFromSettings()
    {
        return PreferencesService.Get(l => l.Theme) == "light";
    }

    public void Init()
    {
        SetTheme();

        BackgroundTransitions = PreferencesService.Get(s => s.BackgroundTransitions);
        BackgroundAnimations = PreferencesService.Get(s => s.BackgroundAnimations);
        OnBackgroundStyleChanged(PreferencesService.Get(s => s.BackgroundStyle));
        
        SukiTransitioningContentControl.AreAnimationsEnabled = BackgroundTransitions;
        
        var languages = LocaleService?.GetAllLanguageDisplays();
        
        AvailableLanguages.Clear();
        
        foreach (var language in languages ?? [])
        {
            AvailableLanguages.Add(language);
        }

        SelectedLanguage = LocaleService?.FromCodeToLanguage(PreferencesService.Get(l => l.Locale));
    }

    private void SetTheme()
    {
        _theme.ChangeBaseTheme(IsLightThemeFromSettings() ? ThemeVariant.Light : ThemeVariant.Dark);
        _theme.ChangeColorTheme(AvailableColors.FirstOrDefault(a
            => a.DisplayName == PreferencesService.Get(s => s.Colour)) ?? AvailableColors[0]);
        
        IsLightTheme = _theme.ActiveBaseTheme == ThemeVariant.Light;
        SetLiveChartsTheme(IsLightTheme);
    }

    private void SetLiveChartsTheme(bool light)
    {
        LiveCharts.Configure(c =>
        {
            if (light) c.AddLightTheme();
            else c.AddDarkTheme();
        });
    }

    private bool CheckShad()
    {
        var colour = PreferencesService.Get(s => s.Colour);

        if (colour == "Black")
        {
            Shadcn.Configure(Application.Current, Application.Current.ActualThemeVariant);
            IsLightTheme = false;

            return true;
        }

        return false;
    }


    partial void OnSelectedLanguageChanged(string? value)
    {
        if (value == null)
            return;
        
        var code = LocaleService.FromLanguageToCode(SelectedLanguage);
        LocaleService.UpdateLocale(code);
        
        PreferencesService.Set(l => l.Locale = code);
    }

    partial void OnIsLightThemeChanged(bool value)
    {
        _theme.ChangeBaseTheme(value ? ThemeVariant.Light : ThemeVariant.Dark);
        PreferencesService.Set(l => l.Theme = value ? "light" : "dark");
        SetLiveChartsTheme(value);
    }
    
    [RelayCommand]
    private async Task SwitchToColorTheme(SukiColorTheme colorTheme)
    {
        if (colorTheme.DisplayName is "White" && IsLightTheme
            || colorTheme.DisplayName is "Black" && !IsLightTheme)
        {
            //await DialogueServices.ShowMessageAsync(this["Problem"], this["IncompatibleColours"]);
            return;
        }
        
        PreferencesService.Set(s => s.Colour = colorTheme.DisplayName);

        _theme.ChangeColorTheme(colorTheme);
    }

    partial void OnBackgroundStyleChanged(SukiBackgroundStyle value)
    {
        BackgroundStyleChanged?.Invoke(value);
        PreferencesService.Set(s => s.BackgroundStyle = value);
    } 

    partial void OnBackgroundAnimationsChanged(bool value)
    {
        BackgroundAnimationsChanged?.Invoke(value);
        PreferencesService.Set(s => s.BackgroundAnimations = value);
    }

    partial void OnBackgroundTransitionsChanged(bool value)
    {
        BackgroundTransitionsChanged?.Invoke(value);
        PreferencesService.Set(s => s.BackgroundTransitions = value);
        SukiTransitioningContentControl.AreAnimationsEnabled = value;
    }

    [RelayCommand]
    private void TryCustomShader(string shaderType)
    {
        _customShader = _customShader == shaderType ? null : shaderType;
        CustomBackgroundStyleChanged?.Invoke(_customShader);
    }

    [RelayCommand]
    private void OnLogout()
    {
        if (OperatingSystem.IsWindows())
        {
            AuthenticationStorageService.DeleteToken();
            Process.Start(Environment.ProcessPath);
            App.Shutdown();
        }
    }
}