using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoicesPage : UserControlBase
{
    public PurchaseReturnInvoicesPageViewModel ViewModel { get; set; }
    
    public PurchaseReturnInvoicesPage(PurchaseReturnInvoicesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}