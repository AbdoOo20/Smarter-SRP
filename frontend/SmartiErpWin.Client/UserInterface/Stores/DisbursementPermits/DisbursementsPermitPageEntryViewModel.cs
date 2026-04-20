using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

namespace SmartiErpWin.Client.UserInterface.Stores.DisbursementPermits;

public sealed partial class DisbursementsPermitPageEntryViewModel : BasePageEntryViewModel<DtoDisbursementPermitModel, DtoDisbursementPermitDetailModel>
{
    protected override string Endpoint => "DisbursementPermit";

    private readonly IMapper _mapper;

    public DisbursementsPermitDetailsEntryWindow? EntryState { get; set; }

    public DisbursementsPermitPageEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoDisbursementPermit? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType): base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoDisbursementPermitModel
        {
            DtoDisbursementPermit = model ?? new()
        };
        
        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        if (EntryType == EntryType.Add)
        {
            Model.DtoDisbursementPermit.Date = DateTime.Now;
        }
        
        Model.DtoDisbursementPermit.DisbursementPermitDetails ??= [];
        
        OnItemDelete += OnDelete;

        OnDataGridSelectionChanged += OnDataSelectionChanged;
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        var categories = (await RequestComboModelsAsync<ComboModel>("items")).Data;

        var data = new List<DtoDisbursementPermitDetailModel>();

        foreach (var dto in Model.DtoDisbursementPermit.DisbursementPermitDetails)
        {
            var model = _mapper.Map<DtoDisbursementPermitDetailModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }
        
        Model.DisbursementPermitDetailModels.AddRange(data);

        if (refresh)
        {
            await RefreshAsync();
        }
    }
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.DisbursementPermitDetailModels);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("costcente");
            await RequestApiComboItemsFromInnerEndpointAsync("stores", 1);

            ComboModelBase.SetComboModelOrDefault(Model.DtoDisbursementPermit.CostCenterId);
            ComboModelBase.SetComboModelOrDefault(Model.DtoDisbursementPermit.StoreId, 1);
        }
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoDisbursementPermitDetailModel
        {
            ExpireDate = DateTime.Now,
            TemporaryId = Identifier.Create(Guid.NewGuid())
        };
        
        Model.DisbursementPermitDetailModels.Add(@new);
        SetData(Model.DisbursementPermitDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoDisbursementPermitDetailModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoDisbursementPermitDetailModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }
    
    private void Edit(DtoDisbursementPermitDetailModel model)
    {
        SynchroniseTypeBase<DisbursementsPermitDetailsEntryViewModel>(model);
    }
    
    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;
        
        var model = DataGridServices.FindModel<DtoDisbursementPermitDetailModel>(sender);

        if (model is null)
            return;

        Model.DisbursementPermitDetailModels = HelperServices
            .RemoveItemFromModelList(Model.DisbursementPermitDetailModels.AsEnumerable(), model)
            .OfType<DtoDisbursementPermitDetailModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoDisbursementPermitDetailModel model)
    {
        var list = Model.DisbursementPermitDetailModels;
        
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

        Model.DisbursementPermitDetailModels = list;
        SetData(Model.DisbursementPermitDetailModels);
        RefreshDataGrid();
    }

    protected override void PreSaveActions()
    {
        Model.DtoDisbursementPermit.CostCenterId = ComboModelBase.GetSelectedItemId();
        Model.DtoDisbursementPermit.StoreId = ComboModelBase.GetSelectedItemId(1);
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        
        PreSaveActions();

        Model.DtoDisbursementPermit.DisbursementPermitDetails = [];

        foreach (var item in Model.DisbursementPermitDetailModels)
        {
            Model.DtoDisbursementPermit.DisbursementPermitDetails = Model.DtoDisbursementPermit.DisbursementPermitDetails
                .Append(_mapper.Map<DtoDisbursementPermitDetail>(item));
        }
        
        var innerEndpoint = EntryType == EntryType.Add ? "Create" : "Update";
        
        await PostSaveSpecificAsync(Model.DtoDisbursementPermit, innerEndpoint);
        
        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }
    
    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is CodeBook.RefData)
        {
            RefreshDataGrid();
        }
        else if (args.SingleOrDefault() is DtoDisbursementPermitDetailModel model)
        {
            UpdateListing(model);
        }
        else
        {
            await RefreshAsync(false);
        }
    }
}