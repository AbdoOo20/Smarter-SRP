using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemVariance;

public partial class ItemVariancesPage : UserControlBase
{
    public ItemVariancesPageViewModel ViewModel { get; set; }
    
    public ItemVariancesPage(ItemVariancesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}