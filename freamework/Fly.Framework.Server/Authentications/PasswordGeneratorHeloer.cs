using System.Security.Cryptography;
using Fly.Framework.Server.Security.Algorithms;
using Microsoft.Extensions.Configuration;

namespace Fly.Framework.Server.Authentications;

public class PasswordGeneratorHeloer
{
    private readonly IConfiguration configuration;
    private readonly ICustomEncryptionAlgorithm _customEncryptionAlgorithm;

    public PasswordGeneratorHeloer(IConfiguration configuration, ICustomEncryptionAlgorithm customEncryptionAlgorithm)
    {
        this.configuration = configuration;
        _customEncryptionAlgorithm = customEncryptionAlgorithm;
    }


    public void CreatePasswordHashBytes(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        return VerifyPasswordHash(password, Convert.FromBase64String(passwordHash), Convert.FromBase64String(passwordSalt));
    }

}
