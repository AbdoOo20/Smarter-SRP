using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Fly.Core.Modules.Global.Service;

public interface ITokenService
{
    string MixGuidStrings(Guid guid1, Guid guid2);

    void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);

    ValueTask<bool> VerifyPassword(string salt, string plain, string password);

    ValueTask<bool> ValidateToken(string token, string fingerPrint);

    bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt);

    ValueTask<(string Password, string Salt)> GeneratePassword(string fingerPrint);
    ValueTask<string> GenerateToken(string fingerPrint);
    string MixGuidStrings(string guid1, string guid2);
    string GenerateHallVedioId(Guid userid, Guid hallId);


    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> logger;

        public TokenService(ILogger<TokenService> logger)
        {
            this.logger = logger;
        }

        public ValueTask<string> GenerateToken(string fingerPrint)
        {
            using var hmac = new HMACSHA512();
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(fingerPrint));
            var token = Convert.ToBase64String(hash);
            return ValueTask.FromResult(token);
        }

        public ValueTask<(string Password, string Salt)> GeneratePassword(string plainPassword)
        {
            using var hmac = new HMACSHA512();

            var plainBytes = GetBytes(plainPassword);

            var saltBytes = hmac.Key;
            var hashBytes = hmac.ComputeHash(plainBytes);

            var saltString = To64String(saltBytes);
            var passwordHash = To64String(hashBytes);
            return ValueTask.FromResult((passwordHash, saltString));

        }
        private byte[] GetBytes(string text) => Encoding.UTF8.GetBytes(text);
        private byte[] GetBytes2(string text) => Convert.FromBase64String(text);
        private string To64String(byte[] bytes) => Convert.ToBase64String(bytes);



        public ValueTask<bool> VerifyPassword(string salt, string plain, string password)
        {
            var saltBytes = GetBytes2(salt);
            var plainBytes = GetBytes(plain);
            var dbPasswordBytes = GetBytes2(password);

            using var hmac = new HMACSHA512(saltBytes);
            var hash = hmac.ComputeHash(plainBytes);
            var isEqualBytes = hash.SequenceEqual(dbPasswordBytes);

            return ValueTask.FromResult(isEqualBytes);
        }

        public ValueTask<bool> ValidateToken(string token, string fingerPrint)
        {
            return ValueTask.FromResult(token.Equals(GenerateToken(fingerPrint)));
        }



        public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordHash = Convert.ToBase64String(hash);
        }

        public bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var isEqual = computedHash.SequenceEqual(passwordHash);
            return isEqual;
        }


        public string MixGuidStrings(string guid1, string guid2)
        {
            return MixGuidStrings(Guid.Parse(guid1), Guid.Parse(guid2));
        }
        public string MixGuidStrings(Guid guid1, Guid guid2)
        {
            if (guid1 == Guid.Empty || guid2 == Guid.Empty)
                throw new ArgumentNullException(nameof(guid1) + ", " + nameof(guid2));

            var mixed = new char[36];
            var v1 = guid1.ToString();
            var v2 = guid2.ToString();

            if (v1.Length != 36 || v2.Length != 36)
                throw new ArgumentException("Guids must be 36 characters");

            for (int i = 0; i < 36; i++)
            {
                if (i % 2 == 0)
                {
                    mixed[i] = v1[i];
                }
                else
                {
                    mixed[i] = v2[i];
                }
            }

            return new string(mixed);
        }

        public string GenerateHallVedioId(Guid userid, Guid hallId)
        {
            return $"{userid}-{hallId}";
        }
    }

}