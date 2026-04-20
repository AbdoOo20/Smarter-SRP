using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.DisbursementPermits;

public partial class DisbursementPermitEntryViewModel : BaseItemPageEntryViewModel<DtoDisbursementPermit, DtoDisbursementPermitDetail>
{
    protected override string Endpoint => "DisbursementPermit";
    
    [ObservableProperty]
    public partial SuperComboVm<DtoDisbursementPermitDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoDisbursementPermitDetail> CostCentres { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    public DisbursementPermitEntryViewModel(
        DtoDisbursementPermit? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType, false);
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;
        
        RefreshState(async () => await StartupAsync());
        InnerSaveEndpoint = EntryType is EntryType.Add ? "Create" : "Update";
    }

    private async Task StartupAsync(bool refresh = true)
    {
        SetDetailsModel(null);
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        
        await OnAsync(() => refresh, async () => await RefreshAsync());
        On(EntryType, EntryType.Add, () => Model.Date = DateTime.Now);
        
        OnPropertyChanged();
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        await CostCentres.LoadAsync(this, "costcente", Model.CostCenterId,
            s => Model.CostCenterId = s.Id);

        var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("stores")).Data.ToList());
        var defStore = GetDefaultStorage(stores, Model.StoreId, EntryType);
        Stores.Load(stores, defStore, s => Model.StoreId = s.Id);
        
        SetData(Model.DisbursementPermitDetails);
        RefreshDataGrid();
    }
}