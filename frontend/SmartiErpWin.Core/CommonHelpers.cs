using System.Text.Json;

namespace SmartErpWin.Core;

public static class CommonHelpers
{
    public static bool TryGetFilesFromPath(string path, out string[] files)
    {
        files = Enumerable.Empty<string>().ToArray();

        try
        {
            files = Directory.GetFiles(path, "*.json");
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static async Task<T> DeepCopyValueOfReferenceTypeAsync<T>(
        T? referenceClass) where T: class, new()
    {
        var serialised = JsonSerializer.SerializeToUtf8Bytes(referenceClass ?? new());
        
        var stream = new MemoryStream(serialised);
        var deepCopy = await JsonSerializer.DeserializeAsync<T>(stream);

        return deepCopy ?? new();
    }
    
    public static T DeepCopyValueOfReferenceType<T>(
        T? referenceClass) where T: class, new()
    {
        var serialised = JsonSerializer.SerializeToUtf8Bytes(referenceClass ?? new());
        
        var stream = new MemoryStream(serialised);
        var deepCopy = JsonSerializer.Deserialize<T>(stream);

        return deepCopy ?? new();
    }

    public static T DeepCopy<T>(this T? referenceClass) where T: class, new()
        => DeepCopyValueOfReferenceType(referenceClass);

    public static string OpenLocalFile() => Constants.StatsRange + Constants.DateHelper;
}