using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.SalesPoint.SuspendedInvoices;

public partial class SuspendedInvoicesWindow: UserControlBase
{
    public SuspendedInvoicesViewModel ViewModel { get; }
    
    public SuspendedInvoicesWindow(SuspendedInvoicesViewModel viewModel)
    {
        ViewModel = viewModel;
        
        InitializeComponent();
        DataContext = this;
    }
    
    private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.OnSearch((sender as TextBox)?.Text ?? "");
    }
}