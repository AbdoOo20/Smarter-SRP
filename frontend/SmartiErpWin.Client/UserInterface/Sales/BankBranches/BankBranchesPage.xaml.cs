using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Sales.BankBranches;

public partial class BankBranchesPage : INavigableView<BankBranchesPageViewModel>
{
    public BankBranchesPageViewModel ViewModel { get; }

    public BankBranchesPage(BankBranchesPageViewModel  pageViewModel)
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

        ToolbarPrefab.DataSizeChanged(sender, e);
    }
}