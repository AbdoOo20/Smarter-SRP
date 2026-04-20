using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoiceEntryWindow: UserControlBase
{
    public PurchaseReturnInvoiceEntryViewModel ViewModel { get; }
    
    public PurchaseReturnInvoiceEntryWindow(WindowEntryModel<DtoPurchaseReturnInvoice> window)
    {
        ViewModel = new PurchaseReturnInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        Init();
    }

    public PurchaseReturnInvoiceEntryWindow(PurchaseReturnInvoiceEntryViewModel vm)
    {
        ViewModel = vm;
        Init();
    }
    
    private void Init()
    {
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        SynchronisationModel = ViewModel;

        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}