using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Fly.Framework.Server.Common.Localization;

public interface ILocalizationService
{
    string? this[string key] { get; }
    string CurrentLocale { get; }
    string GetText(string key);
    void SetCurrentLanguage(string? locale = null);

    string GetFormatedText(string key, params string[] values);
}

public class LocalizationService : ILocalizationService
{
    public const string DefaultLocale = "ar";
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string localeDir;
    Dictionary<string, string>? locales = null;
    private string? localeDefaultName = null;

    public string CurrentLocale => localeDefaultName;

    public string? this[string key] => GetText(key);

    public string GetFormatedText(string key, params string[] values)
    {
        var txt = this[key] ?? "";
        return string.Format(txt, values);
    }


    public LocalizationService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        localeDir = Path.Combine(_webHostEnvironment.ContentRootPath, "locales");
        Directory.CreateDirectory(localeDir);
    }

    public void SetCurrentLanguage(string? locale = null)
    {
        localeDefaultName = locale ?? DefaultLocale;
        var localeFile = Path.Combine(localeDir, localeDefaultName + ".json");
        if (!File.Exists(localeFile)) return;

        var content = File.ReadAllText(localeFile);
        locales = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
    }

    public string GetText(string key)
    {
        if (locales is null)
            return string.Empty;
        if (locales.TryGetValue(key, out string? value))
            return value;
        return key;
    }

}