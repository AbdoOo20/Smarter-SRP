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

namespace SmartiErpWin.Client.UserInterface.Sales.SellReturnInvoice;


public partial class SellReturnInvoiceEntryViewModel : BasePageEntryViewModel<DtoSelReturnlInvoiceModel, DtoSellReturnInvoiceDetailModel>
{
    protected override string Endpoint => "SellReturnInvoice";

    private readonly IMapper _mapper;

    [ObservableProperty]
    private long _invoiceNumber;

    [ObservableProperty]
    private long? _SellInvoiceNumber;

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

    public SellReturnInvoiceDetailsEntryWindow? EntryState { get; set; }

    [ObservableProperty]
    private decimal _totalPrice;

    [ObservableProperty]
    private ComboModelBase _secondaryComboModelBase = new();

    [ObservableProperty]
    private ComboModelBase _tertiaryComboModelBase = new();

    public SellReturnInvoiceEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoSellReturnInvoice? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType) : base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoSelReturnlInvoiceModel
        {
            SellInvoiceReturn = model ?? new DtoSellReturnInvoice
            {
                // toodo: link it to settings
                Date = DateTime.Now,
                StorageType = GlobalConfog.StorageType,
                PeymentType = GlobalConfog.PeymentType,
                InvoiceNumber = model?.InvoiceNumber,
            }
        };

        SellInvoiceNumber = Model.SellInvoiceReturn.InvoiceNumber;

        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);

        if (EntryType == EntryType.Add)
        {
            Model.SellInvoiceReturn.Date = DateTime.Now;
        }

        SecondaryComboModelBase.OnStaticComboItemChanged += () =>
        {
            CalculatePrice();
            return null;
        };

        Model.SellInvoiceReturn.Details ??= [];

        OnItemDelete += OnDelete;
        OnDataGridSelectionChanged += OnDataSelectionChanged;

        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        ComboModelBase.SetStaticItemsInternal<PeymentType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItem(
            ComboModelBase.ConvertToDisplay((int)Model.SellInvoiceReturn.PeymentType));

        SecondaryComboModelBase.SetStaticItemsInternal<DiscountType>(LocaleService);
        SecondaryComboModelBase.SetStaticComboBoxItem(
            SecondaryComboModelBase.ConvertToDisplay((int)Model.SellInvoiceReturn.DiscountType));

        TertiaryComboModelBase.SetStaticItemsInternal<StorageType>(LocaleService);
        TertiaryComboModelBase.SetStaticComboBoxItem(
            TertiaryComboModelBase.ConvertToDisplay((int)Model.SellInvoiceReturn.StorageType));

        if (EntryType == EntryType.Add)
        {
            var request = await RequestSpecificAsync<long>("Number");
            InvoiceNumber = request.Data;
        }
        else
        {
            InvoiceNumber = Model.SellInvoiceReturn.Number;
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

        var data = new List<DtoSellReturnInvoiceDetailModel>();

        foreach (var dto in Model.SellInvoiceReturn.Details)
        {
            var model = _mapper.Map<DtoSellReturnInvoiceDetailModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;

            var units = (await RequestComboModelsAsync<ComboModel>("Units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }

        Model.SellReturnInvoiceDetailModels.AddRange(data);

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
        if (Model is null || Model.SellInvoiceReturn is null) return;
        var storageId = StorageType == StorageType.Normal
            ? ComboModelBase.GetSelectedItemId(1)
            : ComboModelBase.GetSelectedItemId(5);
        SynchroniseTypeBase<SellReturnInvoiceDetailsEntryViewModel>(storageId);
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        if (Model?.SellReturnInvoiceDetailModels is null) return;

        SetData(Model.SellReturnInvoiceDetailModels);
        RefreshDataGrid();

        CalculatePrice();
        SynchroniseTypeBase<SellReturnInvoiceDetailsEntryViewModel>(new DtoSellReturnInvoiceDetailModel());
        InformComboChange();

        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("Clients");
            ComboModelBase.SetComboModelOrDefault(Model.SellInvoiceReturn.ClientId);

            await RequestApiComboItemsFromInnerEndpointAsync("Stores", 1);
            ComboModelBase.SetComboModelOrDefault(Model.SellInvoiceReturn.StoreId, 1, true);

            await RequestApiComboItemsFromInnerEndpointAsync("Safes", 2);
            ComboModelBase.SetComboModelOrDefault(Model.SellInvoiceReturn.SafeId, 2, true);

            //await RequestApiComboItemsFromInnerEndpointAsync("MarketerStores", 5);
            //ComboModelBase.SetComboModelOrDefault(Model.SellInvoiceReturn.MarketerStorageId, 5);

            /*await RequestApiComboItemsFromInnerEndpointAsync("Representatives", 4);
            ComboModelBase.SetComboModelOrDefault(Model.SellInvoiceReturn.RepresentativeId, 4);*/
        };
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

        //ComboModelBase.SetComboModelOrDefault(Model.SellInvoiceReturn.RepresentativeId, 0);
    }

    [ObservableProperty]
    private StorageType _storageType;

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
        var @new = new DtoSellReturnInvoiceDetailModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid()),
            TemporaryStoreId = ComboModelBase.GetSelectedItemId(1)
        };

        Model.SellReturnInvoiceDetailModels.Add(@new);
        SetData(Model.SellReturnInvoiceDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSellReturnInvoiceDetailModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoSellReturnInvoiceDetailModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }

    private void Edit(DtoSellReturnInvoiceDetailModel model)
    {
        model.TemporaryStoreId = ComboModelBase.GetSelectedItemId(1);
        SynchroniseTypeBase<SellReturnInvoiceDetailsEntryViewModel>(model);
        SynchroniseTypeBase<SellReturnInvoiceDetailsEntryViewModel>(Model.SellInvoiceReturn.StoreId);
    }

    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;

        var model = DataGridServices.FindModel<DtoSellReturnInvoiceDetailModel>(sender);

        if (model is null)
            return;

        Model.SellReturnInvoiceDetailModels = HelperServices
            .RemoveItemFromModelList(Model.SellReturnInvoiceDetailModels.AsEnumerable(), model)
            .OfType<DtoSellReturnInvoiceDetailModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoSellReturnInvoiceDetailModel model)
    {
        var list = Model.SellReturnInvoiceDetailModels;

        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            list.Insert(0, model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        Model.SellReturnInvoiceDetailModels = list;
        SetData(Model.SellReturnInvoiceDetailModels);
        RefreshDataGrid();
        //ScrollTo(index);
    }

    private void LimitVisibility()
    {
        //_isReturn = true;

        ApplicationTitle = this["ReturnSales"];
        // FullVisibility = false.ApplyVisibility();
        //NormalStoreVisibility = false.ApplyVisibility();
        //MarketerStoreVisibility = false.ApplyVisibility();
        //SerialNumberVisibility = false.ApplyVisibility();

        //Model.SellReturnInvoiceDetailModels.Clear();
        _ = RefreshAsync(false);
        EntryWindow.Height = 500;
    }

    public void CalculatePrice(decimal? discountParam = null)
    {
        var discount = discountParam ?? Model.SellInvoiceReturn.Discount;

        var isStatic = SecondaryComboModelBase.AssertThatStaticIsAt();

        var totalPrice = Model.SellReturnInvoiceDetailModels.Sum(d => (d.Price - d.Discount) * d.Quantity);
        var discountedPrice = totalPrice - (isStatic ? discount : discount
            / 100 * totalPrice);

        TotalPrice = discountedPrice;
    }

    protected override void PreSaveActions()
    {
        Model.SellInvoiceReturn.ClientId = ComboModelBase.GetSelectedItemId();
        Model.SellInvoiceReturn.StoreId = ComboModelBase.GetSelectedItemId(1);
        Model.SellInvoiceReturn.PeymentType = ComboModelBase.ConvertCurrentToEnum<PeymentType>();
        Model.SellInvoiceReturn.DiscountType = SecondaryComboModelBase.ConvertCurrentToEnum<DiscountType>();
        Model.SellInvoiceReturn.SafeId = ComboModelBase.GetSelectedItemId(2);
        //Model.SellInvoiceReturn.RepresentativeId = ComboModelBase.GetSelectedItemId(4);
        Model.SellInvoiceReturn.MarketerStorageId = ComboModelBase.GetSelectedItemId(5);
        Model.SellInvoiceReturn.StorageType = TertiaryComboModelBase.ConvertCurrentToEnum<StorageType>();
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        PreSaveActions();

        Model.SellInvoiceReturn.Details = [];

        foreach (var item in Model.SellReturnInvoiceDetailModels)
        {
            Model.SellInvoiceReturn.Details.Add(_mapper.Map<DtoSellReturnInvoiceDetail>(item));
        }

        var reqx = await PostSaveSpecificAsync(Model.SellInvoiceReturn);
        if (reqx is { Status: true })
        {
            if (_printReturn)
            {
                EventHelper.InvokeSellReturnInvoice(new(reqx.Data));
            }
        }
        else throw new("SellInvoiceFailed");

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
            case DtoSellReturnInvoiceDetailModel model:
                UpdateListing(model);
                break;
            case CodeBook.LimVis:
                LimitVisibility();
                SynchroniseTypeBase<SellReturnInvoiceDetailsEntryViewModel>(CodeBook.LimVis);
                break;
            default:
                await RefreshAsync(false);
                break;
        }

        CalculatePrice();
    }
}