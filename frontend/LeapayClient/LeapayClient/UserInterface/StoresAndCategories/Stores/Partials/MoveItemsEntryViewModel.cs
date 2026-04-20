using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core.Enums;
using SukiUI.Toasts;

namespace Leapay.Client.UserInterface.StoresAndCategories.Stores.Partials;

public partial class MoveItemsEntryViewModel : BaseItemPageEntryViewModel<MoveItemsRequest, StorageItemsRequest>
{
    protected override string Endpoint => "Store";

    [ObservableProperty]
    public partial SuperComboVm<StorageItemsRequest> Categories { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<StorageItemsRequest> SecondCategories { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }
    
    private readonly ISukiToastManager _toastManager;

    public MoveItemsEntryViewModel(
        MoveItemsRequest? model,
        EntryType entryType,
        Type pageType)
    {
        _toastManager = App.GetService<ISukiToastManager>();
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
        
        await OnAsync(() => refresh, async () => await RefreshAsync());
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.Details);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            await Categories.LoadAsync(this, "Stores", Model.SourceLocationId,
                s => Model.SourceLocationId = s.Id);
            
            await SecondCategories.LoadAsync(this, "Stores", Model.DestinationLocationId,
                s => Model.DestinationLocationId = s.Id);
        }
    }
    
    protected override async Task SaveAsync(Action? successCallback = null)
    {
        if (Model is null)
            return;
        PreSaveActions();
        
        var req = await PostSaveSpecificAsync(Model, "MoveItems");

        if (req.Status)
        {
            ContentHelper.CloseWindow<MoveItemsEntryWindow>();
            
            _toastManager.CreateSimpleInfoToast()
                .WithTitle(this["Success"])
                .WithContent(this["ItemsMoved"])
                .Queue();
        }
        else
        {
            await DialogueServices.ShowMessageAsync("Error", req.ErrorMessage);
        }
    }
}