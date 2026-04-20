using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Departments;

public partial class DepartmentsPage : UserControlBase
{
    public DepartmentsPageViewModel ViewModel { get; set; }
    
    public DepartmentsPage(DepartmentsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}