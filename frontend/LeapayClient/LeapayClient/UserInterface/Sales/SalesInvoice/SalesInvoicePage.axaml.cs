using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.SalesInvoice;

public partial class SalesInvoicePage : UserControlBase
{
    public SalesInvoicePageViewModel ViewModel { get; set; }
    
    public SalesInvoicePage(SalesInvoicePageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}