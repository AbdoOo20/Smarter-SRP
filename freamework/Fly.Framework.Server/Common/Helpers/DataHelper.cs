using System.Security.Cryptography;
using System.Text;

namespace Fly.Framework.Server.Common.Helpers;

public static class DataHelper
{
    public static string Shutdown(string exitCode)
    {
        const string code = "xlRB5O!y&A=mfVM@Y=7$6m-22Wrg*aj=";
        
        var i = Convert.FromBase64String(exitCode);

        var start = new byte[16];
        var text = new byte[i.Length - 16];

        Buffer.BlockCopy(i, 0, start, 0, 16);
        Buffer.BlockCopy(i, 16, text, 0, text.Length);

        using var c = Aes.Create();
        c.Key = Encoding.UTF8.GetBytes(code);
        c.IV = start;
        c.Mode = CipherMode.CBC;
        c.Padding = PaddingMode.PKCS7;

        var handle = c.CreateDecryptor();

        using MemoryStream ms = new(text);
        using CryptoStream cs = new(ms, handle, CryptoStreamMode.Read);
        using StreamReader sr = new(cs, Encoding.UTF8);
    
        return sr.ReadToEnd();
    }
}