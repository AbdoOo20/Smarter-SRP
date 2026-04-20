using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.SupplyPermits;

public partial class SupplyPermitsPage : UserControlBase
{
    public SupplyPermitsPageViewModel ViewModel { get; set; }
    
    public SupplyPermitsPage(SupplyPermitsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}