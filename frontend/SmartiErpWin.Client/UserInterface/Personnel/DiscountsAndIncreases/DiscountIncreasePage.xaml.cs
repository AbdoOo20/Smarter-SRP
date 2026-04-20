using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Personnel.DiscountsAndIncreases;

public partial class DiscountIncreasePage : INavigableView<DiscountIncreasePageViewModel>
{
    public DiscountIncreasePageViewModel ViewModel { get; }

    public DiscountIncreasePage(DiscountIncreasePageViewModel pageViewModel)
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