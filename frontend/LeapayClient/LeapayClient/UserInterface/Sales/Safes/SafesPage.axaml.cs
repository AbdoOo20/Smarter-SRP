using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.Safes;

public partial class SafesPage : UserControlBase
{
    public SafesPageViewModel ViewModel { get; set; }
    
    public SafesPage(SafesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}