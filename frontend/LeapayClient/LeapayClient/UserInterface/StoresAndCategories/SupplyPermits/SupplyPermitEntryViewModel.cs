using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.SupplyPermits;

public partial class SupplyPermitEntryViewModel : BaseItemPageEntryViewModel<DtoSupplyPermit, DtoSupplyPermitDetail>
{
    protected override string Endpoint => "SupplyPermit";
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSupplyPermitDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoSupplyPermitDetail> Suppliers { get; set; }
        = new("Supplier", SplitSearchableComboEntryWidth);
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    public SupplyPermitEntryViewModel(
        DtoSupplyPermit? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType, false);
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;
        
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        SetDetailsModel(null);
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        
        On(EntryType, EntryType.Add, () => Model.Date = DateTime.Now);
        await OnAsync(() => refresh, async () => await RefreshAsync());

        OnPropertyChanged();
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        await Suppliers.LoadAsync(this, "suppliers", Model.SupplierId,
            s => Model.SupplierId = s.Id);
        
        var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("stores")).Data.ToList());
        var defStore = GetDefaultStorage(stores, Model.StoreId, EntryType);
        Stores.Load(stores, defStore, s => Model.StoreId = s.Id);
        
        SetData(Model.SupplyPermitDetails);
        RefreshDataGrid();
    }
}