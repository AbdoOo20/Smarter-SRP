using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Stores.DisbursementPermits;

public partial class DisbursementsPermitsPage : INavigableView<DisbursementsPermitsPageViewModel>
{
    public DisbursementsPermitsPageViewModel ViewModel { get; }

    public DisbursementsPermitsPage(DisbursementsPermitsPageViewModel pageViewModel)
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