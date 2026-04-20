using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Assignments;

public partial class AssignmentsPage : UserControlBase
{
    public AssignmentsPageViewModel ViewModel { get; set; }
    
    public AssignmentsPage(AssignmentsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}