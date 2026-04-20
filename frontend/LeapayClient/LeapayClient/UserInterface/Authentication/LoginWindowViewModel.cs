using CommunityToolkit.Mvvm.Messaging;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Global.Dtos;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.RequestModels;

namespace Leapay.Client.UserInterface.Authentication;

public partial class LoginWindowViewModel : BaseRefreshableViewModel<DtoBase>
{
    public LoginWindow? LoginWindow { get; set; }
    
    private AuthenticationStorageService _authService;

    private BackendService _backendService;

    public LoginWindowViewModel(
        AuthenticationStorageService authService,
        BackendService backendService)
    {
        _authService = authService;
        _backendService = backendService;

        InfoText = this["PleaseSignIn"];
    }

    [ObservableProperty]
    public partial string? Username { get; set; }
    
    [ObservableProperty]
    public partial string? Password { get; set; }
    
    [ObservableProperty]
    public partial string? InfoText { get; set; }

    [RelayCommand]
    private async Task ActionAsync()
    {
        await LoginAsync();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        SetRefreshState(true);
        var password = Password;

        if (string.IsNullOrEmpty(password))
        {
            InfoText = LocaleService["PasswordEmpty"];
            return;
        }

        try
        {
            SetInfo(LocaleService["SigningIn"]);
            var request = await SignInAsync(new SignInRequestModel
            {
                Phone = Username,
                Password = password
            });

            await Task.Delay(1000);

            if (request is { Data: not null, Data.Token: not null })
            {
                var token = request.Data.Token;

                AuthenticationStorageService.SaveToken(token);
                AuthenticateRequests(token);

                var companyInfo = (await GetSpecificInnerOneAsync<DtoCompanyInfo>("Signing/company-settings")).Data;
                //companyInfo.Adapt(App.CLientInfo);
                //
                /*App.CLientInfo = new CLientInfo()
                {
                    Address = companyInfo.Address,
                    Decription = companyInfo.Type,
                    NameAr = companyInfo.Name,
                    NameEn = companyInfo.NameEn,
                    Phone1 = companyInfo.PhoneNumber1,
                    Phone2 = companyInfo.PhoneNumber2,
                    Phone3 = companyInfo.PhoneNumber3,
                    Image = "logo.png", //companyInfo.Image,
                    Email = companyInfo.Email,
                };
                App.CLientInfo.LoadImage();*/

                Success();
            }
            else
            {
                Unsuccessful(request?.ErrorMessage ?? "NO ERROR MESSAGE");
            }
        }
        catch (Exception x)
        {
            Unsuccessful(x.Message);
        }

        return;

        void Unsuccessful(string message)
        {
            SetRefreshState(false);
            InfoText = $"{LocaleService["UnsuccessfulSignIn"]}\n{message}";
        }

        void Success()
        {
            SynchronizeAllBase();
            
            LoginWindow?.Close();

            WeakReferenceMessenger.Default.Send("SignInComplete");
            
            SetRefreshState(false);
        }
    }

    [RelayCommand]
    private static Task ExitAsync()
    {
        App.Shutdown();

        return Task.CompletedTask;
    }
}
