using System.ComponentModel;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;
using SmartiErpWin.Client.DtoModels.PersonnelModels;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Personnel.AttendanceAndDeparture;

public partial class AttendanceDeparturePageViewModel : BasePageViewModel<DtoAttendanceDeparture>
{
    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Now;
    
    public AttendanceDeparturePageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new AttendanceDepartureImportEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(SelectedDate))
        {
            RefreshAsync();
        }
    }
    
    
    // needs further testing

    private static IEnumerable<AttendanceDepartureDataModel>
        ConvertDatabaseModelsToDataGridModels(
            IEnumerable<DtoAttendanceDeparture> models) => models
            .Where(dto => dto.Date.HasValue)
            .GroupBy(dto => new { dto.EmployeeName, dto.Date.Value.Date })
            .ToList()
            .Select(group => new AttendanceDepartureDataModel
            {
                EmployeeName = group.Key.EmployeeName,
                AttendanceDate = group.Where(
                        g => g.Mode == AttendanceDepartureMode.Attendance)
                    .Select(g => g.Date.Value)
                    .FirstOrDefault(),
                DepartureDate = group.Where(
                        g => g.Mode == AttendanceDepartureMode.Departure)
                    .Select(g => g.Date.Value)
                    .FirstOrDefault()
            })
            .AsEnumerable();
}