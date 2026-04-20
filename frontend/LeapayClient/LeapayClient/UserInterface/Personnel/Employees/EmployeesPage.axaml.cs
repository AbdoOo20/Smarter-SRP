using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Employees;

public partial class EmployeesPage : UserControlBase
{
    public EmployeesPageViewModel ViewModel { get; set; }
    
    public EmployeesPage(EmployeesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}