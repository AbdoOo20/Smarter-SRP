using Fly.Shared.Modules.Personnel.Dtos;

namespace SmartiErpWin.Client.UserInterface.Personnel.EmployeeAssignments;

public partial class AssignmentEntryWindow
{
    public AssignmentsPageEntryViewModel ViewModel { get; }
    
    public AssignmentEntryWindow(LocalisationService localisationService,
        DtoEmployeeAssignment model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new AssignmentsPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}