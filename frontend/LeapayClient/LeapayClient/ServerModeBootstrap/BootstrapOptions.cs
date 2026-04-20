namespace Leapay.Client.ServerModeBootstrap;

public sealed record BootstrapOptions(
    MySqlOptions MySql,
    string BackendExePath,
    string BackendServiceName,
    string BackendServiceDisplayName,
    int BackendListenPort,
    string? BackendExeArguments = null,
    string? BackendPath = null,
    string BackendDbConfigJsonPathInAppSettings = "ConnectionStrings:Default",
    bool OpenInboundForFrontendExe = false,
    string? FrontendExePath = null,
    string FrontendFirewallRuleName = "MyApp Frontend",
    string BackendFirewallRuleName = "MyApp Backend (TCP)",
    string? DbInfoOutputPath = null // optional: persist returned POCO as JSON
);