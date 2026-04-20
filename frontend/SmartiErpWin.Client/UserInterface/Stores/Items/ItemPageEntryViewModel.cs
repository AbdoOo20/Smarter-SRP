using System.ComponentModel;
using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

namespace SmartiErpWin.Client.UserInterface.Stores.Items;

public partial class ItemPageEntryViewModel : BasePageEntryViewModel<DtoItemModel, DtoItemUnitModel>
{
    protected override string Endpoint => "Item";

    [ObservableProperty]
    private bool _isExpirable;
    
    private readonly IMapper _mapper;

    [ObservableProperty]
    private DateTime? _selectedExpireDate;
    
    public ItemsUnitEntryWindow? EntryState { get; set; }

    public ItemPageEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoItem? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType): base(client)
    {
        InitBaseModel(localisationService, null, null);
        _mapper = mapper;
        
        Model = new DtoItemModel
        {
            DtoItem = model ?? new()
        };
        
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);

        Model.DtoItem.ItemUnits ??= Enumerable.Empty<DtoItemUnit>();
        
        OnItemDelete += OnDelete;
        OnDataGridSelectionChanged += OnDataSelectionChanged;

        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync()
    {
        var units = (await RequestComboModelsAsync<ComboModel>("units")).Data;
        
        var data = Model.DtoItem.ItemUnits.Select(dto =>
        {
            var model = _mapper.Map<DtoItemUnitModel>(dto);

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            return model;
        }).ToList();

        SelectedExpireDate = Model.DtoItem.ExpireDate;
        IsExpirable = Model.DtoItem.ExpireDate != null;
        
        Model.ItemUnitModels.AddRange(data);
        
        await RefreshAsync();
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.ItemUnitModels);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("gategories");

            ComboModelBase.SetComboModelOrDefault(Model.DtoItem.CategoryId);
        }
    }

    protected virtual bool PreSaveChecks()
    {
        if((IsExpirable && Model.DtoItem.ExpireDate is null)
            .Stop("SelectExpireDateTitle", "SelectExpireDateMessage", LocaleService))
            return false;

        return true;
    }
    
    protected override void PreSaveActions()
    {
        Model.DtoItem.CategoryId = ComboModelBase.GetSelectedItemId();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IsExpirable):
            {
                if (!IsExpirable)
                {
                    SelectedExpireDate = null;
                }

                break;
            }
            case nameof(SelectedExpireDate):
                if (SelectedExpireDate != null)
                {
                    IsExpirable = true;
                }

                Model.DtoItem.ExpireDate = SelectedExpireDate;
                break;
        }

        base.OnPropertyChanged(e);
    }

    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;
        
        var model = DataGridServices.FindModel<DtoItemUnitModel>(sender);

        if (model is null)
            return;

        Model.ItemUnitModels = HelperServices
            .RemoveItemFromModelList(Model.ItemUnitModels.AsEnumerable(), model)
            .OfType<DtoItemUnitModel>()
            .ToList();

        _ = RefreshAsync(false);
    }
    
    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        
        PreSaveActions();

        Model.DtoItem.ItemUnits = Enumerable.Empty<DtoItemUnit>();

        foreach (var item in Model.ItemUnitModels)
        {
            Model.DtoItem.ItemUnits = Model.DtoItem.ItemUnits
                .Append(_mapper.Map<DtoItemUnit>(item));
        }
        
        await PostSaveSpecificAsync(Model.DtoItem);
        
        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }

    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoItemUnitModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid())
        };
        
        Model.ItemUnitModels.Add(@new);
        SetData(Model.ItemUnitModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoItemUnitModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoItemUnitModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }
    
    private void Edit(DtoItemUnitModel model)
    {
        SynchroniseTypeBase<ItemsUnitEntryViewModel>(model);
    }

    public void UpdateListing(DtoItemUnitModel model)
    {
        if (!PreSaveChecks(model))
        {
            return;
        }
        
        var list = Model.ItemUnitModels;
        
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

        Model.ItemUnitModels = list;
        SetData(Model.ItemUnitModels);
        RefreshDataGrid();
        SynchroniseTypeBase<ItemsUnitEntryViewModel>(CodeBook.ResState);
    }
    
    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is CodeBook.RefData)
        {
            RefreshDataGrid();
        }
        else if (args.SingleOrDefault() is DtoItemUnitModel model)
        {
            UpdateListing(model);
        }
        else
        {
            await RefreshAsync(false);
        }
    }
    
    private bool PreSaveChecks(DtoItemUnitModel model)
    {
        var units = Model.ItemUnitModels.Where(u => u.TemporaryId != model?.TemporaryId);
        
        if (units.Any(i => i.UnitId == model?.UnitId)
            .Stop("AnotherItemUnitExistsTitle",
                "AnotherItemUnitExistsMessage",
                LocaleService))
            return false;
        
        return !(units.Any(i => i.IsPrimary) && model.IsPrimary)
            .Stop("AnotherPrimaryItemUnitExistsTitle",
                "AnotherPrimaryItemUnitExistsMessage",
                LocaleService);
    }
}