namespace Leapay.Client.ServerModeBootstrap;

internal sealed class FirewallProvisioner
{
    public async Task EnsureInboundTcpAllPortsAllowedAsync(
        string ruleName,
        CancellationToken ct)
    {
        await Proc.RunAsync(
            "netsh",
            $"advfirewall firewall delete rule name=\"{ruleName}\"",
            workingDir: null,
            ct: ct,
            allowNonZeroExitCode: true);

        await Proc.RunAsync(
            "netsh",
            $"advfirewall firewall add rule name=\"{ruleName}\" dir=in action=allow protocol=TCP localport=any",
            workingDir: null,
            ct: ct);
    }
    
    public async Task EnsureInboundProgramAllowedAsync(string ruleName, string programFullPath, CancellationToken ct)
    {
        if (!File.Exists(programFullPath))
            throw new FileNotFoundException("Frontend exe not found.", programFullPath);

        await Proc.RunAsync("netsh",
            $"advfirewall firewall delete rule name=\"{ruleName}\"",
            workingDir: null,
            ct: ct,
            allowNonZeroExitCode: true);

        await Proc.RunAsync("netsh",
            $"advfirewall firewall add rule name=\"{ruleName}\" dir=in action=allow program=\"{programFullPath}\" enable=yes",
            workingDir: null,
            ct: ct);
    }
}