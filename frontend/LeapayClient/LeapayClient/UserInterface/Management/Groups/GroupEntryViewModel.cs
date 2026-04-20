using System.Collections.ObjectModel;
using Fly.Shared.Modules.Global.Dtos;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Wrappers.INPC;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.UserInterface.Management.Groups;

public partial class GroupPageEntryViewModel : BaseRefreshableEntryViewModel<DtoRoleGroup>
{
    protected override string Endpoint => "Group";
    
    [ObservableProperty]
    public partial ObservableCollection<ObservableSelectableRole> Roles { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ObservableSelectableRole> SearchedRoles { get; set; } = [];
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedItemVisibility))]
    public partial ObservableSelectableRole? SelectedRole { get; set; }
    
    [ObservableProperty]
    public partial string? SelectionState { get; set; }

    [ObservableProperty]
    public partial string? RoleSelectionState { get; set; }

    public bool SelectedItemVisibility => (SelectedRole is not null).ApplyVisibility(true);
    
    public GroupPageEntryViewModel(
        DtoRoleGroup? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        
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