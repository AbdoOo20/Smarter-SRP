using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.ClientModels.Dto.DtoModels.FastSales;
using Leapay.Client.ClientModels.Dto.DtoModels.SalesModels;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using Leapay.Client.UserInterface.Sales.Clients;
using Leapay.Client.UserInterface.Sales.SalesInvoice;
using Leapay.Client.UserInterface.Sales.SalesPoint.SuspendedInvoices;
using Leapay.Client.UserInterface.Sales.SalesReturnInvoice;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;
using SukiUI.Toasts;
using DiscountType = Fly.Shared.Modules.PurchaseAndSales.Requests.DiscountType;

namespace Leapay.Client.UserInterface.Sales.SalesPoint;

public partial class SalesPointPageViewModel(ISukiToastManager toastManager) : BaseRefreshablePageViewModel<FastSaleInvoiceWrapper>
{
    public ISukiToastManager ToastManager { get; } = toastManager;
    
    protected override void OnEdit(object? sender, DataGridButtonEventArgs<FastSaleInvoiceWrapper> args)
    {
        //throw new NotImplementedException();
    }

    private static string BaseEndpoint => "Sales";
    
    protected override string Endpoint => "";

    [ObservableProperty]
    public partial FastSaleInvoiceWrapper Data { get; set; } = new();

    [ObservableProperty]
    public partial string? CurrentBarcode { get; set; }

    [ObservableProperty]
    public partial decimal? CurrentDiscountValue { get; set; } = 0m;
    
    [ObservableProperty]
    public partial decimal? CurrentTaxValue { get; set; } = 0m;
    
    private TextBox? BarcodeBox { get; set; }
    
    private IEnumerable<DtoCategory> Categories { get; set; } = [];

