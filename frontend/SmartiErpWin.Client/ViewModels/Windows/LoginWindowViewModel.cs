using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Global.Dtos;
using SmartErpWin.Core;
using SmartErpWin.Core.RequestModels;
using SmartiErpWin.Client.Views.Windows;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.ViewModels.Windows;

public partial class LoginWindowViewModel : BaseRefreshableViewModel<DtoBase>
{
    public LoginWindow? LoginWindow { get; set; }

    public PasswordBox? PasswordBox { get; set; }

    private AuthenticationStorageService _authService;

    private BackendService _backendService;

    [ObservableProperty]
    private string _actionText;

    public LoginWindowViewModel(
        SessionService session,
        LocalisationService localeService,
        AuthenticationStorageService authService,
        BackendService backendService,
        HttpClient client) : base(client)
    {
        _authService = authService;
        _backendService = backendService;

        InitBaseModel(localeService, session, null);
        InfoText = LocaleService["PleaseSignInWithPhoneAndPassword"];
        ActionText = LocaleService?["SignInText"];
    }

    [ObservableProperty]
    private string? _infoText;

    [ObservableProperty]
    private string? _username;

    [RelayCommand]
    private async Task ActionAsync()
    {
        await LoginAsync();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        SetRefreshState(true);
        var password = PasswordBox.Password;

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
                var clientInfo = new ClientInfo()
                {
                    Address = companyInfo.Address,
                    Decription = companyInfo.Type,
                    NameAr = companyInfo.Name,
                    NameEn = companyInfo.NameEn,
                    Phone1 = companyInfo.PhoneNumber1,
                    Phone2 = companyInfo.PhoneNumber2,
                    Phone3 = companyInfo.PhoneNumber3,
                    ImagePath = "logo.png", //companyInfo.Image,
                    Email = companyInfo.Email,
                };

                var config = Config.Get;
                config.ClientConfig = clientInfo;
                Config.SaveSettings(config);

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

            LoginWindow.DialogResult = true;
            LoginWindow?.Close();

            SetRefreshState(false);
        }
    }

    /*[RelayCommand]
    private async Task SignUpAsync()
    {
        var password = PasswordBox?.Password;
        
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(Username))
        {
            InfoText = LocaleService?["UsernameOrPasswordEmpty"];
            return;
        }
        
        var request = await Requester.InternalRequestAsync<string>("auth/signup",
            HttpMethod.Get,
            (
                Constants.BodyParam, new SignInRequestModel
                {
                    Username = Username,
                    Password = password
                }
            ));

        if (request is { IsSuccess: true, ResponseText: not null })
        {
            AuthenticationStorageService.SaveToken(request.ResponseText);
        }
        else
        {
            InfoText = LocaleService["UnsuccessfulSignUp"];
            return;
        }

        await BackendService.RefreshAccountData(Session);
        
        SendSynchroniseOnce();
        
        LoginWindow?.Close();
    }*/

    [RelayCommand]
    private static Task ExitAsync()
    {
        Application.Current.Shutdown();

        return Task.CompletedTask;
    }
}
/*
    * ALTER TABLE `tincometax` CHANGE `Ammount` `Ammount` DECIMAL(18,3) NOT NULL;
    * ALTER TABLE `tincometaxdetail` CHANGE `Ammount` `Ammount` DECIMAL(18,3) NOT NULL;
 */