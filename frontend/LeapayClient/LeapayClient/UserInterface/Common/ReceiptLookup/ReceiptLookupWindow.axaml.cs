using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Common.ReceiptLookup;

public partial class ReceiptLookupWindow: UserControlBase
{
    public ReceiptLookupViewModel ViewModel { get; }
    
    public ReceiptLookupWindow(ReceiptLookupViewModel viewModel)
    {
        ViewModel = viewModel;
        SynchronisationModel = viewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}