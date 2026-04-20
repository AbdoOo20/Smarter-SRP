using Fly.Shared.Modules.Personnel.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Personnel.Departments;

public partial class DepartmentsPageViewModel : BasePageViewModel<DtoDepartment>
{
    protected override string Endpoint => "Department";
    
    public DepartmentsPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoDepartment>(sender);

        if (model is null)
            return;
        
        var entry = new DepartmentEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new DepartmentEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}