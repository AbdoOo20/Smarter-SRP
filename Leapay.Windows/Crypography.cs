using System.Security.Cryptography;
using System.Text;

namespace Leapay.Windows;

public class Cryptography
{
    public static byte[] Protect(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);

        return ProtectedData.Protect(
            bytes,
            optionalEntropy: null,
            scope: DataProtectionScope.CurrentUser
        );
    }
    
    public static string Unprotect(byte[] encrypted)
    {
        var bytes = ProtectedData.Unprotect(
            encrypted,
            optionalEntropy: null,
            scope: DataProtectionScope.CurrentUser
        );

        return Encoding.UTF8.GetString(bytes);
    }
}