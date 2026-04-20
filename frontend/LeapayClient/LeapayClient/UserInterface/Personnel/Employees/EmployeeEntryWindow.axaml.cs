using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Employees;

public partial class EmployeeEntryWindow: UserControlBase
{
    public EmployeePageEntryViewModel ViewModel { get; }
    
    public EmployeeEntryWindow(WindowEntryModel<DtoEmployee> window)
    {
        ViewModel = new EmployeePageEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}