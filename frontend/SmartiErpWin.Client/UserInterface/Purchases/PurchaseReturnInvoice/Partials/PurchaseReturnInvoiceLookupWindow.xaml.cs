namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice.Partials;

public partial class PurchaseReturnInvoiceLookupWindow
{
    public PurchaseReturnInvoiceWindowViewModel ViewModel { get; }

    public PurchaseReturnInvoiceLookupWindow(PurchaseReturnInvoiceWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        ViewModel.LoginWindow = this;
        
        DataContext = this;

        InitializeComponent();
    }
}