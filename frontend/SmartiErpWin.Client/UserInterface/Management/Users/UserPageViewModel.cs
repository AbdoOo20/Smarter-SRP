using Fly.Shared.Modules.Global.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Management.Users;

public partial class UserPageViewModel : BasePageViewModel<DtoUser>
{
    protected override string Endpoint => "User";
    
    public UserPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoUser>(sender);

        if (model is null)
            return;
        
        var entry = new UserEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new UserEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }

    protected override List<DtoUser> ApplyModifications(IEnumerable<DtoUser> models)
    {
        if(!Session?.UserInfo?.Roles.Any(r => r.Key is "GHOST") ?? false)
        {
            models = models.Where(m => !m.IsGhost);
        }

        return models.ToList();
    }
}