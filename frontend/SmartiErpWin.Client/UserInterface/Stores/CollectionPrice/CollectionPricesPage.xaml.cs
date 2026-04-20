using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Stores.CollectionPrice;

public partial class CollectionPricesPage : INavigableView<CollectionPricesPageViewModel>
{
    public CollectionPricesPageViewModel ViewModel { get; }

    public CollectionPricesPage(CollectionPricesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        InitializeComponent();
        
        DataContext = this;
        ViewModel.SetDataGrid(dataGrid);
        //ViewModel.ShowData(DataGrid);
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        //DataScroller.MaxHeight = e.NewSize.Height * DataScrollerHeightMultiplier;
        //DataScroller.MaxWidth = e.NewSize.Width * DataScrollerWidthMultiplier;
        //ViewModel.MinDataGridHeight = e.NewSize.Height * DataGridHeightMultiplier;
    
        ToolbarPrefab.DataSizeChanged(sender, e);
    }

    private void dataGrid_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
    {
    }

}