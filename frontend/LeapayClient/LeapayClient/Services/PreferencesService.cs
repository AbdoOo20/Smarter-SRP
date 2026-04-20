using Avalonia.Preferences;
using Leapay.Client.ClientModels.Configuration;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.Services;

public static class PreferencesService
{
    private static Preferences? _pref;

    private const string SettingsKey = "settings";
    
    public static SettingsModel? GetSettings()
    {
        return PreferenceStore.Get;
    }

    public static SettingsModel UpdateSettings(SettingsModel model)
    {
        PreferenceStore.SaveSettings(model);
        return model;
    }

    public static void Set(Action<SettingsModel> model)
    {
        var current = GetSettings();
        model.Invoke(current);

        UpdateSettings(current);
    }

    public static string? GetS(string key)
    {
        _pref ??= App.GetService<Preferences>();

        try
        {
            return ApplicationHost.Shutdown(_pref?.Get<string>(key, null));
        }
        catch
        {
            return null;
        }
    }

    public static bool? SetS(string key, string value)
    {
        _pref ??= App.GetService<Preferences>();

        return _pref?.Set(key, ApplicationHost.Start(value));
    }

    public static T Get<T>(Func<SettingsModel, T> selector)
    {
        return selector.Invoke(GetSettings());
    }
}