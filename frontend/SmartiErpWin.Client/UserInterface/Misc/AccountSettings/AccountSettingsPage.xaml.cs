using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Misc.AccountSettings;

public partial class AccountSettingsPage : INavigableView<AccountSettingsPageViewModel>
{
    public AccountSettingsPageViewModel ViewModel { get; }

    public AccountSettingsPage(AccountSettingsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
    }

    public void ResetInstance()
    {
        InitializeComponent();
        
        DataContext = null;
        DataContext = this;
    }

    private void BrowseImageFile(object sender, RoutedEventArgs e)
    {
        ViewModel.BrowseImage();
    }
}