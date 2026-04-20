using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Personnel.AttendanceAndDeparture;

public partial class AttendanceDeparturePage : INavigableView<AttendanceDeparturePageViewModel>
{
    public AttendanceDeparturePageViewModel ViewModel { get; }

    public AttendanceDeparturePage(AttendanceDeparturePageViewModel pageViewModel)
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