using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public partial class PurchasesInvoiceEntryWindow
{
    public PurchasesInvoiceEntryViewModel ViewModel { get; }
    
    public PurchasesInvoiceEntryWindow(LocalisationService localisationService,
        DtoPurchaseInvoice model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new PurchasesInvoiceEntryViewModel(this,
            localisationService, model, type, client, mapper, pageType);
        
        InitializeComponent();
        DataContext = this;

        ViewModel.EntryState = EntryState;
        ViewModel.SecondaryEntryState = SecondaryEntryState;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.ShowSecondaryDataPanel(SecondaryDataGrid);
    }

    private void PaymentSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
    {
        ViewModel.CheckVisibilities();
    }
}