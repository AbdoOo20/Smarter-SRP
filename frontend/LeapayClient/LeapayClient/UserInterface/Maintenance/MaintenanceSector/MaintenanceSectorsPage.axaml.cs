using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceSector;

public partial class MaintenanceSectorsPage : UserControlBase
{
    public MaintenanceSectorsPageViewModel ViewModel { get; set; }
    
    public MaintenanceSectorsPage(MaintenanceSectorsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}