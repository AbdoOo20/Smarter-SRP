using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Factoring.ManufacturingFacility;

public partial class ManufacturingFacilitiesPage : UserControlBase
{
    public ManufacturingFacilitiesPageViewModel ViewModel { get; set; }
    
    public ManufacturingFacilitiesPage(ManufacturingFacilitiesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}