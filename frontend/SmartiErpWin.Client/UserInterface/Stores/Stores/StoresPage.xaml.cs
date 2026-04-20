using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Stores.Stores;

public partial class StoresPage : INavigableView<StoresPageViewModel>
{
    public StoresPageViewModel ViewModel { get; }

    public StoresPage(StoresPageViewModel pageViewModel)
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