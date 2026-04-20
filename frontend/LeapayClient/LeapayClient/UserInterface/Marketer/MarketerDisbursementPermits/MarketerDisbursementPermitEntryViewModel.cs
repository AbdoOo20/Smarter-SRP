using Fly.Shared.Modules.Marketers.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Marketer.MarketerDisbursementPermits;

public partial class MarketerDisbursementPermitEntryViewModel : BaseItemPageEntryViewModel<DtoMarketerDisbursementPermit, DtoMarketerDisbursementPermitDetail>
{
    protected override string Endpoint => "MarketerDisbursementPermit";

    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }

    [ObservableProperty]
    public partial SuperComboVm<DtoMarketerDisbursementPermitDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoMarketerDisbursementPermitDetail> Marketers { get; set; } = new("Marketer");
    
    public MarketerDisbursementPermitEntryViewModel(
        DtoMarketerDisbursementPermit? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType, false);
        
        if (EntryType == EntryType.Add)
        {
            Model.Date = DateTime.Now;
        }
        
        Model.Details ??= [];
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;
        
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        await OnAsync(EntryType, EntryType.Add, async () => InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data);
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.Number);
        SetDetailsModel(null);
        
        await OnAsync(() => refresh, async () => await RefreshAsync());
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.Details);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.StoreId, EntryType);
            Stores.Load(stores, defStore, s => Model.StoreId = s.Id);
            
            await Marketers.LoadAsync(this, "marketers", Model.MarketerId,
                s => Model.MarketerId = s.Id);
        }
    }
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokeMarketerDisbursement(new(Model)));
    }
}