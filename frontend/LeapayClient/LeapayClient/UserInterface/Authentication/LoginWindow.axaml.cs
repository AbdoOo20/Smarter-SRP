using Avalonia.Input;
using SukiUI.Controls;

namespace Leapay.Client.UserInterface.Authentication;

public partial class LoginWindow: SukiWindow
{
    public LoginWindowViewModel ViewModel { get; }

    public LoginWindow(LoginWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        
        ViewModel = viewModel;
        ViewModel.LoginWindow = this;
        
        DataContext = this;

        InitializeComponent();
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        /*if (e is { OriginalSource: Wpf.Ui.Controls.PasswordBox, Key: Key.Enter })
        {
            FallbackToLogin();
            return;
        }*/
        
    }

    private void FallbackToLogin()
    {
        ViewModel.LoginCommand.Execute(null);
    }
}