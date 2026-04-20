using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Accounting.AccountingTree;

public partial class AccountingTreePage : INavigableView<AccountingTreePageViewModel>
{
    public AccountingTreePageViewModel ViewModel { get; }

    public AccountingTreePage(AccountingTreePageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        
    }
}