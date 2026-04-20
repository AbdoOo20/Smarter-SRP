using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemsTransferInvoice;

public partial class ItemsTransferInvoiceEntryViewModel : BaseItemPageEntryViewModel<DtoItemsTransferInvoice, DtoItemsTransferInvoiceDetail>
{
    protected override string Endpoint => "ItemsTransferInvoice";
    
    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }

    public StaticComboVm<ItemsTransferRequestStatus> TransferStatus { get; } = new();

    [ObservableProperty]
    public partial SuperComboVm<DtoItemsTransferInvoiceDetail> FromStores { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoItemsTransferInvoiceDetail> ToStores { get; set; } = new();

    public ItemsTransferInvoiceEntryViewModel(
        DtoItemsTransferInvoice? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType, false);
        
        /*if (EntryType == EntryType.Add)
        {
            Model.Date = DateTime.Now;
            Model.EndDate = DateTime.Now.AddDays(1);
        }*/
        
        Model.Details ??= [];
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;

        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        await OnAsync(EntryType, EntryType.Add,
            async () =>
            {
                InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data;
                Model.InvoiceNumber = InvoiceNumber;
            });
        
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.InvoiceNumber);
        SetStaticComboBoxes();
        SetDetailsModel(null);
        
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Quantity, optional: false));
        
        await LoadDataAsync();
        await OnAsync(() => refresh, async () => await RefreshAsync());
    }

    private void SetStaticComboBoxes()
    {
        TransferStatus.Set(Model.TransferStatus, s =>
        {
            Model.TransferStatus = s;
        });
    }

    private async Task LoadDataAsync()
    {

    }
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.Details);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.FromStorageId, EntryType);
            FromStores.Load(stores, defStore, s =>
            {
                Model.FromStorageId = s.Id;
                Model.FromStorageName = s.Display;
            });

            ToStores.Load(stores, Model.ToStorageId, s =>
            {
                Model.ToStorageId = s.Id;
                Model.ToStorageName = s.Display;
            });
        }
    }

    #region Model
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokeItemsTransferInvoice(new(Model)));
    }

    #endregion
}