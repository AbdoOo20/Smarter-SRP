using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Leapay.Client.ClientModels.Configuration;
using Leapay.Windows;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.Services;

public static class PreferenceStore
{
    private static string ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\LeapayStateCore\";
    private static string ConfigFile = ConfigDirectory + "pref.json";
   
    public static SettingsModel? Get => UpdateSettings();
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };
    
    private static SettingsModel? UpdateSettings()
    {
        Directory.CreateDirectory(ConfigDirectory);

        if (!File.Exists(ConfigFile))
            using (_ = File.Create(ConfigFile)) { }

        var content = File.ReadAllText(ConfigFile);

        SettingsModel? config = null;
        try
        {
            var rawContent = Convert.FromBase64String(content);
            var s = Cryptography.Unprotect(rawContent);

            config = string.IsNullOrEmpty(s)
                ? SaveSettings(new SettingsModel())
                : JsonSerializer.Deserialize<SettingsModel>(s, JsonSerializerOptions);
        }
        catch (Exception x)
        {
            Error("Either the config file is corrupt, the user is using a config file from an older version, " +
                  "or it may have been tampered with. Resetting settings...\n{0}", x.Message);
            return SaveSettings(new SettingsModel());
        }
        
        return config;
    }

    public static SettingsModel? SaveSettings(SettingsModel? settings)
    {
        if (!File.Exists(ConfigFile))
            using (_ = File.Create(ConfigFile)) { }

        var content = JsonSerializer.Serialize(settings, JsonSerializerOptions);
        var cr = Cryptography.Protect(content);
        var base64 = Convert.ToBase64String(cr);
        
        File.WriteAllText(ConfigFile, base64);

        return settings;
    }
    
    public static void Modify(Action<SettingsModel> config)
    {
        var configEntity = Get;

        if (configEntity is null)
            return;
        
        config.Invoke(configEntity);

        SaveSettings(configEntity);
    }
}