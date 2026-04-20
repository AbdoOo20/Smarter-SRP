using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

namespace SmartiErpWin.Client.UserInterface.Stores.SupplyPermits;

public partial class SupplyPermitPageEntryViewModel : BasePageEntryViewModel<DtoSupplyPermitModel, DtoSupplyPermitDetailModel>
{
    protected override string Endpoint => "SupplyPermit";

    private readonly IMapper _mapper;
    
    public SupplyPermitDetailsEntryWindow? EntryState { get; set; }

    public SupplyPermitPageEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoSupplyPermit? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType): base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoSupplyPermitModel
        {
            DtoSupplyPermit = model ?? new()
        };
        
        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        if (EntryType == EntryType.Add)
        {
            Model.DtoSupplyPermit.Date = DateTime.Now;
        }
        
        Model.DtoSupplyPermit.SupplyPermitDetails ??= [];
        
        OnItemDelete += OnDelete;

        OnDataGridSelectionChanged += OnDataSelectionChanged;
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        var categories = (await RequestComboModelsAsync<ComboModel>("item")).Data;

        var data = new List<DtoSupplyPermitDetailModel>();

        foreach (var dto in Model.DtoSupplyPermit.SupplyPermitDetails)
        {
            var model = _mapper.Map<DtoSupplyPermitDetailModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }
        
        Model.SupplyPermitDetailModels.AddRange(data);

        if (refresh)
        {
            await RefreshAsync();
        }
    }
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.SupplyPermitDetailModels);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("suppliers");
            await RequestApiComboItemsFromInnerEndpointAsync("stores", 1);

            ComboModelBase.SetComboModelOrDefault(Model.DtoSupplyPermit.SupplierId);
            ComboModelBase.SetComboModelOrDefault(Model.DtoSupplyPermit.StoreId, 1);
        }
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoSupplyPermitDetailModel
        {
            ExpireDate = DateTime.Now,
            TemporaryId = Identifier.Create(Guid.NewGuid())
        };
        
        Model.SupplyPermitDetailModels.Add(@new);
        SetData(Model.SupplyPermitDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSupplyPermitDetailModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoSupplyPermitDetailModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }
    
    private void Edit(DtoSupplyPermitDetailModel model)
    {
        SynchroniseTypeBase<SupplyPermitDetailsEntryViewModel>(model);
    }
    
    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;
        
        var model = DataGridServices.FindModel<DtoSupplyPermitDetailModel>(sender);

        if (model is null)
            return;

        Model.SupplyPermitDetailModels = HelperServices
            .RemoveItemFromModelList(Model.SupplyPermitDetailModels.AsEnumerable(), model)
            .OfType<DtoSupplyPermitDetailModel>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoSupplyPermitDetailModel model)
    {
        var list = Model.SupplyPermitDetailModels;
        
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

        Model.SupplyPermitDetailModels = list;
        SetData(Model.SupplyPermitDetailModels);
        RefreshDataGrid();
    }

    protected override void PreSaveActions()
    {
        Model.DtoSupplyPermit.SupplierId = ComboModelBase.GetSelectedItemId();
        Model.DtoSupplyPermit.StoreId = ComboModelBase.GetSelectedItemId(1);
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        
        PreSaveActions();

        Model.DtoSupplyPermit.SupplyPermitDetails = [];

        foreach (var item in Model.SupplyPermitDetailModels)
        {
            Model.DtoSupplyPermit.SupplyPermitDetails = 
                Model.DtoSupplyPermit.SupplyPermitDetails.Append(_mapper.Map<DtoSupplyPermitDetail>(item));
        }
        
        await PostSaveSpecificAsync(Model.DtoSupplyPermit);
        
        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }
    
    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is CodeBook.RefData)
        {
            RefreshDataGrid();
        }
        else if (args.SingleOrDefault() is DtoSupplyPermitDetailModel model)
        {
            UpdateListing(model);
        }
        else
        {
            await RefreshAsync(false);
        }
    }
}