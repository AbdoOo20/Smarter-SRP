using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Reports.AccountStatementReport;

public partial class AccountStatementReportPage : INavigableView<AccountStatementReportPageViewModel>
{
    public AccountStatementReportPageViewModel ViewModel { get; }

    public AccountStatementReportPage(AccountStatementReportPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;
        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DataScroller.MaxHeight = e.NewSize.Height * DataScrollerHeightMultiplier;
        DataScroller.MaxWidth = e.NewSize.Width * DataScrollerWidthMultiplier;
        ViewModel.MinDataGridHeight = e.NewSize.Height * DataGridHeightMultiplier;

        ToolbarPrefab.DataSizeChanged(sender, e, 0.8);
    }
}