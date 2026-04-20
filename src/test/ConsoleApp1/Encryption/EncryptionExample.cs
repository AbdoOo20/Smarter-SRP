using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1.Encryption;


public static class EncryptionExample
{

    public static void StartExample()
    {
        var text = "I'm Islam Alshiki";

        var encrypedText = CustomEncryptionLayer.Encrypt(text);
        var plainText = CustomEncryptionLayer.Decrypt(encrypedText);

        Console.WriteLine(text);
        Console.WriteLine(encrypedText);
        Console.WriteLine(plainText);
    }

}

public static class KeyGenerator
{
    public static byte[] GenerateKey()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        byte[] key = new byte[32]; // 32 bytes for AES-256
        randomNumberGenerator.GetBytes(key);
        return key;
    }
}

public static class CustomEncryptionLayer
{
    private static readonly byte[] CustomKey = KeyGenerator.GenerateKey();
    //new byte[] { /* Your 32-byte key for AES */ };

    public static string Encrypt(string plainText)
    {
        using var aes = new AesGcm(CustomKey);
        byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        byte[] cipherText = new byte[plainText.Length];
        byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];

        // Generate a random nonce
        RandomNumberGenerator.Fill(nonce);

        // Encrypt the data
        aes.Encrypt(nonce, Encoding.UTF8.GetBytes(plainText), cipherText, tag);

        // Combine nonce, tag, and ciphertext for storage
        var result = new byte[nonce.Length + tag.Length + cipherText.Length];
        Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
        Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
        Buffer.BlockCopy(cipherText, 0, result, nonce.Length + tag.Length, cipherText.Length);

        // Return as Base64
        return Convert.ToBase64String(result);
    }

    public static string Decrypt(string cipherText)
    {
        byte[] combined = Convert.FromBase64String(cipherText);
        using var aes = new AesGcm(CustomKey);

        // Extract nonce, tag, and ciphertext
        byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];
        byte[] cipherTextBytes = new byte[combined.Length - nonce.Length - tag.Length];

        Buffer.BlockCopy(combined, 0, nonce, 0, nonce.Length);
        Buffer.BlockCopy(combined, nonce.Length, tag, 0, tag.Length);
        Buffer.BlockCopy(combined, nonce.Length + tag.Length, cipherTextBytes, 0, cipherTextBytes.Length);

        // Decrypt the data
        var plainTextBytes = new byte[cipherTextBytes.Length];
        aes.Decrypt(nonce, cipherTextBytes, tag, plainTextBytes);

        return Encoding.UTF8.GetString(plainTextBytes);
    }

    // Example of a custom transformation (optional)
    private static byte[] ApplyCustomTransformation(byte[] data)
    {
        // Implement your custom transformation logic here
        return data; // Placeholder
    }

    // Further methods for index mapping and timestamp can be added here
}


/*

 here are some popular licensing mechanisms for WPF applications:

License Manager: This is a built-in .NET class that allows you to manage licenses for your application. It provides a simple way to validate licenses and handle expired or invalid licenses.

LicenseKey: This is a simple licensing mechanism where you provide a license key to the user. The user enters the key into your application, and you validate it on the server.

Software Protection Platform (SPP): This is a Microsoft licensing service that provides a secure and reliable way to manage licenses for your application. It integrates well with Windows and provides a user-friendly licensing experience.

LicenseServer: This is a custom licensing mechanism where you host a server that validates licenses. Your application sends a request to the server to validate a license, and the server responds with a result.

Public Key Cryptography: This is a method of encrypting and decrypting data using public and private keys. You can use this method to encrypt a license file, and then decrypt it on the client-side to validate the license.

Hardware Binding: This is a method of binding your application to a specific hardware configuration. This can be used to prevent piracy by making it difficult for users to run your application on different hardware.

Remember, no licensing mechanism is foolproof, so it's important to test and validate your licensing system.
 */