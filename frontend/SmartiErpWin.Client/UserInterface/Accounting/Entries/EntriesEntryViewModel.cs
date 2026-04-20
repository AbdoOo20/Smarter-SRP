using AutoMapper;
using Fly.Shared.Modules.Accounting.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.AccountingModels;

namespace SmartiErpWin.Client.UserInterface.Accounting.Entries;

public partial class EntriesEntryViewModel : BasePageEntryViewModel<DtoEntryModel, DtoEntryDetailModel>
{
    protected override string Endpoint => "Entry";

    private readonly IMapper _mapper;
    
    public EntryDetailEntryWindow? EntryState { get; set; }

    public EntriesEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoEntry? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType): base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoEntryModel
        {
            DtoEntry = model ?? new()
        };
        
        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        if (EntryType == EntryType.Add)
        {
            Model.DtoEntry.Date = DateTime.Now;
        }
        
        Model.DtoEntry.Details ??= [];
        
        OnItemDelete += OnDelete;

        OnDataGridSelectionChanged += OnDataSelectionChanged;
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        var units = (await RequestComboModelsAsync<ComboModel>("accounts")).Data;
        
        var data = Model?.DtoEntry?.Details?.Select(dto =>
        {
            var model = _mapper.Map<DtoEntryDetailModel>(dto);

            model.AccountName = units.FirstOrDefault(u 
                => u.Value == dto.AccountId.Value.ToString())?.Display;
            model.TemporaryId = dto.Id;

            return model;
        }).ToList();
        
        Model?.EntryDetailModels.AddRange(data ?? []);

        if (refresh)
        {
            await RefreshAsync();
        }
    }
    
    protected override Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model?.EntryDetailModels);
        RefreshDataGrid();

        return Task.CompletedTask;
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoEntryDetailModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoEntryDetailModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }
    
    private void Edit(DtoEntryDetailModel model)
    {
        SynchroniseTypeBase<EntriesDetailPageEntryViewModel>(model);
    }
    
    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;
        
        var model = DataGridServices.FindModel<DtoEntryDetailModel>(sender);

        if (model is null)
            return;

        Model.EntryDetailModels = HelperServices
            .RemoveItemFromModelList(Model.EntryDetailModels.AsEnumerable(), model)
            .OfType<DtoEntryDetailModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoEntryDetailModel model)
    {
        var list = Model.EntryDetailModels;
        
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

        Model.EntryDetailModels = list;
        SetData(Model.EntryDetailModels);
        RefreshDataGrid();
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        
        PreSaveActions();

        Model.DtoEntry.Details = [];

        foreach (var item in Model.EntryDetailModels)
        {
            Model.DtoEntry.Details.Add(_mapper.Map<DtoEntryDetail>(item));
        }
        
        await PostSaveSpecificAsync(Model.DtoEntry);
        
        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }
    
    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.SingleOrDefault())
        {
            case CodeBook.RefData:
                RefreshDataGrid();
                break;
            case DtoEntryDetailModel model:
                UpdateListing(model);
                break;
            default:
                await RefreshAsync(false);
                break;
        }
    }
}