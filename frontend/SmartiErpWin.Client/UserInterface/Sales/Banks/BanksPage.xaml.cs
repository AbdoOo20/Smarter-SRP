using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Sales.Banks;

public partial class BanksPage : INavigableView<BanksPageViewModel>
{
    public BanksPageViewModel ViewModel { get; }

    public BanksPage(BanksPageViewModel pageViewModel)
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