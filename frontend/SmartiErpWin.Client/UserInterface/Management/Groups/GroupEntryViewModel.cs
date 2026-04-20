using System.Collections.ObjectModel;
using Fly.Shared.Modules.Global.Dtos;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Helpers;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.Wrappers.INPC;

namespace SmartiErpWin.Client.UserInterface.Management.Groups;

public sealed partial class GroupEntryViewModel : BaseRefreshableEntryViewModel<DtoRoleGroup>
{
    protected override string Endpoint => "Group";
    
    [ObservableProperty]
    private ObservableCollection<ObservableSelectableRole> _roles = [];

    [ObservableProperty]
    private ObservableCollection<ObservableSelectableRole> _searchedRoles = [];
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedItemVisibility))]
    private ObservableSelectableRole? _selectedRole;

    [ObservableProperty]
    private string? _selectionState;
    
    [ObservableProperty]
    private string? _roleSelectionState;
    
    public Visibility SelectedItemVisibility => (SelectedRole is not null).ApplyVisibility(true);
    
    public GroupEntryViewModel(Window window,
        LocalisationService locale,
        DtoRoleGroup? model,
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

        Startup();
    }

    public void OnState()
    {
        if (SelectedRole is null)
            return;

        var oldId = SelectedRole.Id;
        
        Roles[Roles.IndexOf(Roles.FirstOrDefault(r => r.Id == SelectedRole.Id))] = SelectedRole;
        SelectedRole = Roles[Roles.IndexOf(Roles.FirstOrDefault(r => r.Id == oldId))];
        SelectedRole.Refresh();
        UpdateSelectionState(SelectedRole);
    }

    partial void OnSelectedRoleChanged(ObservableSelectableRole? oldValue, ObservableSelectableRole? newValue)
    {
        UpdateSelectionState(SelectedRole);
    }

    private void Startup()
    {
        RefreshState(async() =>
        {
            var roles = await GetInnerAllSpecificAsync<ObservableSelectableRole>("Roles", Model.Id);
        
            roles.Data.Observe(Roles);
            Roles.ObserveForEach(r => r.Refresh());
            OnSearch(null);
            SelectedRole = SearchedRoles.FirstOrDefault();
            UpdateSelectionState(SelectedRole);
        });
        
        AddValidation(() => Model.Name, optional: false);
        AddValidation(() => Model.Roles, [ new(r => r.Count != 0, "SelectAtLeastOnePermission") ]);
    }

    public void OnSearch(string? search)
    {
        var searched = SearchHelper.Search(search, Roles.ToList());
        
        searched.Observe(SearchedRoles);
        SearchedRoles.ObserveForEach(r => r.Refresh());
    }
    
    protected override void PreSaveActions()
    {
        Model.Roles = (from role in Roles
            from key in role.Keys
            where key.IsChecked
            select key.Id).ToList();
    }

    [RelayCommand]
    private void OnSelection()
    {
        ChangeCheck(IsNotChecked(SelectedRole));
    }
    
    [RelayCommand]
    private void OnRoleSelection()
    {
        var roleList = Roles.ToList();
        var state = !AreAllRolesChecked();
        
        foreach (var role in roleList)
        {
            ChangeRoleCheck(role, state);
        }
    }
    
    private void ChangeCheck(bool state)
    {
        ChangeRoleCheck(SelectedRole, state);
    }

    [RelayCommand]
    private void OnSwitchCheck(ObservableRoleKey roleKey)
    {
        roleKey.IsChecked = !roleKey.IsChecked;
        SelectedRole?.Refresh();
    }

    [RelayCommand]
    private void OnSwitchRoleCheck(ObservableSelectableRole? role = null)
    {
        role ??= SelectedRole;
        ChangeRoleCheck(role, IsNotChecked(role));
    }

    private void ChangeRoleCheck(ObservableSelectableRole? role, bool state)
    {
        foreach (var roleKey in role?.Keys ?? [])
        {
            roleKey.IsChecked = state;
        }
        
        role?.Refresh();
        UpdateSelectionState(role);
    }
    
    private void UpdateSelectionState(ObservableSelectableRole? role)
    {
        var state = !IsNotChecked(role);
        var roleState = !AreAllRolesChecked();
        
        SelectionState = state ? this["DeselectAll"] : this["SelectAll"];
        RoleSelectionState = roleState ? this["SelectAll"] : this["DeselectAll"];
    }
    
    private static bool IsNotChecked(ObservableSelectableRole? role)
    {
        return !role?.Keys.All(k => k.IsChecked) ?? false;
    }

    private bool AreAllRolesChecked()
    {
        return Roles.All(r => r.Keys.All(k => k.IsChecked));
    }
}