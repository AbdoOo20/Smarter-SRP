using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.SalesModels;
using SmartiErpWin.Client.Helpers;
using DiscountType = Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice;


public partial class PurchaseReturnInvoiceEntryViewModel : BasePageEntryViewModel<DtoPurchaseReturnlInvoiceModel, DtoPurchaseReturnlInvoiceDetailModel>
{
    protected override string Endpoint => "PurchaseReturnInvoice";

    private readonly IMapper _mapper;

    [ObservableProperty]
    private long _invoiceNumber;

    [ObservableProperty]
    private long? _PurchaseInvoiceNumber;

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

    public PurchaseReturnInvoiceDetailsEntryWindow? EntryState { get; set; }

    [ObservableProperty]
    private decimal _totalPrice;

    [ObservableProperty]
    private ComboModelBase _secondaryComboModelBase = new();

    [ObservableProperty]
    private ComboModelBase _tertiaryComboModelBase = new();

    public PurchaseReturnInvoiceEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoPurchaseReturnInvoice? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType) : base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoPurchaseReturnlInvoiceModel
        {
            PurchaseReturnlInvoiceReturn = model ?? new DtoPurchaseReturnInvoice
            {
                // toodo: link it to settings
                Date = DateTime.Now,
                StorageType = StorageType.Marketer,
                PeymentType = PeymentType.Deferred,
                InvoiceNumber = model?.InvoiceNumber,
            }
        };
        PurchaseInvoiceNumber = Model.PurchaseReturnlInvoiceReturn.InvoiceNumber;

        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);

        if (EntryType == EntryType.Add)
        {
            Model.PurchaseReturnlInvoiceReturn.Date = DateTime.Now;
        }

        SecondaryComboModelBase.OnStaticComboItemChanged += () =>
        {
            CalculatePrice();
            return null;
        };

        Model.PurchaseReturnlInvoiceReturn.Details ??= [];

        OnItemDelete += OnDelete;
        OnDataGridSelectionChanged += OnDataSelectionChanged;

        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        ComboModelBase.SetStaticItemsInternal<PeymentType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItem(
            ComboModelBase.ConvertToDisplay((int)Model.PurchaseReturnlInvoiceReturn.PeymentType));

        SecondaryComboModelBase.SetStaticItemsInternal<DiscountType>(LocaleService);
        SecondaryComboModelBase.SetStaticComboBoxItem(
            SecondaryComboModelBase.ConvertToDisplay((int)Model.PurchaseReturnlInvoiceReturn.DiscountType));

        TertiaryComboModelBase.SetStaticItemsInternal<StorageType>(LocaleService);
        TertiaryComboModelBase.SetStaticComboBoxItem(
            TertiaryComboModelBase.ConvertToDisplay((int)Model.PurchaseReturnlInvoiceReturn.StorageType));

        if (EntryType == EntryType.Add)
        {
            var request = await RequestSpecificAsync<long>("Number");
            InvoiceNumber = request.Data;
        }
        else
        {
            InvoiceNumber = Model.PurchaseReturnlInvoiceReturn.Number;
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

        var data = new List<DtoPurchaseReturnlInvoiceDetailModel>();
        
        foreach (var dto in Model.PurchaseReturnlInvoiceReturn.Details.AsEnumerable().Reverse())
        {
            var model = _mapper.Map<DtoPurchaseReturnlInvoiceDetailModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;

            var units = (await RequestComboModelsAsync<ComboModel>("Units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }

        Model.PurchaseReturnInvoiceDetailModels.AddRange(data);

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
        if (Model is null || Model.PurchaseReturnlInvoiceReturn is null) return;
        Identifier storageId = StorageType == StorageType.Normal
            ? ComboModelBase.GetSelectedItemId(1)
            : ComboModelBase.GetSelectedItemId(5);
        SynchroniseTypeBase<PurchaseReturnInvoiceDetailsEntryViewModel>(storageId);
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.PurchaseReturnInvoiceDetailModels);
        RefreshDataGrid();


        if (refreshCombos && Model is not null && Model.PurchaseReturnlInvoiceReturn is not null)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("Stores");
            ComboModelBase.SetComboModelOrDefault(Model.PurchaseReturnlInvoiceReturn.StoreId);
            
            await RequestApiComboItemsFromInnerEndpointAsync("Supplier", 1);
            ComboModelBase.SetComboModelOrDefault(Model.PurchaseReturnlInvoiceReturn.SupplierId, 1);
            
            await RequestApiComboItemsFromInnerEndpointAsync("Safes", 2);
            ComboModelBase.SetComboModelOrDefault(Model.PurchaseReturnlInvoiceReturn.SafeId, 2);

        };

        SynchroniseTypeBase<PurchaseReturnInvoiceDetailsEntryViewModel>(new DtoPurchaseReturnlInvoiceDetailModel());
        CalculatePrice();
        //InformComboChange();
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

        //ComboModelBase.SetComboModelOrDefault(Model.PurchaseReturnlInvoiceReturn.RepresentativeId, 0);
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
        var @new = new DtoPurchaseReturnlInvoiceDetailModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid()),
            TemporaryStoreId = ComboModelBase.GetSelectedItemId(1)
        };

        Model.PurchaseReturnInvoiceDetailModels.Add(@new);
        SetData(Model.PurchaseReturnInvoiceDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchaseReturnlInvoiceDetailModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoPurchaseReturnlInvoiceDetailModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }

    private void Edit(DtoPurchaseReturnlInvoiceDetailModel model)
    {
        model.TemporaryStoreId = ComboModelBase.GetSelectedItemId(1);
        SynchroniseTypeBase<PurchaseReturnInvoiceDetailsEntryViewModel>(model);
        SynchroniseTypeBase<PurchaseReturnInvoiceDetailsEntryViewModel>(Model.PurchaseReturnlInvoiceReturn.StoreId);
    }

    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;

        var model = DataGridServices.FindModel<DtoPurchaseReturnlInvoiceDetailModel>(sender);

        if (model is null)
            return;

        Model.PurchaseReturnInvoiceDetailModels = HelperServices
            .RemoveItemFromModelList(Model.PurchaseReturnInvoiceDetailModels.AsEnumerable(), model)
            .OfType<DtoPurchaseReturnlInvoiceDetailModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoPurchaseReturnlInvoiceDetailModel model)
    {
        var list = Model.PurchaseReturnInvoiceDetailModels;

        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            list.Insert(0, model);
            //list.Add(model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        Model.PurchaseReturnInvoiceDetailModels = list;
        SetData(Model.PurchaseReturnInvoiceDetailModels);
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

        //Model.PurchaseReturnInvoiceDetailModels.Clear();
        _ = RefreshAsync(false);
        EntryWindow.Height = 770;
    }

    public void CalculatePrice(decimal? discountParam = null)
    {
        var discount = discountParam ?? Model.PurchaseReturnlInvoiceReturn.Discount;

        var isStatic = SecondaryComboModelBase.AssertThatStaticIsAt();

        var totalPrice = Model.PurchaseReturnInvoiceDetailModels.Sum(d => (d.Price - d.Discount) * d.Quantity);
        var discountedPrice = totalPrice - (isStatic ? discount : discount
            / 100 * totalPrice);

        TotalPrice = discountedPrice;
    }

    protected override void PreSaveActions()
    {
        Model.PurchaseReturnlInvoiceReturn.StoreId = ComboModelBase.GetSelectedItemId();
        Model.PurchaseReturnlInvoiceReturn.SupplierId = ComboModelBase.GetSelectedItemId(1);
        Model.PurchaseReturnlInvoiceReturn.PeymentType = ComboModelBase.ConvertCurrentToEnum<PeymentType>();
        Model.PurchaseReturnlInvoiceReturn.DiscountType = SecondaryComboModelBase.ConvertCurrentToEnum<DiscountType>();
        Model.PurchaseReturnlInvoiceReturn.SafeId = ComboModelBase.GetSelectedItemId(2);
        //Model.PurchaseReturnlInvoiceReturn.RepresentativeId = ComboModelBase.GetSelectedItemId(4);
        Model.PurchaseReturnlInvoiceReturn.StorageType = TertiaryComboModelBase.ConvertCurrentToEnum<StorageType>();
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        PreSaveActions();

        Model.PurchaseReturnlInvoiceReturn.Details = [];

        foreach (var item in Model.PurchaseReturnInvoiceDetailModels)
        {
            Model.PurchaseReturnlInvoiceReturn.Details.Add(_mapper.Map<DtoPurchaseReturnInvoiceDetail>(item));
        }

        var reqx = await PostSaveSpecificAsync(Model.PurchaseReturnlInvoiceReturn);
        if (reqx is { Status: true })
        {
            if (_printReturn)
            {
                EventHelper.InvokePurchaseReturnInvoice(new(reqx.Data));
            }
        }
        else throw new("PurchaseInvoiceFailed");

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
            case DtoPurchaseReturnlInvoiceDetailModel model:
                UpdateListing(model);
                break;
            case CodeBook.LimVis:
                LimitVisibility();
                SynchroniseTypeBase<PurchaseReturnInvoiceDetailsEntryViewModel>(CodeBook.LimVis);
                break;
            default:
                await RefreshAsync(false);
                break;
        }

        CalculatePrice();
    }
}