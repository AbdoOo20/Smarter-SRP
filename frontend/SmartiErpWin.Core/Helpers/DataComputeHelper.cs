using System.Security.Cryptography;

namespace SmartErpWin.Core.Helpers;

public static class DataComputeHelper
{
    public static string ComputeHash(string value)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        using var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        var hashBytes = new byte[48];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
        Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyHash(string value, string storedHash)
    {
        var hashBytes = Convert.FromBase64String(storedHash);

        var salt = new byte[16];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

        var storedSubkey = new byte[32];
        Buffer.BlockCopy(hashBytes, 16, storedSubkey, 0, 32);

        using var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 100_000, HashAlgorithmName.SHA256);
        var computedSubkey = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(storedSubkey, computedSubkey);
    }
}