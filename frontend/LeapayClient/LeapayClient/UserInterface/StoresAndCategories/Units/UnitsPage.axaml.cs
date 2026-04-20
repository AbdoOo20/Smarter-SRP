using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Units;

public partial class UnitsPage : UserControlBase
{
    public UnitsPageViewModel ViewModel { get; set; }
    
    public UnitsPage(UnitsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}