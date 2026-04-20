using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Leapay.Client.ServerModeBootstrap;

internal sealed class MySqlProvisioner
{
    private readonly MySqlOptions _opt;
    private static readonly HttpClient _http = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

    public MySqlProvisioner(MySqlOptions opt) => _opt = opt;

    public async Task<MySqlConnectionInfo> EnsureAsync(CancellationToken ct, Action<(string, double)> currentProgress)
    {
        Directory.CreateDirectory(_opt.ManagedInstanceRootDir);

        // We identify "our" MySQL instance by presence of a marker file containing expected version
        var markerPath = Path.Combine(_opt.ManagedInstanceRootDir, "myapp-mysql.marker.json");
        var expected = new Marker { Version = _opt.Version, ServiceName = _opt.ServiceName };

        var mysqlBaseDir = Path.Combine(_opt.ManagedInstanceRootDir, "mysql");
        var mysqlBinDir = Path.Combine(mysqlBaseDir, "bin");
        var mysqldExe = Path.Combine(mysqlBinDir, "mysqld.exe");
        var mysqlExe = Path.Combine(mysqlBinDir, "mysql.exe");

        var myIniPath = Path.Combine(_opt.ManagedInstanceRootDir, "my.ini");
        var dataDir = Path.Combine(_opt.ManagedInstanceRootDir, "data");

        bool needsInstall = true;

        if (File.Exists(markerPath) && File.Exists(mysqldExe) && File.Exists(mysqlExe))
        {
            var markerJson = await File.ReadAllTextAsync(markerPath, ct);
            var marker = JsonSerializer.Deserialize<Marker>(markerJson);
            if (marker?.Version == expected.Version && marker.ServiceName == expected.ServiceName)
            {
                needsInstall = false;
            }
        }

        if (needsInstall)
        {
            // If service exists but marker doesn't, we DO NOT hijack unknown MySQL. We install our own under our folders with our service name.
            await DownloadAndExtractAsync(mysqlBaseDir, ct, i =>
            {
                currentProgress(("Downloading", i * 0.8));
            });
            
            currentProgress(("ConfiguringDatabase", 85));

            WriteMyIni(myIniPath, mysqlBaseDir, dataDir, _opt.Port, _opt.BindOnLan);

            if (!Directory.Exists(dataDir) || Directory.GetFileSystemEntries(dataDir).Length == 0)
            {
                currentProgress(("InitializingData", 90));

                Directory.CreateDirectory(dataDir);
                // --initialize-insecure => no random root password; we’ll create app user afterwards.
                await Proc.RunAsync(mysqldExe,
                    $"--initialize-insecure --basedir=\"{mysqlBaseDir}\" --datadir=\"{dataDir}\"",
                    workingDir: mysqlBinDir,
                    ct: ct);
            }

            if (!WindowsServiceProvisioner.ServiceExists(_opt.ServiceName))
            {
                currentProgress(("InitializingDatabaseService", 95));

                await Proc.RunAsync(mysqldExe,
                    $"--install {_opt.ServiceName} --defaults-file=\"{myIniPath}\"",
                    workingDir: mysqlBinDir,
                    ct: ct);
            }

            currentProgress(("StartingDatabaseService", 98));

            await WindowsServiceProvisioner.StartServiceAsync(_opt.ServiceName, ct);

            var outJson = JsonSerializer.Serialize(expected, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(markerPath, outJson, ct);
        }
        else
        {
            currentProgress(("StartingDatabaseService", 96));

            if (WindowsServiceProvisioner.ServiceExists(_opt.ServiceName))
                await WindowsServiceProvisioner.StartServiceAsync(_opt.ServiceName, ct);
            else
                throw new InvalidOperationException($"Marker exists but Windows service '{_opt.ServiceName}' does not. Installation is inconsistent.");
        }

        currentProgress(("ConfiguringDatabase", 98));

        await EnsureDbAndUserAsync(mysqlExe, mysqlBinDir, ct);

        var host = "127.0.0.1"; // backend typically connects locally; LAN binding is for other machines.
        currentProgress(("DatabaseIsReady", 100));

        return new MySqlConnectionInfo(host, _opt.Port, _opt.DatabaseName, _opt.AppUserName, _opt.AppUserPassword);
    }

    private async Task DownloadAndExtractAsync(string mysqlBaseDir, CancellationToken ct, Action<double> progress)
    {
        if (Directory.Exists(mysqlBaseDir))
        {
            Directory.Delete(mysqlBaseDir, recursive: true);
        }
        
        Directory.CreateDirectory(mysqlBaseDir);

        var zipPath = Path.Combine(_opt.ManagedInstanceRootDir, $"mysql-{_opt.Version}.zip");

        await DownloadWithProgressAsync(
            _http,
            _opt.ZipDownloadUrl,
            zipPath,
            progress,
            ct);

        var tempExtract = Path.Combine(_opt.ManagedInstanceRootDir, "tmp_extract");
        if (Directory.Exists(tempExtract)) Directory.Delete(tempExtract, true);
        Directory.CreateDirectory(tempExtract);

        await ZipFile.ExtractToDirectoryAsync(zipPath, tempExtract, ct);

        // MySQL zip usually contains a single top folder like "mysql-8.0.xx-winx64"
        var topDirs = Directory.GetDirectories(tempExtract);
        if (topDirs.Length != 1)
            throw new InvalidOperationException("Unexpected MySQL ZIP structure (expected exactly one top-level folder).");

        var extractedRoot = topDirs[0];

        CopyDirectory(extractedRoot, mysqlBaseDir);

        // cleanup
        Directory.Delete(tempExtract, true);
        File.Delete(zipPath);
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var name = Path.GetFileName(file);
            File.Copy(file, Path.Combine(destDir, name), overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var name = Path.GetFileName(dir);
            CopyDirectory(dir, Path.Combine(destDir, name));
        }
    }

    private static void WriteMyIni(string myIniPath, string baseDir, string dataDir, int port, bool bindLan)
    {
        // Important: forward slashes in ini avoid escaping issues
        string Fix(string p) => p.Replace('\\', '/');

        var bind = bindLan ? "0.0.0.0" : "127.0.0.1";

        var ini = $"""
                   [mysqld]
                   port={port}
                   basedir={Fix(baseDir)}
                   datadir={Fix(dataDir)}
                   bind-address={bind}
                   default-authentication-plugin=mysql_native_password
                   character-set-server=utf8mb4
                   collation-server=utf8mb4_unicode_ci
                   max_connections=500

                   [client]
                   port={port}
                   """;

        File.WriteAllText(myIniPath, ini, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    public static void RelaunchAsAdminAndExit(string extraArgs)
    {
        var exePath = Environment.ProcessPath!;
        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = extraArgs,
            UseShellExecute = true,
            Verb = "runas",
            WorkingDirectory = Environment.CurrentDirectory
        };

        try
        {
            Process.Start(psi);
        }
        catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == 1223)
        {
            // User cancelled UAC prompt
            return;
        }

        Environment.Exit(0);
    }
    
    private async Task EnsureDbAndUserAsync(string mysqlExe, string workingDir, CancellationToken ct)
    {
        // We connect as root with no password (initialize-insecure).
        // If the environment might already have root password set, a different strategy might be needed (admin credential input or local socket auth).
        var sql = $"""
                   CREATE DATABASE IF NOT EXISTS `{EscapeIdent(_opt.DatabaseName)}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

                   CREATE USER IF NOT EXISTS '{EscapeSqlLiteral(_opt.AppUserName)}'@'%' IDENTIFIED BY '{EscapeSqlLiteral(_opt.AppUserPassword)}';
                   ALTER USER '{EscapeSqlLiteral(_opt.AppUserName)}'@'%' IDENTIFIED BY '{EscapeSqlLiteral(_opt.AppUserPassword)}';

                   GRANT ALL PRIVILEGES ON `{EscapeIdent(_opt.DatabaseName)}`.* TO '{EscapeSqlLiteral(_opt.AppUserName)}'@'%';
                   FLUSH PRIVILEGES;
                   """;

        // Run via: mysql.exe -u root --protocol=tcp -h 127.0.0.1 -P <port> -e "<sql>"
        // Use localhost TCP to avoid named pipes differences.
        var args =
            $"-u root --protocol=tcp -h 127.0.0.1 -P {_opt.Port} -e \"{EscapeForCmdArgument(sql)}\"";

        await Proc.RunAsync(mysqlExe, args, workingDir, ct);
    }

    private static string EscapeSqlLiteral(string s) => s.Replace("\\", "\\\\").Replace("'", "''");
    private static string EscapeIdent(string s) => s.Replace("`", "``");

    // Safe-ish for cmd argument inside quotes
    private static string EscapeForCmdArgument(string s)
        => s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", " ").Replace("\n", " ");
    
    private sealed record Marker
    {
        public string Version { get; init; } = "";
        public string ServiceName { get; init; } = "";
    }
    
    private static async Task DownloadWithProgressAsync(
        HttpClient http,
        Uri url,
        string destinationPath,
        Action<double>? reportProgress,
        CancellationToken ct)
    {
        using var response = await http.GetAsync(
            url,
            HttpCompletionOption.ResponseHeadersRead,
            ct);

        response.EnsureSuccessStatusCode();

        var contentLength = response.Content.Headers.ContentLength;

        await using var contentStream = await response.Content.ReadAsStreamAsync(ct);
        await using var fileStream = new FileStream(
            destinationPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        var buffer = new byte[81920];
        long totalRead = 0;
        int lastReportedProgress = -1;

        while (true)
        {
            int read = await contentStream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct);
            if (read == 0)
                break;

            await fileStream.WriteAsync(buffer.AsMemory(0, read), ct);
            totalRead += read;

            if (contentLength.HasValue && reportProgress != null)
            {
                int progress = (int)((totalRead * 100L) / contentLength.Value);

                // Avoid spamming UI with duplicate values
                if (progress != lastReportedProgress)
                {
                    lastReportedProgress = progress;
                    reportProgress(progress);
                }
            }
        }

        // Ensure we end on 100%
        if (contentLength.HasValue)
            reportProgress?.Invoke(100);
    }
}