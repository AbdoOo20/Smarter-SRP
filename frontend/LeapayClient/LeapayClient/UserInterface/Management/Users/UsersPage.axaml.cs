using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Management.Users;

public partial class UsersPage : UserControlBase
{
    public UsersPageViewModel ViewModel { get; set; }
    
    public UsersPage(UsersPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}