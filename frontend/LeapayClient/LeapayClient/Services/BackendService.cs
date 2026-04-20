using System.Security.Cryptography.X509Certificates;
using Fly.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using SmartErpWin.Core.HttpServices;

namespace Leapay.Client.Services;

public class BackendService
{
    public static async Task RefreshAccountData(SessionService session)
    {
        // ignored
    }

    public static async Task<bool> EnsureTokenIsValidAsync(string token)
    {
        var featureBase = new FeatureBase<DtoAccount>(CreatePinnedClient(), CreatePinnedHandler());
        featureBase.AuthenticateRequests(token);

        var req = await featureBase.GetAllInnerAsync("Account/accounts", null);

        return req.Status;
    }
    
    public static HttpClient CreatePinnedClient(string? expectedThumbprint = null)
    {
        var handler = CreatePinnedHandler(expectedThumbprint);

        return new HttpClient(handler);
    }
    
    public static HttpClientHandler CreatePinnedHandler(string? expectedThumbprint = null)
    {
        expectedThumbprint ??= SharedConstants.DiscoveryConfig.SharedThumbprint;
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, cert, _, _) =>
                cert is X509Certificate2 c &&
                c.Thumbprint!.Equals(expectedThumbprint,
                    StringComparison.OrdinalIgnoreCase)
        };
    }
}