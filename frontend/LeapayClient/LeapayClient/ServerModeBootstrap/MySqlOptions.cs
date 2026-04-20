namespace Leapay.Client.ServerModeBootstrap;

public sealed record MySqlOptions(
    string ManagedInstanceRootDir,
    string Version,
    Uri ZipDownloadUrl,
    int Port,
    string ServiceName,
    string DatabaseName,
    string AppUserName,
    string AppUserPassword,
    bool BindOnLan,
    bool OpenMySqlPortOnLan = false,
    string MySqlFirewallRuleName = "MyApp MySQL (TCP)"
);