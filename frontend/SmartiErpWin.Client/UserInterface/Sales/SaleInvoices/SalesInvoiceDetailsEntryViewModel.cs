using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.SalesModels;
using SmartiErpWin.Client.UserInterface.Sales.Lookup;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoiceDetailsEntryViewModel : BaseDetailsEntryViewModel<DtoSalesInvoiceDetailsModel>
{
    protected override string Endpoint => "Sales";

    [ObservableProperty]
    private decimal _suggestedPrice;

    [ObservableProperty]
    private StorageType _storageType;

    [ObservableProperty]
    private Visibility _fullVisibility = Visibility.Visible;

    [ObservableProperty]
    private ObservableCollection<ComboModel> _itemsSource = [];

    [ObservableProperty]
    private ComboModel? _selectedItem;

    [ObservableProperty]
    private string? _searchText;
    
    [ObservableProperty]
    private string? _selectedItemStatus;

    private SalesInvoiceDetailsEntryWindow? _self;
    
    [ObservableProperty]
    private InfoBarSeverity _selectedItemStatusSeverity = InfoBarSeverity.Informational;
    
    public SalesInvoiceDetailsEntryViewModel(
        LocalisationService locale,
        HttpClient client,
        Type pageType,
        SalesInvoiceDetailsEntryWindow? self) : base(client)
    {
        _self = self;
        PageType = pageType;

        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        SetState(null);

        AddValidation(() => Model.ItemId, optional: false)
        .AddValidation(() => Model.UnitId, optional: false)
        .AddValidation(() => Model.Discount, [new(d => Model.Price < d, "DiscountCannotBeHigherThanPrice")], optional: true);
    }
    
    protected internal sealed override async void Refresh()
    {
        //await SetItemInitialAsync();
        
        RefreshSelectedItemStatus();
        await RequestApiComboItemsFromInnerEndpointAsync("Categories", 3);
        ComboModelBase.SetComboModelOrDefault(null, 3);

        if (Model is not null && Model.ExpireDate is not null)
        {
            SetExpireDate();
            ComboModelBase.SetComboModel(x => x.Value == Model.ExpireDateString, 2);
        }
        
        if(Model.ItemId != default)
        {
            var combos = await RequestComboModelsAsync<ComboModel>("Items");
            SelectedItem = combos.Data.FirstOrDefault(x => x.Value == Model.ItemId.ToString());
            SetUnits();
            SetPrice();
        }
    }

    partial void OnSelectedItemChanged(ComboModel? value)
    {
        RefreshSelectedItemStatus();
    }
    
    /*public async Task SetItemInitialAsync()
    {
        await RequestApiComboItemsFromInnerEndpointAsync("Items");
        ComboModelBase.SetComboModelOrDefault(Model?.ItemId);

        SearchText = ComboModelBase.GetSelectedItemDisplay();
        
        OnItemSuggestionChosen(ComboModelBase.Api1);
    }*/

    private void RefreshSelectedItemStatus()
    {
        if (SelectedItem is null)
        {
            SelectedItemStatusSeverity = InfoBarSeverity.Informational;
            SelectedItemStatus = this["PleaseSelectAnItem"];
        }
        else
        {
            SelectedItemStatusSeverity = InfoBarSeverity.Success;
            SelectedItemStatus = string.Format(this["TheFollowingItemIsSelected"],
                SelectedItem.Display ?? this["ItemWithNoName"]);
        }
    }
    
    /*public void ClearItem() 
    {
        SelectedItem = null;
        WorkingModel.ItemId = default;
    }*/
    
    /*public void OnItemSuggestionChosen(ComboModel selectedItem)
    {
        SelectedItem = selectedItem;
        
        SetUnits();
        SetExpireDate();
    }*/
    
    /*public async void SetItemsByCategory()
    {
        if (stopChanging) return;

        var selectedItem = ComboModelBase.GetSelectedItemId(3);

        if (selectedItem.IsNullOrEmpty())
            return;

        await RequestApiComboItemsFromInnerEndpointAsync("Items", 0, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.ItemId);
        
        ComboModelBase.GetApiCollectionByIndex(0).Observe(ItemsSource);
    }*/

    public bool IsSelectionWindowOpen { get; set; }
    
    [RelayCommand]
    private void OnSelectItem(string? previousId = null)
    {
        if (IsSelectionWindowOpen)
            return;
        
        previousId ??= SelectedItem?.Value;
        
        var window = new ItemSelectionWindow(new ItemSelectionWindowViewModel
            (null, LocaleService, HttpClient, null, Endpoint, previousId));

        IsSelectionWindowOpen = true;
        
        var result = window.ShowDialog() ?? false;

        IsSelectionWindowOpen = false;
        
        if (!result)
            return;
        
        SelectedItem = window.ViewModel.SelectedItem;
        
        if (string.IsNullOrEmpty(SelectedItem?.Value))
        {
            WorkingModel.ItemId = default;
            return;
        }
        
        if (Guid.TryParse(SelectedItem.Value, out var id))
        {
            WorkingModel.ItemId = id;
        }
        
        SetUnits();
        SetPrice();

        _self?.FocusFirst();
    }
    
    public async void SetUnits()
    {
        if (stopChanging) return;

        if (SelectedItem?.Value is null)
            return;

        await RequestApiComboItemsFromInnerEndpointAsync("Units", 1, SelectedItem.Value);
        ComboModelBase.SetComboModelOrDefault(Model?.UnitId, 1);

        ComboModelBase.SelectAtOnePoint(1);
    }
    
    private bool _isSettingPrice;

    public async void SetPrice()
    {
        if (stopChanging) return;

        /*if (Model?.Price != 0)
            return;*/

        if (WorkingModel is null)
            return;

        if (FullVisibility is Visibility.Collapsed)
            return;

        if (_isSettingPrice)
            return;

        try
        {
            _isSettingPrice = true;
        
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
        finally
        {
            _isSettingPrice = false;
        }
    }

    public async void SetExpireDate()
    {
        if (stopChanging) return;

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
            StorageType = StorageType,
        });

        ComboModelBase.SelectAtOnePoint(2);
    }

    protected override void PreSaveActions()
    {
        var model = GetWorkingModel();

        model.UnitId = ComboModelBase.GetSelectedItemId(1);
        model.UnitName = ComboModelBase.SelectedApiComboItems[1]?.Display;
        
        if (SelectedItem is null)
        {
            // Message will be handled by PreSaveValidations
            return;
        }
        
        model.ItemId = Guid.Parse(SelectedItem.Value);
        model.ItemName = SelectedItem.Display;
        
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
        var cc = ComboModelBase.GetApiCollectionByIndex(2).Count;
        if (cc > 1 & model.ExpireDate is null)
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], "ExpireDateCannotBeEmpty", localeService: LocaleService);
            return false;
        }
        return base.PreSaveValidations();
    }
    
    bool stopChanging = false;
    
    private bool _firstEverChange = true;
    
    protected override Task SaveAsync()
    {
        if (IsSelectionWindowOpen)
            return Task.CompletedTask;
        
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
        
        stopChanging = true;

        SynchroniseTypeBase(PageType, model, IsNew ? CodeBook.IsNew : "");

        var copy = SelectedItem?.Value;
        
        SearchText = string.Empty;
        SelectedItem = null;
        SuggestedPrice = 0;
        Model.TemporaryStoreId = x;
        ComboModelBase.SelectAtOnePoint(0);
        ComboModelBase.SetApiComboItems([], 2);
        _actionClear?.Invoke();
        stopChanging = false;

        if (IsNew || _firstEverChange)
        {
            OnSelectItem(copy);
        }

        SetState(null);
        _firstEverChange = false;
        
        return Task.CompletedTask;
    }

    private Action? _actionClear;
    public void OnClear(Action actionClear)
    {
        _actionClear = actionClear;
    }

    private void LimitVisibility()
    {
        FullVisibility = Visibility.Collapsed;
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.Length >= 2 && args[1] is StorageType storageType)
        {
            StorageType = storageType;
        }

        switch (args.FirstOrDefault())
        {
            case DtoSalesInvoiceDetailsModel model:
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
            case CodeBook.LimVis:
                LimitVisibility();
                break;
        }

        return Task.CompletedTask;
    }
}