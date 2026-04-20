using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Sales.Clients;

public partial class ClientsPage : INavigableView<ClientsPageViewModel>
{
    public ClientsPageViewModel ViewModel { get; }

    public ClientsPage(ClientsPageViewModel pageViewModel)
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