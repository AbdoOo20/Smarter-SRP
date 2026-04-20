using Fly.Shared.Modules.Global.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Management.Groups;

public sealed partial class GroupPageViewModel : BasePageViewModel<DtoRoleGroup>
{
    protected override string Endpoint => "Group";
    
    public GroupPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoRoleGroup>(sender);

        if (model is null)
            return;
        
        var entry = new GroupEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new GroupEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}