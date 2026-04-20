using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Purchases.PurchasesInvoice;

public partial class PurchaseInvoicesPage : UserControlBase
{
    public PurchaseInvoicesPageViewModel ViewModel { get; set; }
    
    public PurchaseInvoicesPage(PurchaseInvoicesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}