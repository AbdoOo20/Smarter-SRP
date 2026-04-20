using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Marketers.Response;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.HttpServices;

namespace Leapay.Client.Services;

public partial class SessionService
    : FeatureBase<DtoBase>
{
    public SessionService()
    {
        SetClient(App.GetService<HttpClient>(), BackendService.CreatePinnedHandler());
    }
    
    public event EventHandler? UserInfoChanged;

    private TaskCompletionSource _userIsSet = new();
    
    public string? GetToken()
    {
        var process = AuthenticationStorageService.TryGetToken(out var token);

        return !process ? string.Empty : token;
    }

    private async Task<DtoUserInfo> GetUserInfoAsync()
    {
        return (await PostSpecificOneAsync<DtoUserInfo>("Signing")).Data;
    }

    public async Task RefreshUserInfoAsync()
    {
        UserInfo = await GetUserInfoAsync();
    }
    
    [ObservableProperty]
    public partial DtoUserInfo? UserInfo { get; set; }

    partial void OnUserInfoChanged(DtoUserInfo? value)
    {
        UserInfoChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool Verify(bool pred)
    {
        return pred || UserInfo.Roles.Any(r => r.Key is "GHOST");
    }
    
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

    public bool JudgeSpecificAccess(string key)
    {
        if (UserInfo is null)
        {
            _userIsSet.Task.Wait();
        }
        
        if (UserInfo.Roles.Any(r => r.Key is "GHOST"))
            return true;
        
        return UserInfo.Roles.Any(r => r.Key == key);
    }
    
    public bool UserKeyContains(string key)
    {
        if (UserInfo is null)
        {
            _userIsSet.Task.Wait();
        }
        
        if (UserInfo.Roles.Any(r => r.Key is "GHOST"))
            return true;
        
        if (UserInfo.Roles.Any(r => r.Key.Contains(key)))
            return true;

        return false;
    }
    
    private static string[] Bypass { get; } =
    [
        "Home",
        "Settings"
    ];
}