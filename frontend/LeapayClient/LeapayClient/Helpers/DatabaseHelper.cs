using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Leapay.Client.Services;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.Helpers;

public static class DatabaseHelper
{
    public static async Task TakeBackupAsync()
    {
        if (!OperatingSystem.IsWindows())
            return;
        
        var mysqlDump = new FileInfo(Path.Combine(Environment.CurrentDirectory, "Unmanaged", "mysqldump.exe"));

        if (!mysqlDump.Exists)
        {
            Warning("CODE: Dump service is missing. For backups, add the files and restart.");
            return;
        }
        
        while (true)
        {
            var model = PreferencesService.Get(s => s.SysConfigModel);
            if (!model.EnableBackup || string.IsNullOrEmpty(model.BackupPath))
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                continue;
            }
            
            Information("Backup is enabled. Interval: {0}, path: {1}.",
                model.BackupMinutes, model.BackupPath);

            try
            {
                var outputFile = Path.Combine(model.BackupPath, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.emd");
                var args = $"--user={model.DatabaseUsername} --password={model.GetPassword()} " +
                           $"--host={model.DatabaseUrl} --port={model.DatabasePort} {model.DatabaseName} ";

                var psi = new ProcessStartInfo
                {
                    FileName = mysqlDump.FullName,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using var process = Process.Start(psi)!;
                
                var errorTask = Task.Run(async () =>
                {
                    var errors = await process.StandardError.ReadToEndAsync();
                    return errors;
                });

                //Debug("Mysqldump log output:\n {0}", output);
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(FileHelper.RenameFileWithExtension());
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                await using var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                await fs.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length));

                await using var cryptoStream = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
                await process.StandardOutput.BaseStream.CopyToAsync(cryptoStream);
                await cryptoStream.FlushAsync();
                await process.WaitForExitAsync();
                
                var stderr = await errorTask;

                //await fs.DisposeAsync();
                if (process.ExitCode != 0)
                {
                    //File.Delete(outputFile);
                    Error("mysqldump failed (deleting file): {Stderr}", stderr);
                }
            }
            catch (Exception ex)
            {
                Error("Error while trying to take a backup: {0}", ex.ToString());
            }

            await Task.Delay(TimeSpan.FromMinutes(model.BackupMinutes));
        }
    }
}