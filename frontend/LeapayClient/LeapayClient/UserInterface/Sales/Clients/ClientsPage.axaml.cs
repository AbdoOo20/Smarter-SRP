using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.Clients;

public partial class ClientsPage : UserControlBase
{
    public ClientsPageViewModel ViewModel { get; set; }
    
    public ClientsPage(ClientsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}