using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;
using SmartiErpWin.Client.DtoModels.QuantityCreditorModels;

namespace SmartiErpWin.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingDetailsEntryViewModel : BaseDetailsEntryViewModel<DtoQuantityCreditingDetailsModel>
{
    protected override string Endpoint => "QuantityCrediting";

    [ObservableProperty]
    private decimal _suggestedPrice;
    
    public QuantityCreditingDetailsEntryViewModel(
        LocalisationService locale,
        HttpClient client,
        Type pageType): base(client)
    {
        PageType = pageType;
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        SetState(null);

        AddValidation(() => Model.ItemId, optional: false)
            .AddValidation(() => Model.UnitId, optional: false);
    }

    protected internal sealed override async void Refresh()
    {
        await RequestApiComboItemsFromInnerEndpointAsync("Items");
        ComboModelBase.SetComboModelOrDefault(Model?.ItemId);
        
        await RequestApiComboItemsFromInnerEndpointAsync("Categories", 3);
        ComboModelBase.SetComboModelOrDefault(null, 3);
    }

    public async void SetItems()
    {
        var selectedItem = ComboModelBase.GetSelectedItemId(3);

        if (selectedItem == Identifier.Empty)
            return;
        
        await RequestApiComboItemsFromInnerEndpointAsync("Items", 0, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.ItemId);
    }
    
    public async void SetUnits()
    {
        var selectedItem = ComboModelBase.GetSelectedItemId();

        if (selectedItem == Identifier.Empty)
            return;
        
        await RequestApiComboItemsFromInnerEndpointAsync("Units", 1, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.UnitId, 1);
        
        ComboModelBase.SelectAtOnePoint(1);
        
        WorkingModel.ItemId = selectedItem;
    }

    public async void SetPrice()
    {
        if (Model?.Price != 0)
            return;

        if (WorkingModel is null)
            return;
        
        WorkingModel.UnitId = ComboModelBase.GetSelectedItemId(1);
        
        var storeId = WorkingModel.TemporaryStoreId;
        var storeIdValue = storeId.GetValueOrDefault();
        
        if (!WorkingModel.CanSendQueryRequest())
            return;
        
        var itemInfoQuery = new GetItemInfoQueryRequest
        {
            StoreId = storeIdValue,
            UnitId = WorkingModel.UnitId,
            ItemId = WorkingModel.ItemId
        };

        var req = await GetInnerByQueryAsync<ItemInfoQueryResponse>("ItemInfo", itemInfoQuery);

        if (req.Status)
        {
            SuggestedPrice = req.Data!.Price;
        }
    }

    public async void SetExpireDate()
    {
        if (WorkingModel is null)
            return;
        
        var storeId = WorkingModel.TemporaryStoreId;
        var storeIdValue = storeId.GetValueOrDefault();
        var itemId = ComboModelBase.GetSelectedItemId();

        if (storeId == Identifier.Empty || itemId == Identifier.Empty)
            return;
        
        await RequestCombosUsingQueryRequestAsync("ExpireDate", 2, new ExpireDateQueryRequest
        {
            ItemId = itemId,
            StoreId = storeIdValue
        });
        
        ComboModelBase.SetComboModelOrDefault(null, 2);
    }
    
    protected override void PreSaveActions()
    {
        var model = GetWorkingModel();

        model.UnitId = ComboModelBase.GetSelectedItemId(1);
        model.UnitName = ComboModelBase.SelectedApiComboItems[1]?.Display;
        model.ItemId = ComboModelBase.GetSelectedItemId();
        model.ItemName = ComboModelBase.SelectedApiComboItems[0]?.Display;
        if (model.ItemName.Contains("•"))
        {
            var nnn = model.ItemName.Split("•");
            model.ItemBarcode = nnn[1];
            model.ItemName = nnn[0];
        }
        //model.ExpireDate = ComboModelBase.GetSelectedItemDisplay(2);
    }

    protected override Task SaveAsync()
    {
        var model = GetWorkingModel();

        if (model is null)
            return Task.CompletedTask;

        if (model.Quantity == 0)
            model.Quantity = 1;
        
        model.Price = SuggestedPrice;
        
        PreSaveActions();
        
        if (!PreSaveValidations())
            return Task.CompletedTask;
        
        SynchroniseTypeBase(PageType, model);
        
        SetState(null);
        SuggestedPrice = 0;

        ComboModelBase.SetApiComboItems([], 2);
        ComboModelBase.SelectedApiComboItems[0] = ComboModelBase.GetDefaultComboModel();
        _actionClear?.Invoke();

        return Task.CompletedTask;
    }

    private Action? _actionClear;
    public void OnClear(Action actionClear)
    {
        _actionClear = actionClear;
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.FirstOrDefault())
        {
            case DtoQuantityCreditingDetailsModel model:
                SetState(model);
                SuggestedPrice = model.Price;
                break;
            case Identifier id:
                if (WorkingModel is null)
                    break;
                
                WorkingModel.TemporaryStoreId = id;
                SetPrice();
                SetExpireDate();
                break;
        }

        return Task.CompletedTask;
    }
}
