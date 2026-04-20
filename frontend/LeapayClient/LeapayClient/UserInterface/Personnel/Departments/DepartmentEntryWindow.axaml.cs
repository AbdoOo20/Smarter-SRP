using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Departments;

public partial class DepartmentEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoDepartment> ViewModel { get; }
    
    public DepartmentEntryWindow(WindowEntryModel<DtoDepartment> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoDepartment>(window.Model,
            "Department",
            window.EntryType,
            window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}