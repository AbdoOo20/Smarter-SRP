using System.Diagnostics;
using System.Text;

namespace Leapay.Client.ServerModeBootstrap;

internal static class Proc
{
    public static ProcResult Run(string fileName, string args, string? workingDir = null, bool allowNonZeroExitCode = false)
        => RunAsync(fileName, args, workingDir, CancellationToken.None, allowNonZeroExitCode).GetAwaiter().GetResult();

    public static async Task<ProcResult> RunAsync(
        string fileName,
        string args,
        string? workingDir,
        CancellationToken ct,
        bool allowNonZeroExitCode = false)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = args,
            WorkingDirectory = workingDir ?? "",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var p = new Process { StartInfo = psi, EnableRaisingEvents = true };

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();

        p.OutputDataReceived += (_, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
        p.ErrorDataReceived += (_, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

        if (!p.Start())
            throw new InvalidOperationException($"Failed to start process: {fileName}");

        p.BeginOutputReadLine();
        p.BeginErrorReadLine();

        await p.WaitForExitAsync(ct);

        var res = new ProcResult(p.ExitCode, stdout.ToString(), stderr.ToString());

        if (!allowNonZeroExitCode && res.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Command failed ({res.ExitCode}): {fileName} {args}\n\nSTDOUT:\n{res.StdOut}\n\nSTDERR:\n{res.StdErr}");
        }

        return res;
    }
}