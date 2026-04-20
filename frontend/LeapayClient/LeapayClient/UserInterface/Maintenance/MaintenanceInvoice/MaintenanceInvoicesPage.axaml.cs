using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;

public partial class MaintenanceInvoicesPage : UserControlBase
{
    public MaintenanceInvoicesPageViewModel ViewModel { get; set; }
    
    public MaintenanceInvoicesPage(MaintenanceInvoicesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}