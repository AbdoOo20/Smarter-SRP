using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;
using SmartErpWin.Core;
using WinRT;
using static Windows.Security.Credentials.KeyCredentialManager;

namespace SmartiErpWin.Client.WinHelpers;

public static class CredentialManager
{
    private static volatile IObjectReference _REF_global_WINDOWS_SECURITY_CREDS
        = Interlocked.CompareExchange<IObjectReference>(
            ref _REF_global_WINDOWS_SECURITY_CREDS,
            null,
            null);
    
    public static unsafe IAsyncOperation<UserConsentVerificationResult> RequestVerificationAsync(
        IObjectReference obj,
        string message)
    {
        IntPtr num = new();
        try
        {
            var pin = new MarshalString.Pinnable(message);
            fixed (char* chPtr = &pin.GetPinnableReference())
            {
                ExceptionHelpers.ThrowExceptionForHR((int)*(IntPtr*)(*(IntPtr*)
                    (void*)obj.ThisPtr + new IntPtr(7) + chPtr));
                
                return MarshalInterface<IAsyncOperation<UserConsentVerificationResult>>.FromAbi(num);
            }
        }
        finally
        {
            MarshalInterface<IAsyncOperation<UserConsentVerificationResult>>.DisposeAbi(num);
        }
    }
    
    public static async Task<bool> PoseConsentExternAsyncOp()
    {
        try
        {
            if (Config.Get?.UseWindowsHello is not true)
                return true;
            
            if (Environment.OSVersion.Version.Major < 10)
                return true;

            var locale = App.GetService<LocalisationService>();
        
            var consent = await UserConsentVerifier.CheckAvailabilityAsync();

            if (consent is not UserConsentVerifierAvailability.Available)
                return await FallbackToCredentialManagerExternAsyncOp(locale);

            var request = UserConsentVerifier.RequestVerificationAsync(locale["SignInWithWindowsPassword"]).GetResults();

            return request is UserConsentVerificationResult.Verified;
        }
        catch (Exception x)
        {
            Error(x, "Failed to use Windows provided credential manager due to an error.");
            return true;
        }
    }
    
    private static async Task<bool> FallbackToCredentialManagerExternAsyncOp(LocalisationService locale)
    {
        if (!await IsSupportedAsync())
            return true;

        var creation = await RequestCreateAsync(nameof(SmartiErpWin),
                KeyCredentialCreationOption.ReplaceExisting);

        var open = await OpenAsync(creation.Credential.Name);

        return open.Status is KeyCredentialStatus.Success;
    }
    
    public static unsafe IAsyncOperation<UserConsentVerifierAvailability> CheckAvailabilityAsync()
    {
        IntPtr num = new();
        try
        {
            ExceptionHelpers.ThrowExceptionForHR((int)*(IntPtr*)(void*) + new IntPtr(33));
            return MarshalInterface<IAsyncOperation<UserConsentVerifierAvailability>>.FromAbi(num);
        }
        finally
        {
            MarshalInterface<IAsyncOperation<UserConsentVerifierAvailability>>.DisposeAbi(num);
        }
    }
}