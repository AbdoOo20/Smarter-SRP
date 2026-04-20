using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.SalesReturnInvoice;

public partial class SellReturnInvoiceEntryWindow: UserControlBase
{
    public SellReturnInvoiceEntryViewModel ViewModel { get; }
    
    public SellReturnInvoiceEntryWindow(WindowEntryModel<DtoSellReturnInvoice> window)
    {
        ViewModel = new SellReturnInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        Init();
    }

    public SellReturnInvoiceEntryWindow(SellReturnInvoiceEntryViewModel vm)
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
        
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}