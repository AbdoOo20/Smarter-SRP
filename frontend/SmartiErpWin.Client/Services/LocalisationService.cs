using System.Globalization;
using System.IO;
using System.Text.Json;
using Humanizer;
using Microsoft.Extensions.Localization;
using SmartErpWin.Core;

namespace SmartiErpWin.Client.Services;

public class LocalisationService : IStringLocalizer
{
    private static readonly string ResourcesPath = Path.Combine(Environment.CurrentDirectory, "locale");

    private readonly Dictionary<string, Dictionary<string, string>> _localisations;

    private string? _currentLocale;
    
    public HttpClient LocalisedHttpClient { get; set; }
    
    public LocalisationService(HttpClient httpClient)
    {
        _localisations = new Dictionary<string, Dictionary<string, string>>();
        LocalisedHttpClient = httpClient;
        
        LoadResources();
    }

    private void LoadResources()
    {
        DetermineLocale();

        if (!Directory.Exists(ResourcesPath) || !CommonHelpers.TryGetFilesFromPath(ResourcesPath, out var files))
        {
            _ = DialogueServices.ShowMessageAsync("Error", "No resource files found.",
                "Close", true);
            return;
        }

        foreach (var file in files)
        {
            var culture = Path.GetFileNameWithoutExtension(file);
            var jsonContent = File.ReadAllText(file);
            var localisations = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
            _localisations[culture] = localisations;
        }
    }

    private void DetermineLocale()
    {
        var config = Config.Get;

        if (string.IsNullOrEmpty(config?.Locale))
        {
            var cultureName = CultureInfo.CurrentCulture.Name;

            _currentLocale = cultureName.Contains("en") ? "en" : "ar";

            if (config != null)
            {
                config.Locale = _currentLocale;
                Config.SaveSettings(config);
            }
        }

        _currentLocale = config?.Locale;
    }

    public LocalizedString this[string? name]
    {
        get
        {
            name ??= "";
            
            if (_localisations.TryGetValue(_currentLocale, out var localisations)
                && localisations.TryGetValue(name, out var value))
            {
                return new LocalizedString(name, value);
            }

            return new LocalizedString(name, Config.Get?.AutoLocalise
                                             ?? false ? name.Humanize(LetterCasing.Sentence) : name, true);
        }
    }

    public Dictionary<string, string> GetCurrentLocale()
    {
        return _localisations.TryGetValue(_currentLocale, out var localisations)
            ? localisations
            : _localisations[DefaultLocale];
    }

    public LocalizedString this[string name, params object[] arguments] => this[name];

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        if (!_localisations.TryGetValue(_currentLocale, out var localizations))
            yield break;
        
        foreach (var localisation in localizations)
        {
            yield return new LocalizedString(localisation.Key, localisation.Value);
        }
    }

    public FlowDirection DetermineFlowDirection() =>
        _currentLocale.Contains("ar")
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;

    private readonly Dictionary<string, string> _languageDisplayCodes = new()
    {
        { "en", "English" },
        { "ar", "العربية" }
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
            return;

        var config = Config.Get;

        if (config is null)
            return;
        
        config.Locale = locale;
        Config.SaveSettings(config);

        _currentLocale = locale;
    }
}
