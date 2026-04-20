using Avalonia.Threading;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Items;

public partial class ItemEntryViewModel : BasePageEntryViewModel<CreateUpdateItemRequest, CreateUpdateItemUnitRequest>
{
    protected override string Endpoint => "Item";

    [ObservableProperty]
    public partial SuperComboVm<CreateUpdateItemUnitRequest> Categories { get; set; } = new();
    
    [ObservableProperty]
    public partial BasePageEntryViewModel<CreateUpdateItemRequest, CreateUpdateItemBarcodeRequest> Barcodes { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();
    private IValidationEnforcer BarcodesEnforcer { get; set; } = new ValidationEnforceImpl();

    public ItemEntryViewModel(
        CreateUpdateItemRequest? model,
        EntryType entryType,
        Type pageType,
        bool silent = false)
    {
        SetDetails(model, entryType, pageType, false, silent);
        
        Model.ItemUnits ??= [];
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;
        
        Barcodes.OnItemEdit += OnBarcodeEdit;
        Barcodes.OnItemDelete += OnBarcodeDelete;
        
        LoadImageAsync(Model.Id);
        
        RefreshState(async () => await StartupAsync());
    }

    private async void LoadImageAsync(Identifier id)
    {
        if (IsSilent && EntryType is EntryType.Add)
        {
            Model.Image = ContentHelper.GetDefaultProductImage();
            return;
        }
        
        try
        {
            var image = await GetInnerByQueryAsync<ItemQueryResponse>("ImageQuery",
                new ItemQueryRequest
                {
                    ItemId = id
                });

            var data = image.Data.ItemImage;
            Dispatcher.UIThread.Post(() => Model.Image = data);
        }
        catch (Exception e)
        {
            Error(e.ToString());
        }
    }

    private async Task StartupAsync(bool refresh = true)
    {
        SetDetailsModel(null);
        SetBarcodesModel(null);
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        BarcodesEnforcer.AddValidation(CreateValidation(() => ModelBarcodes.Model.Barcode, optional: false));

        await OnAsync(() => refresh && !IsSilent, async () => await RefreshAsync());
        On(() => EntryType == EntryType.Add && !IsSilent, () => Model.Expirable =
            PreferencesService.Get(s => s.SysConfigModel.ItemsHaveExpireDate));
    }
    
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.ItemUnits);
        Barcodes.SetData(Model.ItemBarcodes);
        RefreshDataGrid();
        Barcodes.PublicRefreshDataGrid();
        
        if (refreshCombos && !IsSilent)
        {
            await Categories.LoadAsync(this, "gategories", Model.CategoryId,
                s => Model.CategoryId = s.Id);
        }
    }

    [RelayCommand]
    private async Task OnSelectFileAsync()
    {
        var data = await DataHelper.PickFileAndReadBytesAsync(App.GetOpenWindowOfType<ItemEntryWindow>());
        Model.Image = data;
    }

    protected override bool PreSaveChecks()
    {
        if(Model.MinPrice > 0 && (Model.Price < Model.MinPrice || Model.RegionalPrice < Model.MinPrice))
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], 
                "ItemPriceCannotBeLowerThanMinimumPrice");
            return false;
        }
        
        if(!Model.ItemUnits.Any(s => s.IsPrimary))
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], 
                "AtLeastOnePrimaryItemNeeded");
            return false;
        }
            
        return base.PreSaveChecks();
    }
}