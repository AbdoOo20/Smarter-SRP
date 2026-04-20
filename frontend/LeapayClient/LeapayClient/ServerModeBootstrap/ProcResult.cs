namespace Leapay.Client.ServerModeBootstrap;

internal sealed record ProcResult(int ExitCode, string StdOut, string StdErr);