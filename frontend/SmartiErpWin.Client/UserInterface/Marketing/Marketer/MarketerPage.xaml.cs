using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Marketing.Marketer;

public partial class MarketerDisbursementPermitPage : INavigableView<MarketerDisbursementPermitPageViewModel>
{
    public MarketerDisbursementPermitPageViewModel ViewModel { get; }

    public MarketerDisbursementPermitPage(MarketerDisbursementPermitPageViewModel pageViewModel)
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