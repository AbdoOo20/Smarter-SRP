using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Accounting.ExpenseTypes;

public partial class ExpenseTypesPage : INavigableView<ExpenseTypesPageViewModel>
{
    public ExpenseTypesPageViewModel ViewModel { get; }

    public ExpenseTypesPage(ExpenseTypesPageViewModel pageViewModel)
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