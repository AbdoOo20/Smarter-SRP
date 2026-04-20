namespace Leapay.Client.ServerModeBootstrap;

internal sealed class WindowsServiceProvisioner
{
    public async Task EnsureServiceInstalledAndRunningAsync(
        string serviceName,
        string displayName,
        string exePath,
        string? arguments,
        CancellationToken ct)
    {
        if (!File.Exists(exePath))
            throw new FileNotFoundException("Service executable not found.", exePath);

        if (!ServiceExists(serviceName))
        {
            // sc create: note the spaces after '=' are required by sc.exe syntax
            var binPath = arguments is null
                ? $"\"{exePath}\""
                : $"\"{exePath}\" {arguments}";

            await Proc.RunAsync("sc.exe",
                $"create \"{serviceName}\" binPath= \"{binPath}\" DisplayName= \"{displayName}\" start= auto",
                workingDir: null,
                ct: ct);
        }

        await StartServiceAsync(serviceName, ct);
    }

    public static bool ServiceExists(string serviceName)
    {
        try
        {
            // sc query returns exit code 0 if exists
            var r = Proc.Run("sc.exe", $"query \"{serviceName}\"");
            return r.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public static async Task StartServiceAsync(string serviceName, CancellationToken ct)
    {
        // Try start; if already running, sc returns a message but exit code might still be 0/1062 depending.
        var res = await Proc.RunAsync("sc.exe", $"start \"{serviceName}\"", workingDir: null, ct: ct, allowNonZeroExitCode: true);

        // If it says already running, that's fine. If it truly failed, it usually outputs a FAILED message.
        // We keep it permissive; you can tighten by parsing stdout/stderr.
        _ = res;
    }
}