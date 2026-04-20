using System.Text.Json;

namespace Leapay.Client.ServerModeBootstrap;

internal static class BackendConfigWriter
{
    /// <summary>
    /// Writes/updates a value at a JSON path like "ConnectionStrings:Default" in appsettings.json.
    /// Very small JSON writer: it loads, edits, writes back.
    /// </summary>
    public static async Task WriteDbConfigIntoAppSettingsJson(
        string appSettingsPath,
        string jsonPathColonSeparated,
        MySqlConnectionInfo info,
        CancellationToken ct)
    {
        var connStr = $"Server={info.Host};Port={info.Port};Database={info.Database};User={info.User};Password={info.Password};SslMode=None;";

        var json = File.Exists(appSettingsPath)
            ? await File.ReadAllTextAsync(appSettingsPath, ct)
            : "{}";

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement.Clone();

        // Convert to mutable via JsonNode (simplest without extra packages)
        var node = System.Text.Json.Nodes.JsonNode.Parse(root.GetRawText()) ?? new System.Text.Json.Nodes.JsonObject();

        SetByPath(node, jsonPathColonSeparated, connStr);

        var outJson = node.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(appSettingsPath, outJson, ct);
    }

    private static void SetByPath(System.Text.Json.Nodes.JsonNode node, string path, string value)
    {
        var parts = path.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0) throw new ArgumentException("Invalid JSON path.", nameof(path));

        System.Text.Json.Nodes.JsonObject obj = node as System.Text.Json.Nodes.JsonObject
                                                ?? throw new InvalidOperationException("Root JSON must be an object.");

        for (int i = 0; i < parts.Length - 1; i++)
        {
            var p = parts[i];
            if (obj[p] is not System.Text.Json.Nodes.JsonObject child)
            {
                child = new System.Text.Json.Nodes.JsonObject();
                obj[p] = child;
            }
            obj = child;
        }

        obj[parts[^1]] = value;
    }
}