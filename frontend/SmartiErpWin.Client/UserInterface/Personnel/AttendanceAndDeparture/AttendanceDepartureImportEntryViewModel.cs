using Fly.Shared.Modules.Personnel.Dtos;

namespace SmartiErpWin.Client.UserInterface.Personnel.AttendanceAndDeparture;

public partial class AttendanceDepartureImportEntryViewModel: BaseEntryViewModel<DtoAttendanceDeparture>
{
    protected override string Endpoint => "AttendanceDeparture";

    [ObservableProperty]
    private string? _fileLocation;
    
    public AttendanceDepartureImportEntryViewModel(Window window,
        LocalisationService locale,
        DtoAttendanceDeparture? model,
        EntryType entryType,
        HttpClient client,
        Type pageType): base(client)
    {
        Model = model ?? new();
        EntryWindow = window;
        EntryType = entryType;
        PageType = pageType;

        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);
    }

    [RelayCommand]
    private async Task ImportFromExcelAsync()
    {
        if (string.IsNullOrEmpty(FileLocation))
        {
            await DialogueServices.ShowMessageAsync("Error", "ChooseExcelFile");
            return;
        }
        
        try
        {
            var list = ExportServices.ReadAttendanceFromExcel(FileLocation);
        }
        catch(Exception x)
        {
            await DialogueServices.ShowMessageAsync("Error reading Excel", x.ToString());
        }
    }

    [RelayCommand]
    private void SelectFile()
    {
        FileLocation = ExportServices.SelectExcelFile();
    }
}