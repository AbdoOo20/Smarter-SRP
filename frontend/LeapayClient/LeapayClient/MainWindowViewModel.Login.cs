using Leapay.Client.Services;
using Leapay.Client.UserInterface.Misc.Configuration;
using SmartErpWin.Core.RequestModels;

namespace Leapay.Client;

public partial class MainWindowViewModel
{
    [ObservableProperty]
    public partial string LoginInfoText { get; set; }
    
    [ObservableProperty]
    public partial string Username { get; set; }
    
    [ObservableProperty]
    public partial string Password { get; set; }
        
    [ObservableProperty]
    public partial bool IsLoggingIn { get; set; }
    
    [ObservableProperty]
    public partial bool ShouldShowUi { get; set; }
   
    [ObservableProperty]
    public partial bool ShouldShowLoading { get; set; }
    
    [ObservableProperty]
    public partial bool ShouldShowLogin { get; set; }
    
    public void SwitchToSignIn()
    {
        ShouldShowUi = false;
        ShouldShowLoading = false;
        ShouldShowLogin = true;
        ShouldShowUpdating = false;
        IsUserSelectingBackend = false;
        ShouldShowSetup = false;
        LoginInfoText = string.Empty;
    }
    
    public void SwitchToLoading()
    {
        ShouldShowUi = false;
        ShouldShowLogin = false;
        ShouldShowLoading = true;
        ShouldShowUpdating = false;
        IsUserSelectingBackend = false;
        ShouldShowSetup = false;
        LoginInfoText = LocaleService["Loading"];
    }
    
    public void SwitchToUpdating()
    {
        ShouldShowUi = false;
        ShouldShowLogin = false;
        ShouldShowLoading = false;
        IsUserSelectingBackend = false;
        ShouldShowSetup = false;

        ShouldShowUpdating = true;
    }
    
    public void SwitchToUi()
    {
        ShouldShowUi = true;
        ShouldShowLoading = false;
        ShouldShowLogin = false;
        ShouldShowUpdating = false;
        IsUserSelectingBackend = false;
        ShouldShowSetup = false;

        LoadingTitle = string.Empty;
    }
    
    public event EventHandler? SignInComplete;
    
    [RelayCommand]
    private async Task LoginAsync()
    {
        IsLoggingIn = true;
        
        var password = Password;

        if (string.IsNullOrEmpty(password))
        {
            LoginInfoText = this["PasswordEmpty"];
            IsLoggingIn = false;
            return;
        }

        try
        {
            SetInfo(this["SigningIn"]);
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

                await App.GetService<CompanyInfoPageViewModel>().RefreshCompanyDataAsync();

                Success();
            }
            else
            {
                Unsuccessful(request?.Exception?.ToString() ?? request.ErrorMessage ?? "NO ERROR MESSAGE");
            }
        }
        catch (Exception x)
        {
            Unsuccessful(x.Message);
        }

        return;

        void Unsuccessful(string message)
        {
            IsLoggingIn = false;
            LoginInfoText = $"{this["UnsuccessfulSignIn"]}\n{message}";
        }

        void Success()
        {
            IsLoggingIn = false;
            SignInComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}