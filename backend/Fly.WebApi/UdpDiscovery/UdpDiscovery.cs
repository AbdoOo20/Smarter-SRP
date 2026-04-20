
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Fly.Shared.Common;

namespace Fly.WebApi.UdpDiscovery;

public static class UdpDiscovery
{
    public static async Task BroadcastAsync(List<string> urls, int port, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            foreach (var url in urls)
            {
                using var udp = new UdpClient { EnableBroadcast = true };
                var ep = new IPEndPoint(IPAddress.Broadcast, SharedConstants.DiscoveryConfig.DiscoveryPort);
            
                var msg = CreateBeacon(port);
                var data = Encoding.UTF8.GetBytes(msg);
                await udp.SendAsync(data, data.Length, ep);
            }
            
            await Task.Delay(3000, ct);
        }
    }

    static string CreateBeacon(int port)
    {
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var nonce = RandomNumberGenerator.GetInt32(int.MaxValue);

        var body = $"{SharedConstants.DiscoveryConfig.AppId}|{port}|{ts}|{Environment.MachineName.Replace("|", string.Empty)}|{Helpers.GetMachineSid()}|{nonce}";
        using var hmac = new HMACSHA256(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig);
        var sig = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body)));

        return $"{body}|{sig}";
    }
}