using Fly.Shared.Modules.Global.Dtos;

namespace SmartiErpWin.Client.UserInterface.Management.Users;

public class UserPageEntryViewModel : BaseEntryViewModel<DtoUser>
{
    protected override string Endpoint => "User";
    
    public UserPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoUser? model,
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

        Refresh();
    }

    private async void Refresh()
    {
        await PrepareComboBoxAsync("Groups", Model.RoleGroupId);
    }

    protected override void PreSaveActions()
    {
        Model.RoleGroupId = ComboModelBase.GetSelectedItemId();
        //Model.Password
    }
}