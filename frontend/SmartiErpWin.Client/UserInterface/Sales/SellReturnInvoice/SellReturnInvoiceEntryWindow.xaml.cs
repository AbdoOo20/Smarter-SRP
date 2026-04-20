using System.Windows.Controls;
using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace SmartiErpWin.Client.UserInterface.Sales.SellReturnInvoice;

public partial class SellReturnInvoiceEntryWindow
{
    public SellReturnInvoiceEntryViewModel ViewModel { get; }

    public SellReturnInvoiceEntryWindow(LocalisationService localisationService,
        DtoSellReturnInvoice model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new SellReturnInvoiceEntryViewModel(this,
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
    }

    private void OnRepresentativeChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetClient();
    }
}