using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Factoring.FactoringInvoice;

public partial class FactoringInvoicesPage : UserControlBase
{
    public FactoringInvoicesPageViewModel ViewModel { get; set; }
    
    public FactoringInvoicesPage(FactoringInvoicesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}