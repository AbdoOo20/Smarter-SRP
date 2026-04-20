using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Common.ReceiptLookupPurchase;

public partial class PurchaseReceiptLookupWindow: UserControlBase
{
    public PurchaseReceiptLookupViewModel ViewModel { get; }
    
    public PurchaseReceiptLookupWindow(PurchaseReceiptLookupViewModel viewModel)
    {
        ViewModel = viewModel;
        SynchronisationModel = viewModel;

        InitializeComponent();
        DataContext = this;
    }
}