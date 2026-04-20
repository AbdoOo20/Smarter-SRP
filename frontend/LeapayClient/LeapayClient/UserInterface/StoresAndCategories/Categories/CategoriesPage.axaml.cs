using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Categories;

public partial class CategoriesPage : UserControlBase
{
    public CategoriesPageViewModel ViewModel { get; set; }
    
    public CategoriesPage(CategoriesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}