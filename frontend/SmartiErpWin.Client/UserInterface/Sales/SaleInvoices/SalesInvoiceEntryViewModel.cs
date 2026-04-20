using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.SalesModels;
using SmartiErpWin.Client.Helpers;
using DiscountType = Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoiceEntryViewModel : BasePageEntryViewModel<DtoSalesInvoiceModel, DtoSalesInvoiceDetailsModel>
{
    protected override string Endpoint => "Sales";

    private readonly IMapper _mapper;

    [ObservableProperty]
    private long _invoiceNumber;

    [ObservableProperty]
    private Visibility _paymentMethodVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _safeVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _normalStoreVisibility = Visibility.Visible;

    [ObservableProperty]
    private Visibility _marketerStoreVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _fullVisibility = Visibility.Visible;

    private bool _isReturn;

    public SalesInvoiceDetailsEntryWindow? EntryState { get; set; }

    [ObservableProperty]
    private decimal _totalPrice;

    [ObservableProperty]
    private ComboModelBase _secondaryComboModelBase = new();

    [ObservableProperty]
    private StorageType _storageType;

    [ObservableProperty]
    private ComboModelBase _tertiaryComboModelBase = new();

    public SalesInvoiceEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoSalesInvoice? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType) : base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoSalesInvoiceModel
        {
            DtoSalesInvoice = model ?? new DtoSalesInvoice
            {
                // toodo: link it to settings
                Date = DateTime.Now,
                StorageType = GlobalConfog.StorageType,
                PeymentType = GlobalConfog.PeymentType
            }
        };

        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);

        if (EntryType == EntryType.Add)
        {
            Model.DtoSalesInvoice.Date = DateTime.Now;
        }
        if (EntryType == EntryType.Edit)
        {
            SynchroniseTypeBase<SalesInvoiceDetailsEntryViewModel>(model.StoreId);
        }

        SecondaryComboModelBase.OnStaticComboItemChanged += () =>
        {
            CalculatePrice();
            return null;
        };

        Model.DtoSalesInvoice.Details ??= [];

        OnItemDelete += OnDelete;
        OnDataGridSelectionChanged += OnDataSelectionChanged;

        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        ComboModelBase.SetStaticItemsInternal<PeymentType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItem(
            ComboModelBase.ConvertToDisplay((int)Model.DtoSalesInvoice.PeymentType));

        SecondaryComboModelBase.SetStaticItemsInternal<DiscountType>(LocaleService);
        SecondaryComboModelBase.SetStaticComboBoxItem(
            SecondaryComboModelBase.ConvertToDisplay((int)Model.DtoSalesInvoice.DiscountType));

        TertiaryComboModelBase.SetStaticItemsInternal<StorageType>(LocaleService);
        TertiaryComboModelBase.SetStaticComboBoxItem(
            TertiaryComboModelBase.ConvertToDisplay((int)Model.DtoSalesInvoice.StorageType));

        if (EntryType == EntryType.Add)
        {
            var request = await RequestSpecificAsync<long>("Number");
            InvoiceNumber = request.Data;
        }
        else
        {
            InvoiceNumber = Model.DtoSalesInvoice.Number;
            SetClient();
        }

        CheckVisibilities();

        ComboModelBase.OnStaticComboItemChanged += () =>
        {
            CheckVisibilities();
            return default;
        };

        ComboModelBase.OnApiComboItemChanged += () =>
        {
            CheckVisibilities();
            return default;
        };

        var categories = (await RequestComboModelsAsync<ComboModel>("Items")).Data;

        var data = new List<DtoSalesInvoiceDetailsModel>();

        foreach (var dto in Model.DtoSalesInvoice.Details.Reverse())
        {
            var model = _mapper.Map<DtoSalesInvoiceDetailsModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("Units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;
            
            data.Add(model);
        }

        Model.SalesInvoiceDetailModels.AddRange(data);
        
        CalculatePrice();

        if (refresh)
        {
            await RefreshAsync();
        }
    }

    private void CheckVisibilities()
    {
        PaymentMethodVisibility = ComboModelBase.AssertThatStaticIsAt().ApplyVisibility();
        SafeVisibility = ComboModelBase.AssertThatStaticIsAt().ApplyVisibility();
    }

    public void InformComboChange()
    {
        if (Model is null || Model.DtoSalesInvoice is null) return;
        Identifier storageId = StorageType == StorageType.Normal
            ? ComboModelBase.GetSelectedItemId(1)
            : ComboModelBase.GetSelectedItemId(5);
        SynchroniseTypeBase<SalesInvoiceDetailsEntryViewModel>(storageId, StorageType);
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        if (_isReturn)
        {
            Model.SalesInvoiceDetailModels.Clear();
        }
        try
        {
            SetData(Model.SalesInvoiceDetailModels);
        }
        catch
        {
            return;
        }
        
        RefreshDataGrid();
        CalculatePrice();
        SynchroniseTypeBase<SalesInvoiceDetailsEntryViewModel>(new DtoSalesInvoiceDetailsModel());
        InformComboChange();

        if (refreshCombos)
        {
            if (Model.DtoSalesInvoice.RepresentativeId is null)
            {
                await RequestApiComboItemsFromInnerEndpointAsync("Clients");
                ComboModelBase.SetComboModelOrDefault(Model.DtoSalesInvoice.ClientId);
            }

            await RequestApiComboItemsFromInnerEndpointAsync("Stores", 1);
            ComboModelBase.SetComboModelOrDefault(Model.DtoSalesInvoice.StoreId, 1, true);

            await RequestApiComboItemsFromInnerEndpointAsync("Safes", 2);
            ComboModelBase.SetComboModelOrDefault(Model.DtoSalesInvoice.SafeId, 2, true);

            await RequestApiComboItemsFromInnerEndpointAsync("MarketerStores", 5);
            ComboModelBase.SetComboModelOrDefault(Model.DtoSalesInvoice.MarketerStorageId, 5);

            await RequestApiComboItemsFromInnerEndpointAsync("Representatives", 4);
            ComboModelBase.SetComboModelOrDefault(Model.DtoSalesInvoice.RepresentativeId, 4);
        }
    }

    public async void SetClient()
    {
        var id = ComboModelBase.GetSelectedItemId(4);

        if (id == Identifier.Empty)
            return;

        await RequestCombosUsingQueryRequestAsync("Clients", 0, new ClientQueryRequest
        {
            MarketerId = id
        });

        ComboModelBase.SetComboModelOrDefault(Model.DtoSalesInvoice.ClientId, 0);
    }

    public void SetStore()
    {
        var isNormal = TertiaryComboModelBase.GetStaticPosition() is 0;
        StorageType = isNormal ? StorageType.Normal : StorageType.Marketer;

        NormalStoreVisibility = isNormal.ApplyVisibility();
        MarketerStoreVisibility = (!isNormal).ApplyVisibility();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoSalesInvoiceDetailsModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid()),
            TemporaryStoreId = ComboModelBase.GetSelectedItemId(1)
        };

        Model.SalesInvoiceDetailModels.Add(@new);
        SetData(Model.SalesInvoiceDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
        ScrollTo(Model.SalesInvoiceDetailModels.Count - 1);
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSalesInvoiceDetailsModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoSalesInvoiceDetailsModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }

    private void Edit(DtoSalesInvoiceDetailsModel model)
    {
        model.TemporaryStoreId = ComboModelBase.GetSelectedItemId(1);
        SynchroniseTypeBase<SalesInvoiceDetailsEntryViewModel>(model);
        SynchroniseTypeBase<SalesInvoiceDetailsEntryViewModel>(Model.DtoSalesInvoice.StoreId);
    }

    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;

        var model = DataGridServices.FindModel<DtoSalesInvoiceDetailsModel>(sender);

        if (model is null)
            return;

        Model.SalesInvoiceDetailModels = HelperServices
            .RemoveItemFromModelList(Model.SalesInvoiceDetailModels.AsEnumerable(), model)
            .OfType<DtoSalesInvoiceDetailsModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public override Func<DtoSalesInvoiceDetailsModel, DtoSalesInvoiceDetailsModel, bool>? Finder => (x, y) =>
        x.TemporaryId == y.TemporaryId
    //&& x.ItemId == y.ItemId 
    //&& x.Quantity == y.Quantity
    ;

    public async void UpdateListing(DtoSalesInvoiceDetailsModel model, bool checkForExisting = false)
    {
        var list = Model.SalesInvoiceDetailModels;

        var alreadyExisting = list.FirstOrDefault(m => m.Discount == model.Discount
                                                       && m.UnitId == model.UnitId
                                                       && m.ItemId == model.ItemId);
        if (alreadyExisting is not null && checkForExisting)
        {
            // We already have a model with the same item and discount
            // so just update the already existing one with the added quantities

            alreadyExisting.Quantity += model.Quantity;
            
            UpdateListing(alreadyExisting);
            return;
        }
        
        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);
        var index = 0;

        if (item is null)
        {
            list.Insert(0, await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model));
            index = list.Count - 1;
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                {
                    list[i] = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model);
                    index = i;
                }
            }
        }

        Model.SalesInvoiceDetailModels = list;
        SetData(Model.SalesInvoiceDetailModels);
        RefreshDataGrid();
        //ScrollTo(index);
    }

    private void LimitVisibility()
    {
        _isReturn = true;

        ApplicationTitle = this["ReturnSales"];
        FullVisibility = false.ApplyVisibility();
        NormalStoreVisibility = false.ApplyVisibility();
        MarketerStoreVisibility = false.ApplyVisibility();
        SerialNumberVisibility = false.ApplyVisibility();

        Model.SalesInvoiceDetailModels.Clear();
        _ = RefreshAsync(false);
        //EntryWindow.Height = 770;
    }

    public void CalculatePrice(decimal? discountParam = null)
    {
        var discount = discountParam ?? Model.DtoSalesInvoice.Discount;

        var isStatic = SecondaryComboModelBase.AssertThatStaticIsAt();

        var sumPrice = Model.SalesInvoiceDetailModels.Sum(d => (d.Price - d.Discount) * d.Quantity);
        var finalPrice = sumPrice - (isStatic ? discount : discount / 100 * sumPrice);

        TotalPrice = finalPrice;
    }

    protected override void PreSaveActions()
    {
        Model.DtoSalesInvoice.ClientId = Identifier.Parse(ComboModelBase.Api1.Value);
        Model.DtoSalesInvoice.StoreId = ComboModelBase.GetSelectedItemId(1);
        Model.DtoSalesInvoice.PeymentType = ComboModelBase.ConvertCurrentToEnum<PeymentType>();
        Model.DtoSalesInvoice.DiscountType = SecondaryComboModelBase.ConvertCurrentToEnum<DiscountType>();
        Model.DtoSalesInvoice.SafeId = ComboModelBase.GetSelectedItemId(2);
        Model.DtoSalesInvoice.RepresentativeId = ComboModelBase.GetSelectedItemId(4);
        Model.DtoSalesInvoice.MarketerStorageId = ComboModelBase.GetSelectedItemId(5);
        Model.DtoSalesInvoice.StorageType = TertiaryComboModelBase.ConvertCurrentToEnum<StorageType>();
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        PreSaveActions();

        Model.DtoSalesInvoice.Details = [];

        foreach (var item in Model.SalesInvoiceDetailModels)
        {
            Model.DtoSalesInvoice.Details.Add(_mapper.Map<DtoSalesInvoiceDetail>(item));
        }

        if (_isReturn)
        {
            //Model.DtoSalesInvoice.Id = Identifier.Empty;
            //var req = await PostSaveSpecificAndReturnAsync<DtoSalesInvoice, DtoSellReturnInvoice>(
            //    Model.DtoSalesInvoice,
            //    "SellReturnInvoice",
            //    appendOriginal: false);

            //if (req is { Status: true })
            //{
            //    if (_printReturn)
            //    {
            //        EventHelper.InvokePartialSalesReturn(new(req.Data));
            //    }
            //}
            //else throw new("SaleInvoiceReturnFailed");
            throw new("SaleInvoiceReturnFailed");
        }
        else
        {
            var reqx = await PostSaveSpecificAsync(Model.DtoSalesInvoice);
            if (reqx is { Status: true })
            {
                if (_printReturn)
                {
                    EventHelper.InvokeSales(new(reqx.Data));
                }
            }
            else throw new("SellInvoiceFailed");
        }

        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }

    private bool _printReturn;

    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        _printReturn = true;

        await SaveAsync();

        _printReturn = false;

        return;
    }

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.FirstOrDefault())
        {
            case CodeBook.RefData:
                RefreshDataGrid();
                break;
            case DtoSalesInvoiceDetailsModel model:
                var shouldCheck = args.ElementAtOrDefault(1)?.ToString() is CodeBook.IsNew;
                UpdateListing(model, shouldCheck);
                break;
            case CodeBook.LimVis:
                LimitVisibility();
                SynchroniseTypeBase<SalesInvoiceDetailsEntryViewModel>(CodeBook.LimVis);
                break;
            default:
                await RefreshAsync(false);
                break;
        }

        CalculatePrice();
    }
}