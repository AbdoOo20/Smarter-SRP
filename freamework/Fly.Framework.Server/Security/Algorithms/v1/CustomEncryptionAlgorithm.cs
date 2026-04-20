using System.Security.Cryptography;
using System.Text;

namespace Fly.Framework.Server.Security.Algorithms.v1;

public class CustomEncryptionAlgorithm : ICustomEncryptionAlgorithm
{
    private string Key = string.Empty;

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(Key))
            throw new InvalidOperationException("Encryption key not set.");

        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = new byte[16]; // Use a zero IV for simplicity; not secure!

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var memoryStream = new MemoryStream(cipherBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cryptoStream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(Key))
            throw new InvalidOperationException("Encryption key not set.");

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = new byte[16]; // Use a zero IV for simplicity; not secure!

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cryptoStream))
            {
                writer.Write(plainText);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    public void SetKey(string key)
    {
        if (key.Length != 16 && key.Length != 32)
            throw new ArgumentException("Key must be 16 or 32 bytes long.");

        Key = key;
    }
}

public class CustomEncryptionAlgorithmPlus : ICustomEncryptionAlgorithm
{
    // Key for encryption/decryption
    private string Key = string.Empty;

    public void SetKey(string key)
    {
        this.Key = key;
    }

    // Function to encrypt a string
    public string Encrypt(string plainText)
    {
        // Convert the plain text to a byte array
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        // Generate a random salt for the encryption
        var salt = RandomNumberGenerator.GetBytes(16);

        // Combine the salt and the key
        byte[] combinedKey = CombineBytes(salt, Encoding.UTF8.GetBytes(Key));

        // Create a new instance of the Rfc2898DeriveBytes class
        // This class is used to derive a key from a password
        using (var deriveBytes = new Rfc2898DeriveBytes(combinedKey, salt, 1000))
        {
            // Get the derived key
            byte[] derivedKey = deriveBytes.GetBytes(32);

            // Create a new instance of the AesManaged class
            // This class is used to perform AES encryption
            using (var aes = new AesManaged())
            {
                // Set the key and IV for the AES algorithm
                aes.Key = derivedKey;
                aes.IV = salt;

                // Create a new instance of the CryptoStream class
                // This class is used to encrypt the plain text
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    // Write the plain text to the CryptoStream
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    // Get the encrypted bytes from the MemoryStream
                    byte[] cipherBytes = memoryStream.ToArray();

                    // Convert the encrypted bytes to a Base64 string
                    return Convert.ToBase64String(cipherBytes);
                }
            }
        }
    }

    // Function to decrypt a string
    public string Decrypt(string cipherText)
    {
        // Convert the Base64 string to a byte array
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        // Extract the salt from the first 16 bytes of the cipher text
        byte[] salt = new byte[16];
        Array.Copy(cipherBytes, 0, salt, 0, 16);

        // Combine the salt and the key
        byte[] combinedKey = CombineBytes(salt, Encoding.UTF8.GetBytes(Key));

        // Create a new instance of the Rfc2898DeriveBytes class
        using (var deriveBytes = new Rfc2898DeriveBytes(combinedKey, salt, 1000))
        {
            // Get the derived key
            byte[] derivedKey = deriveBytes.GetBytes(32);

            // Create a new instance of the AesManaged class
            using (var aes = new AesManaged())
            {
                // Set the key and IV for the AES algorithm
                aes.Key = derivedKey;
                aes.IV = salt;

                // Create a new instance of the CryptoStream class
                // This class is used to decrypt the cipher text
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream(cipherBytes, 16, cipherBytes.Length - 16))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    // Read the decrypted bytes from the CryptoStream
                    byte[] plainBytes = new byte[cipherBytes.Length - 16];
                    int bytesRead = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

                    // Convert the decrypted bytes to a string
                    return Encoding.UTF8.GetString(plainBytes, 0, bytesRead);
                }
            }
        }
    }

    // Helper function to combine two byte arrays
    private byte[] CombineBytes(byte[] first, byte[] second)
    {
        byte[] combined = new byte[first.Length + second.Length];
        first.CopyTo(combined, 0);
        second.CopyTo(combined, first.Length);
        return combined;
    }
}
