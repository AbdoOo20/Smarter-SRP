using Fly.Shared.Modules.Accounting.Dtos;
using SmartErpWin.Core.HttpServices;

namespace SmartiErpWin.Client.Services;

public class BackendService
{
    public static async Task RefreshAccountData(SessionService session)
    {
        // ignored
    }

    public static async Task<bool> EnsureTokenIsValidAsync(string token)
    {
        var featureBase = new FeatureBase<DtoAccount>(new HttpClient());
        featureBase.AuthenticateRequests(token);

        var req = await featureBase.GetAllInnerAsync("Account/accounts", null);

        return req.Status;
    }
}