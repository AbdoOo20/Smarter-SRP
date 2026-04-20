using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SmartiErpWin.Client.Services;

public class AuthenticationStorageService
{
    private static readonly string FileDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        nameof(SmartiErpWin));
    
    private static readonly string FilePath = Path.Combine(FileDirectory, TokenFile);

    public static void SaveToken(string token)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(token);
        var encryptedToken = ProtectedData.Protect(tokenBytes, null, DataProtectionScope.CurrentUser);

        Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

        using var fileStream = File.Create(FilePath);
        
        fileStream.Write(encryptedToken, 0, encryptedToken.Length);
    }

    public static bool TryGetToken(out string token)
    {
        token = string.Empty;
        
        if (!File.Exists(FilePath))
        {
            return false;
        };

        var encryptedToken = File.ReadAllBytes(FilePath);
        var tokenBytes = ProtectedData.Unprotect(encryptedToken, null,
            DataProtectionScope.CurrentUser);

        token = Encoding.UTF8.GetString(tokenBytes);
        
        return true;
    }

    public static string? GetToken()
    {
        return TryGetToken(out var token) ? token : null;
    }
    
    public static void DeleteToken()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }
}