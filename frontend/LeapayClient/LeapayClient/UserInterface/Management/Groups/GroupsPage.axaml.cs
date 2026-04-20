using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Management.Groups;

public partial class GroupsPage : UserControlBase
{
    public GroupsPageViewModel ViewModel { get; set; }
    
    public GroupsPage(GroupsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}