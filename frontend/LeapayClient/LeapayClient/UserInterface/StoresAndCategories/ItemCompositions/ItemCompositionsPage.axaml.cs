using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemCompositions;

public partial class ItemCompositionsPage : UserControlBase
{
    public ItemCompositionsPageViewModel ViewModel { get; set; }
    
    public ItemCompositionsPage(ItemCompositionsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}