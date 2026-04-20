using SmartErpWin.Core;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoicePage : INavigableView<SalesInvoicePageViewModel>
{
    public SalesInvoicePageViewModel ViewModel { get; }

    public SalesInvoicePage(SalesInvoicePageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DataScroller.MaxHeight = e.NewSize.Height * Constants.DataScrollerHeightMultiplier;
        DataScroller.MaxWidth = e.NewSize.Width * Constants.DataScrollerWidthMultiplier;
        ViewModel.MinDataGridHeight = e.NewSize.Height * Constants.DataGridHeightMultiplier;
    
        ToolbarPrefab.DataSizeChanged(sender, e);
    }
}