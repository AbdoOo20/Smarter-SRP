using System.Security.Cryptography;
using System.Text;

namespace SmartErpWin.Core.Helpers;

public static class ApplicationHost
{
    public static string Start(string args)
    {
        using var c = Aes.Create();
        c.Key = Encoding.UTF8.GetBytes(FileHelper.RenameFileWithExtension());
        c.Mode = CipherMode.CBC;
        c.Padding = PaddingMode.PKCS7;
        c.GenerateIV();

        var standby = c.CreateEncryptor();

        var ar = Encoding.UTF8.GetBytes(args);
        byte[] done;

        using (MemoryStream ms = new())
        using (CryptoStream cs = new(ms, standby, CryptoStreamMode.Write))
        {
            cs.Write(ar, 0, ar.Length);
            cs.FlushFinalBlock();
            done = ms.ToArray();
        }

        var result = new byte[c.IV.Length + done.Length];
        Buffer.BlockCopy(c.IV, 0, result, 0, c.IV.Length);
        Buffer.BlockCopy(done, 0, result, c.IV.Length, done.Length);

        return Convert.ToBase64String(result);
    }
    
    public static string Shutdown(string exitCode)
    {
        var i = Convert.FromBase64String(exitCode);

        var start = new byte[16];
        var text = new byte[i.Length - 16];

        Buffer.BlockCopy(i, 0, start, 0, 16);
        Buffer.BlockCopy(i, 16, text, 0, text.Length);

        using var c = Aes.Create();
        c.Key = Encoding.UTF8.GetBytes(FileHelper.RenameFileWithExtension());
        c.IV = start;
        c.Mode = CipherMode.CBC;
        c.Padding = PaddingMode.PKCS7;

        var handle = c.CreateDecryptor();

        using MemoryStream ms = new(text);
        using CryptoStream cs = new(ms, handle, CryptoStreamMode.Read);
        using StreamReader sr = new(cs, Encoding.UTF8);
    
        return sr.ReadToEnd();
    }
    
    public static async Task ShutdownProgram(string inputFile, string outputFile)
    {
        var key = Encoding.UTF8.GetBytes(FileHelper.RenameFileWithExtension());

        await using var fsIn = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        await using var fsOut = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var iv = new byte[16];
        var read = await fsIn.ReadAsync(iv, 0, iv.Length);
        if (read != iv.Length)
            throw new Exception("Invalid file");

        aes.IV = iv;

        await using var cryptoStream = new CryptoStream(fsOut, aes.CreateDecryptor(), CryptoStreamMode.Write);

        await fsIn.CopyToAsync(cryptoStream);
        await cryptoStream.FlushAsync();
    }
}