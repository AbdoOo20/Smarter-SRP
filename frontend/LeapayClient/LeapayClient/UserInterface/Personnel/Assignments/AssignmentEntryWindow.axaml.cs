using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Assignments;

public partial class AssignmentEntryWindow: UserControlBase
{
    public AssignmentPageEntryViewModel ViewModel { get; }
    
    public AssignmentEntryWindow(WindowEntryModel<DtoEmployeeAssignment> window)
    {
        ViewModel = new AssignmentPageEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}