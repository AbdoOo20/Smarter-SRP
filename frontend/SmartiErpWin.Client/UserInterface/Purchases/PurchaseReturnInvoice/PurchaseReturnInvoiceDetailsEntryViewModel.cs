using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels.SalesModels;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoiceDetailsEntryViewModel : BaseDetailsEntryViewModel<DtoPurchaseReturnlInvoiceDetailModel>
{
    protected override string Endpoint => "PurchaseReturnInvoice";

    [ObservableProperty]
    private decimal _suggestedPrice;

    [ObservableProperty]
    private Visibility _fullVisibility = Visibility.Visible;

    public PurchaseReturnInvoiceDetailsEntryViewModel(
        LocalisationService locale,
        HttpClient client,
        Type pageType) : base(client)
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

        if (Model is not null && Model.ExpireDate is not null)
        {
            SetExpireDate();
            ComboModelBase.SetComboModel(x => x.Value == Model.ExpireDateString, 2);
        }

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
        SetPrice();
    }

    public async void SetPrice()
    {
        if (Model?.Price != 0)
            return;

        if (WorkingModel is null)
            return;

        if (FullVisibility is Visibility.Collapsed)
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

        if (storeId?.IsNullOrEmpty() ?? true || itemId.IsNullOrEmpty())
            return;

        ComboModelBase.Clear(2, true);
        await RequestCombosUsingQueryRequestAsync("ExpireDate", 2, new ExpireDateQueryRequest
        {
            ItemId = itemId,
            StoreId = storeIdValue,
            LinkedFile = SellInvoice,
            StorageType = StorageType
        });
        ComboModelBase.SelectAtOnePoint(2);
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
        model.ExpireDate = null;
        if (ComboModelBase.SelectedApiComboItems[2] != null)
        {
            if (DateTime.TryParse(ComboModelBase.SelectedApiComboItems[2]!.Value, out var date))
            {
                model.ExpireDate = date;
            }
        }
    }

    protected override bool PreSaveValidations()
    {
        var model = GetWorkingModel()!;
        if (ComboModelBase.GetApiCollectionByIndex(2).Any() & model.ExpireDate is null)
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], "ExpireDateCannotBeEmpty", localeService: LocaleService);
            return false;
        }
        return base.PreSaveValidations();
    }

    protected override Task SaveAsync()
    {
        var model = GetWorkingModel();

        if (model is null)
            return Task.CompletedTask;

        var x = Model.TemporaryStoreId;

        if (model.Quantity == 0)
            model.Quantity = 1;

        model.Price = SuggestedPrice;

        PreSaveActions();

        if (!PreSaveValidations())
            return Task.CompletedTask;

        SynchroniseTypeBase(PageType, model);


        SuggestedPrice = 0;
        Model.TemporaryStoreId = x;
        ComboModelBase.SelectAtOnePoint(0);
        ComboModelBase.SetApiComboItems([], 2);
        _actionClear?.Invoke();

        SetState(null);

        return Task.CompletedTask;
    }

    private Action? _actionClear;
    public void OnClear(Action actionClear)
    {
        _actionClear = actionClear;
    }
    private void LimitVisibility()
    {
        //FullVisibility = Visibility.Collapsed;
    }

    [ObservableProperty]
    private StorageType _storageType;
    [ObservableProperty]
    private Identifier? _sellInvoice;

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {

        if (args.Length >= 2 && args[1] is StorageType storageType)
        {
            StorageType = storageType;
        }
        if (args.Length >= 3 && args[2] is Identifier sellInvoiceId)
        {
            SellInvoice = sellInvoiceId;
        }

        switch (args.FirstOrDefault())
        {
            case DtoPurchaseReturnlInvoiceDetailModel model:
                SetState(model);
                SuggestedPrice = model.Price;
                //Model.ExpireDate = model.ExpireDate;
                break;
            case Identifier id:
                if (WorkingModel is null)
                    break;

                WorkingModel.TemporaryStoreId = id;
                SetPrice();
                SetExpireDate();
                break;
            case CodeBook.LimVis:
                LimitVisibility();
                break;
        }

        return Task.CompletedTask;
    }
}