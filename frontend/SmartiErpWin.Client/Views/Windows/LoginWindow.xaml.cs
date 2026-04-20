using SmartiErpWin.Client.ViewModels.Windows;
using System.Windows.Input;
using Wpf.Ui;

namespace SmartiErpWin.Client.Views.Windows;

public partial class LoginWindow
{
    public LoginWindowViewModel ViewModel { get; }

    public LoginWindow(
        LoginWindowViewModel viewModel,
        INavigationService navigationService,
        IServiceProvider serviceProvider,
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService
    )
    {
        ViewModel = viewModel;
        ViewModel.LoginWindow = this;
        ViewModel.PasswordBox = PasswordBox;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.PasswordBox = PasswordBox;
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e is { OriginalSource: Wpf.Ui.Controls.PasswordBox, Key: Key.Enter })
        {
            FallbackToLogin();
            return;
        }
        
        HelperServices.HandlePreviewKey(e);
    }

    private void FallbackToLogin()
    {
        ViewModel.LoginCommand.Execute(null);
    }
}