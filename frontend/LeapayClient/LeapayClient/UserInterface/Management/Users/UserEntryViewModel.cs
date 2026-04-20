using System.Collections.ObjectModel;
using System.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Management.Users;

public partial class UserPageEntryViewModel : BaseEntryViewModel<DtoUser>
{
    protected override string Endpoint => "User";

    public SuperComboVm<DtoUser> Groups { get; } = new();
    
    public SuperComboVm<DtoUser> StoragesCombo { get; } = new();
    
    public SuperComboVm<DtoUser> SafesCombo { get; } = new();
    
    public SuperComboVm<DtoUser> BanksCombo { get; } = new();
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedStorages))]
    [field: Bindable(true)]
    public partial ObservableCollection<ComboModel> Storages { get; set; } = [];
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedBanks))]
    [field: Bindable(true)]
    public partial ObservableCollection<ComboModel> Banks { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ComboModel> SelectedBanks { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ComboModel> SelectedStorages { get; set; } = [];
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedSafes))]
    [field: Bindable(true)]
    public partial ObservableCollection<ComboModel> Safes { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ComboModel> SelectedSafes { get; set; } = [];
    
    public StaticComboVm<StorageType> StorageTypes { get; } = new();
    
    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    public UserPageEntryViewModel(
        DtoUser? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        
        AddValidation(() => Model.RoleGroupId, optional: false);

        Startup();
        Refresh();
    }

    private async void Startup()
    {
        Storages.Clear();
        var storages = await GetListInnerByQueryAsync<ComboModel>("Storages", new());
        foreach (var dto in storages.Data)
        {
            Storages.Add(dto);
        }
        SelectedStorages.Clear();
        var selectedStorages = Storages.Where(s => Model.VisibleStorages.Any(m 
            => m == s.Id.ToString())).ToList();

        foreach (var storage in selectedStorages)
        {
            SelectedStorages.Add(storage);
        }
        
        Safes.Clear();
        var safes = await GetListInnerByQueryAsync<ComboModel>("Safes", new());
        foreach (var dto in safes.Data)
        {
            Safes.Add(dto);
        }
        SelectedSafes.Clear();
        var selectedSafes = Safes.Where(s => Model.VisibleSafes.Any(m 
            => m == s.Id.ToString())).ToList();

        foreach (var safe in selectedSafes)
        {
            SelectedSafes.Add(safe);
        }
        
        Banks.Clear();
        var banks = await GetListInnerByQueryAsync<ComboModel>("Banks", new());
        foreach (var dto in banks.Data)
        {
            Banks.Add(dto);
        }
        SelectedBanks.Clear();
        var selectedBanks = Banks.Where(s => Model.VisibleBanks.Any(m 
            => m == s.Id.ToString())).ToList();

        foreach (var bank in selectedBanks)
        {
            SelectedBanks.Add(bank);
        }
    }

    private async void Refresh()
    {
        await Groups.LoadAsync(this, "Groups", Model.RoleGroupId, s => Model.RoleGroupId = s.Id);
        await StoragesCombo.LoadAsync(this, "Storages", Identifier.Parse(Model.DefaultStorageId),
            s => Model.DefaultStorageId = s.Id.ToString());
        await SafesCombo.LoadAsync(this, "Safes", Identifier.Parse(Model.DefaultSafeId),
            s => Model.DefaultSafeId = s.Id.ToString());
        await BanksCombo.LoadAsync(this, "Banks", Identifier.Parse(Model.DefaultBankId),
            s => Model.DefaultBankId = s.Id.ToString());
        StorageTypes.Set(Model.DefaultStorageType, s => Model.DefaultStorageType = s);
        PaymentTypes.Set(Model.DefaultPaymentType, s => Model.DefaultPaymentType = s);
    }
    
    protected override void PreSaveActions()
    {
        Model.VisibleSafes = [];

        foreach (var selectedSafes in SelectedSafes)
        {
            Model.VisibleSafes = Model.VisibleSafes.Append(selectedSafes.Id.ToString());
        }
        
        Model.VisibleStorages = [];

        foreach (var selectedStorages in SelectedStorages)
        {
            Model.VisibleStorages = Model.VisibleStorages.Append(selectedStorages.Id.ToString());
        }
        
        Model.VisibleBanks = [];

        foreach (var selectedBanks in SelectedBanks)
        {
            Model.VisibleBanks = Model.VisibleBanks.Append(selectedBanks.Id.ToString());
        }
    }

    protected override bool PreSaveValidations()
    {
        if ((!Model.VisibleStorages.Contains(Model.DefaultStorageId) && StoragesCombo.IsAnySelected()).Stop("ValidationFailed", "DefaultStorageMustBeVisible"))
            return false;
        
        if ((!Model.VisibleSafes.Contains(Model.DefaultSafeId) && SafesCombo.IsAnySelected()).Stop("ValidationFailed", "DefaultSafeMustBeVisible"))
            return false;

        if ((!Model.VisibleBanks.Contains(Model.DefaultBankId) && BanksCombo.IsAnySelected()).Stop("ValidationFailed", "DefaultBankMustBeVisible"))
            return false;
        
        if ((Model.RoleGroupId is null).Stop("ValidationFailed", "RoleGroupIdSelectOption"))
            return false;
        
        return base.PreSaveValidations();
    }

    protected override void AfterSave()
    {
        _ = Session.RefreshUserInfoAsync();
        
        base.AfterSave();
    }
}