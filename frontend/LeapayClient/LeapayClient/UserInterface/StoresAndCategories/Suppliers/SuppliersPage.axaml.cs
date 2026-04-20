using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Suppliers;

public partial class SuppliersPage : UserControlBase
{
    public SuppliersPageViewModel ViewModel { get; set; }
    
    public SuppliersPage(SuppliersPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}