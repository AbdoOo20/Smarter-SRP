using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Purchases.PurchasesInvoice;

public partial class PurchaseInvoiceEntryWindow: UserControlBase
{
    public PurchaseInvoiceEntryViewModel ViewModel { get; }
    
    public PurchaseInvoiceEntryWindow(WindowEntryModel<DtoPurchaseInvoice> window)
    {
        ViewModel = new PurchaseInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        Init();
    }
    
    public PurchaseInvoiceEntryWindow(PurchaseInvoiceEntryViewModel vm)
    {
        ViewModel = vm;
        Init();
    }
    
    private void Init()
    {
        InitializeComponent();
        DataContext = this;
        SynchronisationModel = ViewModel;

        ViewModel.ShowData(DataGrid);
        ViewModel.Costs.ShowData(SecondaryDataGrid);
        
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}