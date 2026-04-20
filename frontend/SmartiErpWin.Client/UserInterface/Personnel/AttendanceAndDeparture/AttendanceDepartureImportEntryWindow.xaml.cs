using Fly.Shared.Modules.Personnel.Dtos;

namespace SmartiErpWin.Client.UserInterface.Personnel.AttendanceAndDeparture;

public partial class AttendanceDepartureImportEntryWindow
{
    public AttendanceDepartureImportEntryViewModel ViewModel { get; }
    
    public AttendanceDepartureImportEntryWindow(LocalisationService localisationService,
        DtoAttendanceDeparture model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new AttendanceDepartureImportEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}