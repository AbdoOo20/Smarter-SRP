using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Accounting.Entries;

public partial class EntryPage : INavigableView<EntryPageViewModel>
{
    public EntryPageViewModel ViewModel { get; }

    public EntryPage(EntryPageViewModel pageViewModel)
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
        ViewModel.MinDataGridHeight = e.NewSize.Height * DataGridHeightMultiplier * 0.8;
 
        ToolbarPrefab.DataSizeChanged(sender, e);
    }
}