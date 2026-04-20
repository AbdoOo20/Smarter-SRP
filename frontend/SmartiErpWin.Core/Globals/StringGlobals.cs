using SmartErpWin.Core.Models;

namespace SmartErpWin.Core.Globals;

public static class StringGlobals
{
    public static string? BackendUrl => ActiveConnection?.ConnectionUrl;
    
    public static ConnectionInfo? ActiveConnection { get; set; }

    public static string[] Arguments { get; set; } = [];
    
    public static bool IsBackendServiceRunning { get; set; }
    
    public static bool IsMsix { get; set; }
}