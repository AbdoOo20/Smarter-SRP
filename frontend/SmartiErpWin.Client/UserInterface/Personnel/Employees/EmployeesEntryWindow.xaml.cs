using Fly.Shared.Modules.Personnel.Dtos;

namespace SmartiErpWin.Client.UserInterface.Personnel.Employees;

public partial class EmployeesEntryWindow
{
    public EmployeesPageEntryViewModel ViewModel { get; }
    
    public EmployeesEntryWindow(LocalisationService localisationService,
        DtoEmployee model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new EmployeesPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}