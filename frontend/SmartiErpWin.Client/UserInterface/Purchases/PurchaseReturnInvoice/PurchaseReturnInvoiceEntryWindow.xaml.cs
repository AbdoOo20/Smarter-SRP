using System.Windows.Controls;
using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoiceEntryWindow
{
    public PurchaseReturnInvoiceEntryViewModel ViewModel { get; }

    public PurchaseReturnInvoiceEntryWindow(LocalisationService localisationService,
        DtoPurchaseReturnInvoice model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new PurchaseReturnInvoiceEntryViewModel(this,
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