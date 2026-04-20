using Fly.Shared.Modules.Personnel.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Personnel.EmployeeAssignments;

public partial class AssignmentsPageViewModel : BasePageViewModel<DtoEmployeeAssignment>
{
    protected override string Endpoint => "EmployeeAssignment";
    
    public AssignmentsPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoEmployeeAssignment>(sender);

        if (model is null)
            return;
        
        var entry = new AssignmentEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new AssignmentEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}