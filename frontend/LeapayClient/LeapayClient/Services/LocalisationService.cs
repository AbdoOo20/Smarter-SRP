using System.Collections.Frozen;
using System.Globalization;
using Avalonia.Media;
using Leapay.Client.Assets.Locale;

namespace Leapay.Client.Services;

public class LocalisationService
{
    private readonly Dictionary<string, FrozenDictionary<string, string>> _localisations;

    private string? _currentLocale;
    
    public LocalisationService()
    {
        _localisations = new Dictionary<string, FrozenDictionary<string, string>>();
        
        LoadResources();
    }

    private void LoadResources()
    {
        _localisations.Add(English.LanguageCode, English.EnglishLocale);
        _localisations.Add(Arabic.LanguageCode, Arabic.ArabicLocale);
        _localisations.Add(Turkish.LanguageCode, Turkish.TurkishLocale);
        
        DetermineLocale();
    }

    private void DetermineLocale()
    {
        var locale = PreferencesService.Get(s => s.Locale);

        if (string.IsNullOrEmpty(locale))
        {
            var cultureName = CultureInfo.CurrentCulture.Name;

            _currentLocale = _localisations.FirstOrDefault(r 
                => cultureName.Contains(r.Key)).Key ?? DefaultLocale;

            PreferencesService.Set(s => s.Locale = _currentLocale);
            
            return;
        }

        _currentLocale = locale;
    }

    public string this[string? name]
    {
        get
        {
            name ??= "";
            
            if (_localisations.TryGetValue(_currentLocale, out var localisations)
                && localisations.TryGetValue(name, out var value))
            {
                return value;
            }

            return name;
        }
    }

    public FrozenDictionary<string, string> GetCurrentLocale()
    {
        return _localisations.TryGetValue(_currentLocale, out var localisations)
            ? localisations
            : _localisations[DefaultLocale];
    }

    public string this[string name, params object[] arguments] => this[name];

    public FlowDirection DetermineFlowDirection() =>
        _currentLocale?.Contains("ar") ?? false
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;

    private readonly Dictionary<string, string> _languageDisplayCodes = new()
    {
        { "en", "English" },
        { "ar", "العربية" },
        { "tr", "Türkçe" }
    };

    public string FromCodeToLanguage(string? code) =>
        _languageDisplayCodes.TryGetValue(code, out var value)
            ? value
            : string.Empty;

    public string FromLanguageToCode(string? language)
        => _languageDisplayCodes.ContainsValue(language)
            ? _languageDisplayCodes.FirstOrDefault(k => k.Value == language).Key
            : string.Empty;

    public List<string> GetAllLanguageDisplays()
        => _languageDisplayCodes.Values.ToList();

    public void UpdateLocale(string locale)
    {
        if (!_languageDisplayCodes.ContainsKey(locale))
        {
            Warning("Locale '{locale}' is not included in the supported languages.", locale);
            return;
        }

        PreferencesService.Set(s => s.Locale = locale);
    }
}
