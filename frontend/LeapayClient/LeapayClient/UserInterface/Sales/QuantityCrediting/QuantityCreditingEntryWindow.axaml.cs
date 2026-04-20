using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingEntryWindow: UserControlBase
{
    public QuantityCreditingEntryViewModel ViewModel { get; }
    
    public QuantityCreditingEntryWindow(WindowEntryModel<DtoQuantityCreditingInvoice> window)
    {
        ViewModel = new QuantityCreditingEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}