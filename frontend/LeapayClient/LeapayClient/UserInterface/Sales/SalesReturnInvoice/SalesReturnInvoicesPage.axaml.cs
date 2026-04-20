using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.SalesReturnInvoice;

public partial class SellReturnInvoicesPage : UserControlBase
{
    public SellReturnInvoicesPageViewModel ViewModel { get; set; }
    
    public SellReturnInvoicesPage(SellReturnInvoicesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}