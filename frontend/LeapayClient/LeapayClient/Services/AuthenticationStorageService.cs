using Avalonia.Preferences;

namespace Leapay.Client.Services;

public class AuthenticationStorageService
{
    private const string TokenKey = "token";
    private static Preferences? _pref;

    public static void SaveToken(string token)
    {
        PreferencesService.SetS(TokenKey, token);
    }

    public static bool TryGetToken(out string? token)
    {
        token = PreferencesService.GetS(TokenKey);
        
        return !string.IsNullOrEmpty(token);
    }

    public static string? GetToken()
    {
        return TryGetToken(out var token) ? token : null;
    }
    
    public static void DeleteToken()
    {
        _pref ??= App.GetService<Preferences>();

        _pref?.Remove(TokenKey);
    }
}