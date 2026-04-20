namespace Leapay.Client.ServerModeBootstrap;

public sealed record MySqlConnectionInfo(
    string Host,
    int Port,
    string Database,
    string User,
    string Password,
    string Provider = "MySql" // marker if you want
);