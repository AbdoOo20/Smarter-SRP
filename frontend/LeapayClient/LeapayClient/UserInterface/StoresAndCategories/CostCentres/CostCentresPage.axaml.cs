using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.CostCentres;

public partial class CostCentresPage : UserControlBase
{
    public CostCentresPageViewModel ViewModel { get; set; }
    
    public CostCentresPage(CostCentresPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}