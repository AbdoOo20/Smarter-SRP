using System.Windows.Controls;
using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace SmartiErpWin.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingEntryWindow
{
    public QuantityCreditingEntryViewModel ViewModel { get; }
    
    public QuantityCreditingEntryWindow(LocalisationService localisationService,
        DtoQuantityCreditingInvoice model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new QuantityCreditingEntryViewModel(this,
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
}