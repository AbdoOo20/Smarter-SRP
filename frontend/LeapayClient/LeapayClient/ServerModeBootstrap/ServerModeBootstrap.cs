using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Leapay.Client.ClientModels.Configuration;
using Leapay.Client.UserInterface.Common.SysConfigPage;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.ServerModeBootstrap;

public static class ServerModeBootstrap
{
    /// <summary>
    /// Call this when user selects "Server + App mode".
    /// This method is idempotent: safe to call multiple times.
    /// Must run as Admin.
    /// </summary>
    public static async Task<BootstrapResult> EnsureServerModeAsync(BootstrapOptions opt, Action<(bool, string, double)> progress, CancellationToken ct = default)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("This bootstrapper is Windows-only (services + firewall).");

        if (!WindowsAdmin.IsRunningAsAdmin())
        {
            MySqlProvisioner.RelaunchAsAdminAndExit(BackendSetup);
            return null;
        }

        var mysql = new MySqlProvisioner(opt.MySql);
        var mysqlInfo = await mysql.EnsureAsync(ct, tuple => progress((false, tuple.Item1, tuple.Item2 * 0.8)));

        progress((false, "InitializingDatabase", 85));

        if (!string.IsNullOrWhiteSpace(opt.BackendPath))
        {
            var sysConfig = new SysConfigPageViewModel(null, EntryType.Add);
            sysConfig.SysConfigModel = new SysConfigModel
            {
                BackendPath = opt.BackendPath,
                DatabaseName = opt.MySql.DatabaseName,
                DatabasePassword = opt.MySql.AppUserPassword,
                DatabasePort = opt.MySql.Port,
                DatabaseUsername = opt.MySql.AppUserName,
                DatabasePasswordU = opt.MySql.AppUserPassword,
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32))
            };
            
            sysConfig.SaveSettingsCommand.Execute(null);
        }

        progress((false, "StartingService", 90));
            
        var svc = new WindowsServiceProvisioner();
        await svc.EnsureServiceInstalledAndRunningAsync(
            serviceName: opt.BackendServiceName,
            displayName: opt.BackendServiceDisplayName,
            exePath: opt.BackendExePath,
            arguments: opt.BackendExeArguments,
            ct: ct
        );

        progress((false, "ConfiguringNetwork", 95));

        var fw = new FirewallProvisioner();
        await fw.EnsureInboundTcpAllPortsAllowedAsync(opt.BackendFirewallRuleName, ct);

        if (opt.OpenInboundForFrontendExe && !string.IsNullOrWhiteSpace(opt.FrontendExePath))
        {
            await fw.EnsureInboundProgramAllowedAsync(opt.FrontendFirewallRuleName, opt.FrontendExePath!, ct);
        }

        if (opt.MySql.OpenMySqlPortOnLan)
        {
            await fw.EnsureInboundTcpAllPortsAllowedAsync(opt.MySql.MySqlFirewallRuleName, ct);
        }

        progress((true, "Done", 100));

        return new BootstrapResult(mysqlInfo, opt.BackendServiceName);
    }
}
