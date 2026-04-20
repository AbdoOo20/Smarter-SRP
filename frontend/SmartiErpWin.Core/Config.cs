using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using SmartErpWin.Core.Helpers;

namespace SmartErpWin.Core;

public static class Config
{
    private static string ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\LeapayStateCore\";
    private static string ConfigFile = ConfigDirectory + "config.json";
   
    public static ConfigEntity? Get => UpdateSettings();
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };
    
    private static ConfigEntity? UpdateSettings()
    {
        Directory.CreateDirectory(ConfigDirectory);

        if (!File.Exists(ConfigFile))
            using (_ = File.Create(ConfigFile)) { }

        var content = File.ReadAllText(ConfigFile);

        var config = string.IsNullOrEmpty(content) ? SaveSettings(new ConfigEntity())
            : JsonSerializer.Deserialize<ConfigEntity>(content, JsonSerializerOptions);

        config?.ClientConfig.LoadImage();

        return config;
    }

    public static ConfigEntity? SaveSettings(ConfigEntity? settings)
    {
        if (!File.Exists(ConfigFile))
            using (_ = File.Create(ConfigFile)) { }

        var content = JsonSerializer.Serialize(settings, JsonSerializerOptions);
        File.WriteAllText(ConfigFile, content);

        return settings;
    }
    
    public static void Modify(Action<ConfigEntity> config)
    {
        var configEntity = Get;

        if (configEntity is null)
            return;
        
        config.Invoke(configEntity);

        SaveSettings(configEntity);
    }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class ConfigEntity
{
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
    
    [JsonPropertyName("temporaryJwt")] 
    public string? TemporaryJwt { get; set; }

    [JsonPropertyName("theme")]
    public string? Theme { get; set; } = "dark";
    
    [JsonPropertyName("autoLocalize")]
    public bool AutoLocalise { get; set; }

    [JsonPropertyName("paneDisplayType")]
    public string PaneDisplayType { get; set; } = "left";
    
    [JsonPropertyName("backendUrl")]
    public string? BackendUrl { get; set; } = "http://localhost:4455";
    
    [JsonPropertyName("INTERNAL_useWindowsHello")]
    public bool UseWindowsHello { get; set; }
    
    [JsonPropertyName("serialNumberVisible")]
    public bool SerialNumberVisible { get; set; }

    [JsonPropertyName("financialInternalNotesVisible")]
    public bool FinancialInternalNotesVisible { get; set; }

    [JsonPropertyName("droplistMenu")]
    public bool DroplistMenu { get; set; } = true;

    [JsonPropertyName("flist")]
    public List<string> FavouritedItems { get; set; } = [];
    
    [JsonPropertyName("clientConfig")]
    public ClientInfo ClientConfig { get; set; } = new();
}

public class ClientInfo
{
    public ClientInfo()
    {
        LoadImage();
    }

    //public byte[] ImageX { get; set; }
    public string ImagePath { get; set; }

    [JsonIgnore] public byte[] Image { get; set; }

    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Decription { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public string Phone3 { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }

    public void LoadImage()
    {
        if (ImagePath is null)
            return;

        if (!File.Exists(ImagePath))
            return;
        
        Image = File.ReadAllBytes(ImagePath);
    }

    public void MoveImage(string imagePath)
    {
        var dir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "images"));

        if (!File.Exists(imagePath))
        {
            return;
        }

        var newPath = Path.Combine(dir.FullName, new FileInfo(imagePath).Name);
        
        if(File.Exists(newPath))
            File.Delete(newPath);
        
        File.Copy(imagePath, newPath);
        var logo = FileHelper.RenameFilePreservingExtension(newPath, "logo");
        
        ImagePath = logo;
    }
}

