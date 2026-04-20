using System.Text.Json;
using Microsoft.Win32;

namespace Fly.Shared.Common;

public static class Helpers
{
    public static T DeepCopyValueOfReferenceTypeShared<T>(
        T? referenceClass) where T: class, new()
    {
        var serialised = JsonSerializer.SerializeToUtf8Bytes(referenceClass ?? new());
        
        var stream = new MemoryStream(serialised);
        var deepCopy = JsonSerializer.Deserialize<T>(stream);

        return deepCopy ?? new();
    }

    public static T DeepCopyShared<T>(this T? referenceClass) where T: class, new()
        => DeepCopyValueOfReferenceTypeShared(referenceClass);

    public static string? GetMachineSid()
    {
        if (OperatingSystem.IsWindows())
        {
            return Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography",
                "MachineGuid",
                null
            )?.ToString();
        }
        
        return "";
    }
}