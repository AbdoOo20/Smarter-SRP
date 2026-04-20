namespace Leapay.Client.ServerModeBootstrap;

public sealed record BootstrapResult(MySqlConnectionInfo MySql, string BackendServiceName);