namespace Fly.Framework.Server.Security.Algorithms;

public interface ICustomEncryptionAlgorithm
{
    void SetKey(string key);
    string Decrypt(string cipherText);
    string Encrypt(string plainText);
}
