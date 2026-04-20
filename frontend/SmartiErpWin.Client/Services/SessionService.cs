using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Marketers.Response;
using SmartErpWin.Core.HttpServices;

namespace SmartiErpWin.Client.Services;

public partial class SessionService(AuthenticationStorageService authService, HttpClient client)
    : FeatureBase<DtoBase>(client)
{
    public string GetToken()
    {
        var process = AuthenticationStorageService.TryGetToken(out var token);

        return !process ? string.Empty : token;
    }

    private async Task<DtoUserInfo?> GetUserInfoAsync()
    {
        return (await PostSpecificOneAsync<DtoUserInfo>("Signing")).Data;
    }

    [ObservableProperty]
    private DtoUserInfo? _userInfo;

    public async Task<bool> JudgeAccessAsync(string page, string action)
    {
        if (Bypass.Contains(page))
            return true;
        
        UserInfo ??= await GetUserInfoAsync();
        
        if (UserInfo is null)
            return false;

        if (UserInfo.Roles.Any(r => r.Key is "GHOST"))
            return true;

        var role = $"{page}.{action}";

        return UserInfo.Roles.Any(r => r.Key == role);
    }

    private static string[] Bypass { get; } =
    [
        "FastSales",
        "Home"
    ];
}