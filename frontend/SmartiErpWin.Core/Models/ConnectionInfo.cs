using System.Text.Json.Serialization;

namespace SmartErpWin.Core.Models;

public class ConnectionInfo
{
    public string? DeviceName { get; set; }
    
    public string? DeviceId { get; set; }
    
    public string? CurrentConnectionIp { get; set; }
    
    public int CurrentConnectionPort { get; set; }
    
    public bool IsCurrentlyAvailable { get; set; }
    
    public DateTime LastConnectedTo { get; set; }
    
    public DateTime LastUpdated { get; set; }
    
    public string? BroadcastedMachineName { get; set; }
    
    public bool IsLocalDevice => Fly.Shared.Common.Helpers.GetMachineSid() == DeviceId;
    
    [JsonIgnore]
    public bool IsLan => !IsLocalDevice && !IsOnline;
    
    public bool IsOnline { get; set; }
    
    public string ConnectionUrl => $"https://{CurrentConnectionIp}:{CurrentConnectionPort}";

    public string? UiDeviceName => IsLocalDevice ? $"{Constants.TranslateSignature}ThisDevice" : DeviceName ?? BroadcastedMachineName;
    
    public bool ShowLastConnectedTo => LastConnectedTo != default;
   
    public string LastConnectedToText => LastConnectedTo == default ? string.Empty : LastConnectedTo.ToString("g");
}
