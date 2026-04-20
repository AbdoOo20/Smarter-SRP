using Fly.Shared.Common;

namespace Leapay.Client.UdpDiscovery;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

public class UdpAuth
{
    public static async Task<string> AuthenticateAsync(HttpClient http)
    {
        var c = await http.GetFromJsonAsync<Challenge>("auth/challenge");

        using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
        var resp = Convert.ToHexString(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(c!.nonce)));

        var r = await http.PostAsJsonAsync("auth/verify",
            new { nonce = c.nonce, response = resp });

        var t = await r.Content.ReadFromJsonAsync<Token>();
        return t!.token;
    }

    record Challenge(string nonce);
    record Token(string token);
}
