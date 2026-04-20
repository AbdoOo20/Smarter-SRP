using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.QuantityCreditorModels;

namespace SmartiErpWin.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingEntryViewModel : BasePageEntryViewModel<DtoQuantityCreditingModel, DtoQuantityCreditingDetailsModel>
{
    protected override string Endpoint => "QuantityCrediting";

    private readonly IMapper _mapper;

    [ObservableProperty]
    private long _invoiceNumber;

    public QuantityCreditingDetailsEntryWindow? EntryState { get; set; }

    [ObservableProperty]
    private decimal _totalPrice;

    [ObservableProperty]
    private ComboModelBase _secondaryComboModelBase = new();

    [ObservableProperty]
    private ComboModelBase _tertiaryComboModelBase = new();

    public QuantityCreditingEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoQuantityCreditingInvoice? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType) : base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoQuantityCreditingModel
        {
            DtoQuantityCrediting = model ?? new DtoQuantityCreditingInvoice
            {
                Date = DateTime.Now,
                StorageType = Fly.Shared.Modules.PurchaseAndSales.Enums.StorageType.Normal,
            }
        };

        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);

        if (EntryType == EntryType.Add)
        {
            Model.DtoQuantityCrediting.Date = DateTime.Now;
        }

        SecondaryComboModelBase.OnStaticComboItemChanged += () =>
        {
            CalculatePrice();
            return null;
        };

        Model.DtoQuantityCrediting.Details ??= [];

        OnItemDelete += OnDelete;
        OnDataGridSelectionChanged += OnDataSelectionChanged;

        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        if (EntryType == EntryType.Add)
        {
            var request = await RequestSpecificAsync<long>("Number");
            InvoiceNumber = request.Data;
            
        }
        else
        {
            InvoiceNumber = Model.DtoQuantityCrediting.Number;
        }

        var categories = (await RequestComboModelsAsync<ComboModel>("Items")).Data;

        var data = new List<DtoQuantityCreditingDetailsModel>();

        foreach (var dto in Model.DtoQuantityCrediting.Details)
        {
            var model = _mapper.Map<DtoQuantityCreditingDetailsModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("Units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }

        Model.QuantityCreditingDetailModels.AddRange(data);

        CalculatePrice();

        if (refresh)
        {
            await RefreshAsync();
        }
    }

    public void InformComboChange()
    {
        SynchroniseTypeBase<QuantityCreditingDetailsEntryViewModel>(ComboModelBase.GetSelectedItemId(1));
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.QuantityCreditingDetailModels);
        RefreshDataGrid();

        CalculatePrice();

        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("Stores", 1);
            ComboModelBase.SetComboModelOrDefault(Model.DtoQuantityCrediting.StoreId, 1);
            ComboModelBase.SelectAtOnePoint(1, 1);



            await RequestApiComboItemsFromInnerEndpointAsync("Safes", 2);
            ComboModelBase.SetComboModelOrDefault(Model.DtoQuantityCrediting.SafeId, 2);
        }
        SynchroniseTypeBase<QuantityCreditingDetailsEntryViewModel>(new DtoQuantityCreditingDetailsModel());
        InformComboChange();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoQuantityCreditingDetailsModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid()),
            TemporaryStoreId = ComboModelBase.GetSelectedItemId(1)
        };

        Model.QuantityCreditingDetailModels.Add(@new);
        SetData(Model.QuantityCreditingDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoQuantityCreditingDetailsModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoQuantityCreditingDetailsModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }

    private void Edit(DtoQuantityCreditingDetailsModel model)
    {
        model.TemporaryStoreId = ComboModelBase.GetSelectedItemId(1);
        SynchroniseTypeBase<QuantityCreditingDetailsEntryViewModel>(model);
    }

    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;

        var model = DataGridServices.FindModel<DtoQuantityCreditingDetailsModel>(sender);

        if (model is null)
            return;

        Model.QuantityCreditingDetailModels = HelperServices
            .RemoveItemFromModelList(Model.QuantityCreditingDetailModels.AsEnumerable(), model)
            .OfType<DtoQuantityCreditingDetailsModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoQuantityCreditingDetailsModel model)
    {
        var list = Model.QuantityCreditingDetailModels;

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

        Model.QuantityCreditingDetailModels = list;
        SetData(Model.QuantityCreditingDetailModels);
        RefreshDataGrid();
        //ScrollTo(index);
    }

    public void CalculatePrice(decimal? discountParam = null)
    {
        var totalPrice = Model.QuantityCreditingDetailModels.Sum(d => d.Price * d.Quantity);

        TotalPrice = totalPrice;
    }

    protected override void PreSaveActions()
    {
        Model.DtoQuantityCrediting.StoreId = ComboModelBase.GetSelectedItemId(1);
        Model.DtoQuantityCrediting.SafeId = ComboModelBase.GetSelectedItemId(2);
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;

        PreSaveActions();

        Model.DtoQuantityCrediting.Details = [];

        foreach (var item in Model.QuantityCreditingDetailModels)
        {
            Model.DtoQuantityCrediting.Details
                    .Add(_mapper.Map<DtoQuantityCreditingInvoiceDetail>(item));
        }

        await PostSaveSpecificAsync(Model.DtoQuantityCrediting);

        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }

    /*[RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync();
        EventHelper.InvokeQuantityCreditReport(new(Model.DtoQuantityCrediting));
    }*/

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.FirstOrDefault())
        {
            case CodeBook.RefData:
                RefreshDataGrid();
                break;
            case DtoQuantityCreditingDetailsModel model:
                UpdateListing(model);
                break;
            default:
                await RefreshAsync(false);
                break;
        }

        CalculatePrice();
    }
}