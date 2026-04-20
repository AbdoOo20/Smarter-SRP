using Fly.Shared.Modules.Personnel.Dtos;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.UserInterface.Personnel.Departments;

public partial class DepartmentEntryWindow
{
    public CommonPageEntryViewModel<DtoDepartment> ViewModel { get; }
    
    public DepartmentEntryWindow(LocalisationService localisationService,
        DtoDepartment model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CommonPageEntryViewModel<DtoDepartment>(this,
            localisationService, model, type, "Department", client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}