    private IEnumerable<DtoItem> Items { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<ItemModel> FavouritedItems { get; set; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BrowseCategoriesText))]
    public partial bool CategoriesExpanderOpen { get; set; }

    public string BrowseCategoriesText => CategoriesExpanderOpen ? this["CloseBrowseCategories"] : this["OpenBrowseCategories"];
    
    [ObservableProperty]
    public partial ObservableCollection<ItemModel> NoBarcodeItems { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<CategoryModel> TabCategories { get; set; } = [];

    [ObservableProperty]
    public partial CategoryModel? SelectedTabCategory { get; set; }
    
    private StackPanel? _itemsSearchPanel;
    
    [ObservableProperty]
    public partial bool BankVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool SafeVisibility { get; set; }
    
    public StaticComboVm<DiscountType> DiscountType { get; } = new();
    
    public StaticComboVm<TaxType> TaxType { get; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<FastSaleInvoiceWrapper> Safes { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<FastSaleInvoiceWrapper> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<FastSaleInvoiceWrapper> Banks { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<FastSaleInvoiceWrapper>? Clients { get; set; }
    
    public StaticComboVm<PointOfSalePaymentType> PaymentTypes { get; } = new();

    private bool _finishInit;
    
    public async Task InitAsync(StackPanel? itemsSearchPanel)
    {
        _itemsSearchPanel = itemsSearchPanel;

        Clients = new(ContentHelper.GetIconTitle(this["Client"], MaterialIconKind.PersonAdd, BackgroundTheme), SplitSearchableComboEntryWidth - 60);
        await FetchAndSetAsync();

        SetExpanderProperties();
    }

    private void SetExpanderProperties()
    {
        var inSettings = PreferencesService.Get(s => s.SalesPointExpanderOpen);

        CategoriesExpanderOpen = inSettings;
        PropertyChanged += (_, a) =>
        {
            if(a.PropertyName is nameof(CategoriesExpanderOpen))
            {
                PreferencesService.Set(s => s.SalesPointExpanderOpen = CategoriesExpanderOpen);
            }
        };
    }

    private SuggestBoxCreationInfo<ItemSearchModel>? _currentSuggestionBox;
    
    private async Task FetchAndSetAsync()
    {
        RefreshState(async () =>
        {
            await FetchNewDataAsync();
            await RefreshCallsAsync();
            AddCategories();
            _currentSuggestionBox = UiHelpers.CreateItemsSearchBox(_itemsSearchPanel, mainBrush: BackgroundTheme, models: Items.Select(t => new ItemSearchModel
            {
                ItemPrice = t.Price.ToString("N"),
                Barcode = t.Barcode,
                OtherBarcodes = t.ItemBarcodes.Select(c => c.Barcode).ToList(),
                ItemUnitBarcodes = t.ItemUnits.Select(c => new Tuple<Identifier, string>(c.UnitId, c.Barcode)).ToList(),
                PhotoLink = ContentHelper.GetPhotoLink(t.Id, t.UpdateCounter),
                ItemId = t.Id,
                ItemName = t?.Name
            }), width: 330);

            _currentSuggestionBox.OnSuggestionChosen += (_, item) =>
            {
                Data.AddOrUpdate(Items.FirstOrDefault(i => i.Id == item.ItemId), selectedUnitId: item.UnitId);
            };
        });
    }

    private void HandleMultiItems(List<ItemsSelectionModel> models)
    {
        foreach (var model in models)
        {
            var modelInCache = Items.FirstOrDefault(i => i.Id == model.ItemId);
            modelInCache.Price = model.ItemCustomPrice;
            
            Data.AddOrUpdate(modelInCache, model.Quantity, model.SelectedUnit.UnitId);
        }
    }
    
    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task OnSelectItemsAsync()
    {
        var window = await ContentHelper.OpenItemsSelectionWindowAsync(Data.StoreId);
        
        if (!window.result)
        {
            return;
        }
        
        HandleMultiItems(window.items);
    }
    
    private async Task ResetAndRefreshAsync()
    {
        CurrentDiscountValue = 0;
        CurrentTaxValue = 0;
        CurrentBarcode = "";
        Data = null;
        Data = new();

        await FetchAndSetAsync();
    }

    private async Task RefreshCallsAsync()
    {
        DiscountType.Set(Fly.Shared.Modules.PurchaseAndSales.Requests.DiscountType.Value, OnDiscountTypeChanged);
        TaxType.Set(Fly.Shared.Modules.PurchaseAndSales.Enums.TaxType.Percent, OnTaxTypeChanged);
        PaymentTypes.Set(Data.PaymentType, type =>
        {
            Data.PaymentType = type;
            CheckVisibilities();
        });
        
        var safes = GetVisibleSafes((await RequestComboModelsAsync<ComboModel>($"{BaseEndpoint}/Safes")).Data.ToList());
        var defSafe = GetDefaultSafe(safes, Data.SafeId, EntryType.Add);
        Safes.Load(safes, defSafe, s => Data.SafeId = s.Id);
        
        var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>($"{BaseEndpoint}/Stores")).Data.ToList());
        var defStore = GetDefaultStorage(stores, Data.StoreId, EntryType.Add);
        Stores.Load(stores, defStore, s =>
        {
            Data.StoreId = s.Id;
        });
        
        var banks = GetVisibleBanks((await RequestComboModelsAsync<ComboModel>($"{BaseEndpoint}/Banks")).Data.ToList());
        var defBank = GetDefaultBank(banks, Data.BankId, EntryType.Add);
        Banks.Load(banks, defBank, s =>
        {
            Data.BankId = s.Id;
        });
        
        var loader = await Clients.LoadAsync(this, $"{BaseEndpoint}/Clients", Data.ClientId,
            c => Data.ClientId = c.Id,
            adornerRightClick: async () => (await new ClientsPageViewModel().OnAddReturnAsync()).ToString(),
            adornerRightClickEndpointJudge: "Client.Add",
            adornerRightSelectedClickEndpointJudge: "Client.Edit",
            adornerRightSelectedClick: async a => (await new ClientsPageViewModel().OnEditReturnAsync(a)).ToString(),
            notFoundCallback: AutoSuggestNotFoundCallbackModel.CreateClientNotFoundCallback(),
            actionAdorner: button => button.IsVisible = false);
        loader.Box.CornerRadius = new CornerRadius(0, 8, 8, 0);
    }

    private async Task FetchNewDataAsync()
    {
        Categories = (await GetInnerAllSpecificAsync<DtoCategory>("Category", null)).Data;
        Items = (await GetInnerAllSpecificAsync<DtoItem>("Item", null)).Data;

        RefreshFavouritedItems();
        var favourites = Config.Get.FavouritedItems;
        
        Items.Where(i => string.IsNullOrEmpty(i.Barcode))
            .Select(i => GetItemModel(favourites, i))
            .Observe(NoBarcodeItems);
    }

    private ItemModel GetItemModel(List<string> favourites, DtoItem i)
    {
        return new ItemModel
        {
            IsFavourited = favourites.Contains(i.Id.ToString()),
            Name = i.Name,
            Price = i.Price,
            Photo = ContentHelper.GetPhotoLink(i.Id, i.UpdateCounter),
            Id = i.Id
        };
    }

    private void RefreshFavouritedItems()
    {
        FavouritedItems.Clear();
        var favList = Config.Get.FavouritedItems;

        foreach (var item in Items)
        {
            if(favList.Contains(item.Id.ToString()))
                FavouritedItems.Add(GetItemModel(favList, item));
        }
    }
    
    private void OnDiscountTypeChanged(DiscountType discountType)
    {
        switch (discountType)
        {
            case Fly.Shared.Modules.PurchaseAndSales.Requests.DiscountType.Value:
                Data.ApplyDiscountAsValue(CurrentDiscountValue.GetValueOrDefault());
                break;
            case Fly.Shared.Modules.PurchaseAndSales.Requests.DiscountType.Percentage:
                Data.ApplyDiscountAsPercentage((double)CurrentDiscountValue.GetValueOrDefault());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTaxTypeChanged(TaxType taxType)
    {
        switch (taxType)
        {
            case Fly.Shared.Modules.PurchaseAndSales.Enums.TaxType.Value:
                Data.ApplyTaxAsValue(CurrentTaxValue.GetValueOrDefault());
                break;
            case Fly.Shared.Modules.PurchaseAndSales.Enums.TaxType.Percent:
                Data.ApplyTaxAsPercentage((double)CurrentTaxValue.GetValueOrDefault());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    partial void OnCurrentDiscountValueChanged(decimal? value)
    {
        OnDiscountTypeChanged(DiscountType.GetSelectedItem());
    }
    
    partial void OnCurrentTaxValueChanged(decimal? value)
    {
        OnTaxTypeChanged(TaxType.GetSelectedItem());
    }
    
    /*private async Task MatchBarcodeAsync(string? barcode)
    {
        var item = Items.FirstOrDefault(i => i.Barcode == barcode);

        if (item is null)
        {
            await DialogueServices.ShowMessageAsync("NoBarcodeItemFoundTitle",
                "NoBarcodeItemFoundMessage");

            return;
        }
        
        BarcodeBox?.Clear();
        
        Data.AddOrUpdate(item);
        //SoundHelper.Play(Sound.BarcodeScannerBeep);
    }*/

    private void AddCategories()
    {
        var favourites = Config.Get.FavouritedItems;

        var currentSelected = SelectedTabCategory?.Id;
        
        TabCategories.Clear();
        
        foreach (var category in Categories)
        {
            var itemsInCategory = Items.Where(i => i.CategoryId == category.Id);
            var cat = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
            };

            foreach (var item in itemsInCategory ?? [])
            {
                cat.Items.Add(GetItemModel(favourites, item));
            }
            
            TabCategories.Add(cat);
        }

        if (currentSelected is not null && TabCategories.Any(t => t.Id == currentSelected))
        {
            SelectedTabCategory = TabCategories.FirstOrDefault(t => t.Id == currentSelected);
        }
    }
    
    [RelayCommand]
    private void OnUpdateItem()
    {
        Data.CalculatePrices();
    }

    [RelayCommand]
    private void OnIncreaseQuantity(ItemUnitSale item)
    {
        Data.AddOrUpdate(Items.FirstOrDefault(i => i.Id == item.ItemId), item.Quantity + 1, item.UnitId);
    }

    [RelayCommand]
    private void OnDecreaseQuantity(ItemUnitSale item)
    {
        Data.AddOrUpdate(Items.FirstOrDefault(i => i.Id == item.ItemId), item.Quantity - 1, item.UnitId);
    }
    
    [RelayCommand]
    private async Task OnDeleteItem(ItemUnitSale item)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Data.Delete(item);
    }
    
    [RelayCommand]
    private void OnItemSelected(ItemModel item)
    {
        Data.AddOrUpdate(Items.FirstOrDefault(i => i.Id == item.Id));
    }
    
    [RelayCommand]
    private void OnItemFavourite(ItemModel item)
    {
        var config = Config.Get;
        if (!config.FavouritedItems.Contains(item.Id.ToString()))
        {
            config.FavouritedItems.Add(item.Id.ToString());
            item.IsFavourited = true;
        }
        else
        {
            item.IsFavourited = !config.FavouritedItems.Remove(item.Id.ToString());
        }
        
        Config.SaveSettings(config);
        RefreshFavouritedItems();
    }

    [RelayCommand]
    private async Task OnAbortAsync()
    {
        await ResetAndRefreshAsync();
        
        ToastManager.CreateSimpleInfoToast()
            .WithTitle(this["CartClearedTitle"])
            .WithContent(this["CartClearedMessage"])
            .Queue();
    }

    [RelayCommand]
    private async Task OnPutOnHoldAsync()
    {
        Data.IsOnHold = true;
        await PayAsync("OnHold");
    }
    
    [RelayCommand]
    private async Task PayAsync(string print)
    {
        if (!Data.Items.Any())
        {
            await DialogueServices.ShowMessageAsync("AddOneItemTitle", "AddOneItemMessage");
            return;
        }

        await ProcessPaymentAsync(print is "true");
        
        ToastManager.CreateSimpleInfoToast()
            .WithTitle(this[print is "OnHold" ? "OnHoldSuccessfulTitle" : "SalesSuccessfulTitle"])
            .WithContent(this[print is "OnHold" ? "OnHoldSuccessfulMessage" : "SalesSuccessfulMessage"])
            .Queue();

        await ResetAndRefreshAsync();
    }
    
    [RelayCommand]
    private async Task ShowSuspendedAsync()
    {
        var entry = new SuspendedInvoicesWindow(new SuspendedInvoicesViewModel(null, EntryType.Add));
        await ShowContentAsync(entry, options: new(this["SuspendedInvoices"]));
    }
    
    [RelayCommand]
    private async Task ReturnSalesAsync()
    {
        App.GetService<PageNavigationService>().RequestNavigation<SellReturnInvoicesPage>();
    }
    
    [RelayCommand]
    private async Task EditSalesAsync()
    {
        App.GetService<PageNavigationService>().RequestNavigation<SalesInvoicePage>();
    }
    
    private void CheckVisibilities()
    {
        BankVisibility = Data.PaymentType is PointOfSalePaymentType.BankServices;
        SafeVisibility = Data.PaymentType is PointOfSalePaymentType.Cash;
    }

    private async Task ProcessPaymentAsync(bool printReceipt)
    {
        var dto = new DtoSalesInvoice
        {
            Details = Data.Items.Select(i => new DtoSalesInvoiceDetail
            {
                UnitId = i.UnitId,
                ItemId = i.ItemId,
                ItemName = i.Item?.Name ?? this["ItemWithNoName"],
                Quantity = i.Quantity,
                Price = i.Price,
                Discount = i.Discount
            }).ToList(),
            StoreId = Data.StoreId,
            SafeId = Data.SafeId,
            BankId = Data.BankId,
            PeymentType = Data.PaymentType switch
            {
                PointOfSalePaymentType.Cash => PeymentType.Cash,
                PointOfSalePaymentType.BankServices => PeymentType.BankServices,
                PointOfSalePaymentType.Deferred => PeymentType.Deferred
            },
            Discount = CurrentDiscountValue.GetValueOrDefault(),
            InvoiceStatus = Data.IsOnHold ? InvoiceStatus.InvoiceOnHold : InvoiceStatus.InvoicePaid,
            ClientId = Data.ClientId,
            Tax = CurrentTaxValue.GetValueOrDefault(),
            TaxType = TaxType.GetSelectedItem(),
            DiscountType = DiscountType.GetSelectedItem() switch
            {
                Fly.Shared.Modules.PurchaseAndSales.Requests.DiscountType.Value => Fly.Shared.Modules.PurchaseAndSales
                    .Enums.DiscountType.Static,
                Fly.Shared.Modules.PurchaseAndSales.Requests.DiscountType.Percentage => Fly.Shared.Modules
                    .PurchaseAndSales.Enums.DiscountType.Percent
            }
        };

        var vm = new SalesInvoiceEntryViewModel(dto, EntryType.Edit, typeof(SalesInvoicePageViewModel));
        vm.ShouldDisplayToastNotifications = false;
        await vm.SaveCommand.ExecuteAsync(null);

        if (printReceipt)
        {
            ContentHelper.GenerateReceipt(dto);
        }
    }

    public override void Dispose()
    {
        Categories = [];
        TabCategories = [];
        
        base.Dispose();
    }
}