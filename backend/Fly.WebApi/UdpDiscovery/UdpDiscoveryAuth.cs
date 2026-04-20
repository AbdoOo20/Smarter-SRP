using Fly.Shared.Common;

namespace Fly.WebApi.UdpDiscovery;

using System.Security.Cryptography;
using System.Text;

public sealed class UdpDiscoveryAuth
{
    private readonly Dictionary<string, DateTime> _challenges = new();
    private readonly TimeSpan _challengeTtl = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _tokenTtl = TimeSpan.FromMinutes(15);

    public string CreateChallenge()
    {
        Cleanup();

        var nonce = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        _challenges[nonce] = DateTime.UtcNow;
        return nonce;
    }

    public string? VerifyChallenge(string nonce, string response)
    {
        if (!_challenges.TryGetValue(nonce, out var issued))
            return null;

        _challenges.Remove(nonce);
        if (DateTime.UtcNow - issued > _challengeTtl)
            return null;

        using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
        var expected = Convert.ToHexString(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(nonce)));

        if (!CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(expected),
            Encoding.ASCII.GetBytes(response)))
            return null;

        return CreateToken();
    }

    public bool VerifyToken(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return false;

            var payload = $"{parts[0]}.{parts[1]}";
            using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
            var expectedSig = Convert.ToHexString(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

            if (!CryptographicOperations.FixedTimeEquals(
                Encoding.ASCII.GetBytes(expectedSig),
                Encoding.ASCII.GetBytes(parts[2])))
                return false;

            var expires = DateTime.FromBinary(
                long.Parse(Encoding.UTF8.GetString(
                    Convert.FromBase64String(parts[1]))));

            return expires > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    private string CreateToken()
    {
        var expires = DateTime.UtcNow.Add(_tokenTtl);
        var header = Convert.ToBase64String(Encoding.UTF8.GetBytes("v1"));
        var body = Convert.ToBase64String(Encoding.UTF8.GetBytes(expires.ToBinary().ToString()));

        var payload = $"{header}.{body}";
        using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
        var sig = Convert.ToHexString(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

        return $"{payload}.{sig}";
    }

    private void Cleanup()
    {
        var now = DateTime.UtcNow;
        foreach (var k in _challenges
                     .Where(p => now - p.Value > _challengeTtl)
                     .Select(p => p.Key)
                     .ToList())
            _challenges.Remove(k);
    }
}
