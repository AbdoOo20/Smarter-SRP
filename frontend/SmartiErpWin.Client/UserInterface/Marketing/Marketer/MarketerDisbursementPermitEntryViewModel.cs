using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Marketers.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.MarketingModels;
using SmartiErpWin.Client.Helpers;

namespace SmartiErpWin.Client.UserInterface.Marketing.Marketer;

public sealed partial class MarketerDisbursementPermitEntryViewModel
    : BasePageEntryViewModel<DtoMarketerDisbursementPermitModel, DtoMarketerDisbursementPermitDetailsModel>
{
    protected override string Endpoint => "MarketerDisbursementPermit";

    private readonly IMapper _mapper;

    [ObservableProperty]
    private long _invoiceNumber;

    public MarketerDisbursementPermitDetailsEntryWindow? EntryState { get; set; }

    public MarketerDisbursementPermitEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoMarketerDisbursementPermit? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType) : base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoMarketerDisbursementPermitModel
        {
            DtoMarketerDisbursementPermit = model ?? new()
            {
                Date = DateTime.Now
            }
        };

        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);

        Model.DtoMarketerDisbursementPermit.Details ??= [];

        if (EntryType == EntryType.Edit)
        {
            SynchroniseTypeBase<MarketerDisbursementPermitDetailsEntryViewModel>(model.StoreId);
        }

        OnItemDelete += OnDelete;

        OnDataGridSelectionChanged += OnDataSelectionChanged;
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        var categories = (await RequestComboModelsAsync<ComboModel>("items")).Data;

        var data = new List<DtoMarketerDisbursementPermitDetailsModel>();

        foreach (var dto in Model.DtoMarketerDisbursementPermit.Details)
        {
            var model = _mapper.Map<DtoMarketerDisbursementPermitDetailsModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }

        if (EntryType == EntryType.Add)
        {
            var request = await RequestSpecificAsync<long>("Number");
            InvoiceNumber = request.Data;
        }
        else
        {
            InvoiceNumber = Model.DtoMarketerDisbursementPermit.Number;
        }

        Model.MarketerDisbursementPermitDetailsModels.AddRange(data);

        if (refresh)
        {
            await RefreshAsync();
        }
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.MarketerDisbursementPermitDetailsModels);
        RefreshDataGrid();
        SynchroniseTypeBase<MarketerDisbursementPermitDetailsEntryViewModel>(new DtoMarketerDisbursementPermitDetailsModel());

        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("marketers");
            ComboModelBase.SetComboModelOrDefault(Model.DtoMarketerDisbursementPermit.MarketerId);

            await RequestApiComboItemsFromInnerEndpointAsync("stores", 1);
            ComboModelBase.SetComboModelOrDefault(Model.DtoMarketerDisbursementPermit.StoreId, 1);
        }
    }

    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoMarketerDisbursementPermitDetailsModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid())
        };

        Model.MarketerDisbursementPermitDetailsModels.Add(@new);
        SetData(Model.MarketerDisbursementPermitDetailsModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoMarketerDisbursementPermitDetailsModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoMarketerDisbursementPermitDetailsModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }

    private void Edit(DtoMarketerDisbursementPermitDetailsModel model)
    {
        SynchroniseTypeBase<MarketerDisbursementPermitDetailsEntryViewModel>(model);
        SynchroniseTypeBase<MarketerDisbursementPermitDetailsEntryViewModel>(Model.DtoMarketerDisbursementPermit.StoreId);
    }

    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;

        var model = DataGridServices.FindModel<DtoMarketerDisbursementPermitDetailsModel>(sender);

        if (model is null)
            return;

        Model.MarketerDisbursementPermitDetailsModels = HelperServices
            .RemoveItemFromModelList(Model.MarketerDisbursementPermitDetailsModels.AsEnumerable(), model)
            .OfType<DtoMarketerDisbursementPermitDetailsModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoMarketerDisbursementPermitDetailsModel model)
    {
        var list = Model.MarketerDisbursementPermitDetailsModels;

        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            list.Add(model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        Model.MarketerDisbursementPermitDetailsModels = list;
        SetData(Model.MarketerDisbursementPermitDetailsModels);
        RefreshDataGrid();
    }

    protected override void PreSaveActions()
    {
        Model.DtoMarketerDisbursementPermit.MarketerId = ComboModelBase.GetSelectedItemId();
        Model.DtoMarketerDisbursementPermit.StoreId = ComboModelBase.GetSelectedItemId(1);
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;

        PreSaveActions();

        Model.DtoMarketerDisbursementPermit.Details = [];

        foreach (var item in Model.MarketerDisbursementPermitDetailsModels)
        {
            Model.DtoMarketerDisbursementPermit.Details
                     .Add(_mapper.Map<DtoMarketerDisbursementPermitDetail>(item));
        }

        await PostSaveSpecificAsync(Model.DtoMarketerDisbursementPermit);

        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }

    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync();
        EventHelper.InvokeMarketerDisbursement(new(Model.DtoMarketerDisbursementPermit));
    }

    public void OnStorageChanged()
    {
        SynchroniseTypeBase<MarketerDisbursementPermitDetailsEntryViewModel>(ComboModelBase.GetSelectedItemId(1));
    }

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is CodeBook.RefData)
        {
            RefreshDataGrid();
        }
        else if (args.SingleOrDefault() is DtoMarketerDisbursementPermitDetailsModel model)
        {
            UpdateListing(model);
        }
        else
        {
            await RefreshAsync(false);
        }
    }
}