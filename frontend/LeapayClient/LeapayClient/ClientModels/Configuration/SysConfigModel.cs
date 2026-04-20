using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.ClientModels.Configuration;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
[ObservableObject]
public partial class SysConfigModel
{
    public string? BackendPath { get; set; }

    public string? BackendUrl { get; set; } = "http://localhost:4455";
    
    public string? DatabaseUrl { get; set; } = "localhost";

    public int? DatabasePort { get; set; } = 3306;

    public string? DatabaseUsername { get; set; } = "Admin";
    
    public string? DatabaseName { get; set; } = "erp";
    
    public string? DatabasePassword { get; set; }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? DatabasePasswordU { get; set; }
    
    public string? CmdCommand { get; set; } = "/K mysqldump --add-drop-database --add-drop-table";
    
    public string? BackupPath { get; set; }
    
    public bool EnableBackup { get; set; }

    public int BackupMinutes { get; set; } = 30;
    
    public string? Token { get; set; }

    public bool MinusSellCheck { get; set; } = true;
    
    public bool ItemsHaveExpireDate { get; set; } = true;

    public string GetPassword()
    {
        return string.IsNullOrEmpty(DatabasePassword) ? "" : ApplicationHost.Shutdown(DatabasePassword);
    }
    
    public string CompileConstr()
    {
        return $"Server={DatabaseUrl};port={DatabasePort};Database={DatabaseName};Uid={DatabaseUsername};Pwd={GetPassword()}";
    }

    public string CompileCmdConstr()
    {
        return $"{CmdCommand} --user={DatabaseUsername} " +
               $"--password={GetPassword()} --databases {DatabaseName} > {BackupPath} & exit";
    }
    
    public bool VerifyBackendPath([NotNullWhen(true)] out FileInfo? file)
    {
        file = null;
        if (!Directory.Exists(BackendPath))
            return false;
        
        var files =  Directory.GetFiles(BackendPath);
        var appSettings = files.FirstOrDefault(f => f.Contains("appsettings.json"));

        if (appSettings is null)
            return false;
        
        file = new FileInfo(appSettings);
        
        return file.Exists;
    }
    
    public static void UpdateConfig(
        string filePath,
        string? newConnectionString = null,
        string? newT = null,
        bool? newMinusSellCheck = null)
    {
        var json = JsonNode.Parse(File.ReadAllText(filePath))!.AsObject();

        if (newConnectionString != null &&
            json["ConnectionStrings"] is JsonObject connStrings &&
            connStrings["MySqlDefaultConnection"] is not null)
        {
            connStrings["MySqlDefaultConnection"] = ApplicationHost.Start(newConnectionString);
        }

        if (!string.IsNullOrEmpty(newT) &&
            json["Token"] is JsonObject tokenObj &&
            tokenObj["Key"] is not null)
        {
            tokenObj["Key"] = ApplicationHost.Start(newT);
        }

        if (newMinusSellCheck.HasValue)
        {
            json["MinusSellCheck"] = newMinusSellCheck.Value;
        }

        File.WriteAllText(filePath, json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

    public bool VerifyDatabaseAuth()
    {
        return !(string.IsNullOrEmpty(DatabaseName)
                || string.IsNullOrEmpty(DatabasePassword)
                || string.IsNullOrEmpty(DatabaseUsername));
    }
}

