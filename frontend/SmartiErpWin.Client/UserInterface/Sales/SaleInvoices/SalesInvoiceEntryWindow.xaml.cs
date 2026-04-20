using System.Windows.Controls;
using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoiceEntryWindow
{
    public SalesInvoiceEntryViewModel ViewModel { get; }
    
    public SalesInvoiceEntryWindow(LocalisationService localisationService,
        DtoSalesInvoice model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new SalesInvoiceEntryViewModel(this,
            localisationService, model, type, client, mapper, pageType);
        
        InitializeComponent();

        ViewModel.EntryState = EntryState;
        
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
    }

    private void StoreSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.InformComboChange();
    }

    private void OnDiscountTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!decimal.TryParse((sender as TextBox)?.Text, out var val))
            return;
        
        ViewModel.CalculatePrice(val);
    }

    private void OnStorageTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetStore();
        ViewModel.InformComboChange();
    }

    private void OnRepresentativeChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetClient();
    }
}