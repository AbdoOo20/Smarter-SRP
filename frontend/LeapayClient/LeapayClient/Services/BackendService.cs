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
        // With no explicit thumbprint, trust both the repository certificate and
        // the one used by backends already deployed at customers.
        var trusted = expectedThumbprint is null
            ? SharedConstants.DiscoveryConfig.TrustedThumbprints
            : [expectedThumbprint];

        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, cert, _, _) =>
                cert is X509Certificate2 c &&
                trusted.Any(t => c.Thumbprint!.Equals(t, StringComparison.OrdinalIgnoreCase))
        };
    }
}