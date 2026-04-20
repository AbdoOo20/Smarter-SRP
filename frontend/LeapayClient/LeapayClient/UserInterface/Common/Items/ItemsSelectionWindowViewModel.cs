using System.Collections.ObjectModel;
using Avalonia.Threading;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using Leapay.Client.UserInterface.StoresAndCategories.Items;
using Mapster;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Common.Items;

public partial class ItemsSelectionWindowViewModel : BaseRefreshablePageViewModel<DtoBase>
{
    public ItemsSelectionWindow? Window { get; set; }
    
    private Identifier? _customStorageId;

    [ObservableProperty]
    public partial string? ActionText { get; set; }
    
    [ObservableProperty]
    public partial string? SearchText { get; set; }
    
    [ObservableProperty]
    public partial ComboModel? SelectedItem { get; set; }
    
    [ObservableProperty]
    public partial bool AddVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool ShowZero { get; set; }
    
    [ObservableProperty]
    public partial bool PriceVisibility { get; set; }
    
    private const string GlobalEndpoint = "Item";
    
    [ObservableProperty]
    public partial ObservableCollection<ItemsSelectionModel> Models { get; set; } = [];

    private readonly string? _endpoint;
    
    private readonly string? _itemEndpoint;
    
    public event EventHandler? ItemsFetched;
    
    [ObservableProperty]
    public partial SuperComboVm<DtoBase> Storages { get; set; } = new();
    
    public TaskCompletionSource<(bool Success, List<ItemsSelectionModel> item)> SelectionTaskSource { get; } = new();
    
    public ItemsSelectionWindowViewModel(
        Identifier? customStorageId,
        bool hidePrice)
    {
        _customStorageId = customStorageId;

        _endpoint = GlobalEndpoint;
        _itemEndpoint = "ItemsList";

        PriceVisibility = !hidePrice;
        AddVisibility = Session.JudgeSpecificAccess("Item.Create");
        
        PrepareAsync();
    }
    
    private async void PrepareAsync()
    {
        SetRefreshState(true);

        Endpoint = _endpoint;
        var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
        var defStore = GetDefaultStorage(stores, _customStorageId.GetValueOrDefault(),
            _customStorageId.IsNullOrEmpty() ? EntryType.Add : EntryType.Edit);
        Storages.Load(stores, defStore, async void (_) => { await RefreshUiAsync(true); });
        
        await RefreshUiAsync(false);

        SetRefreshState(false);
    }
    
    private async Task RefreshUiAsync(bool reapply)
    {
        var req = await RequestSpecificByQueryAsync<ItemsSaleQueryResponse>(new ItemsSaleQueryRequest
        {
            StorageId = Storages.SelectedItem?.Id
        }, "ItemsList");

        if (req is { Status: true, Data: not null })
        {
            var models = req.Data.Items
                .Select(i =>
                {
                    var vm = i.Adapt<ItemsSelectionModel>();
                    vm.ImageLink = ContentHelper.GetPhotoLink(i.ItemId.GetValueOrDefault(), i.ItemUpdateCounter);
                    vm.SelectedUnit = vm.Units.FirstOrDefault(v => v.IsPrimary);
                    
                    return vm;
                }).ToList();

            if (reapply)
            {
                Reapply(models);
            }
            
            models.Observe(Models);
        }
        
        OnSearch(string.Empty);
        
        ItemsFetched?.Invoke(this, EventArgs.Empty);
    }

    private void Reapply(List<ItemsSelectionModel> models)
    {
        foreach (var model in models)
        {
            var oldModel = Models.FirstOrDefault(i => i.ItemId == model.ItemId);

            if (oldModel is null)
                continue;

            model.IsSelected = oldModel.IsSelected;
            model.SelectedUnit = model.Units.FirstOrDefault(u 
                => u.UnitId == oldModel.SelectedUnit?.UnitId);
            model.Quantity = oldModel.Quantity;
            model.ItemCustomPrice = oldModel.ItemCustomPrice;
        }
    }
    
    /*public async Task SetItemsByCategoryAsync()
    {
        if (string.IsNullOrEmpty(Categories.SelectedItem?.Value) 
            || Identifier.Parse(Categories.SelectedItem?.Value) == Identifier.Empty)
        {
            await RefreshUiAsync();
            return;
        }
        
        var selectedCat = Categories.SelectedItem?.Value;


        OnSearch(string.Empty);
        
        ItemsFetched?.Invoke(this, EventArgs.Empty);
    }*/

    public async Task<(bool result, List<ItemsSelectionModel>? model)> WaitForUserToSelectAsync()
    {
        var result = await SelectionTaskSource.Task;

        if (result.Success)
        {
            return (true, result.item);
        }
        
        return (false, null);
    }
    
    public void OnSearch(string text)
    {
        //SetRefreshState(true);
        var temp = Models.ToList();

        Task.Run(() =>
        {
            temp = string.IsNullOrEmpty(text) ? temp.OrderByDescending(m => m.SelectedUnit?.QuantityInStorage).ToList() :
                temp.OrderByDescending(s => s.ItemName?.Simplify().Contains(text.Simplify()))
                    .ThenBy(m => m.SelectedUnit?.QuantityInStorage)
                    .ThenByDescending(m => m.IsSelected)
                    .ToList();
        
            Debug("Finished ordering items list");
        }).ContinueWith(_ =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                temp.Observe(Models);
                Debug("Finished population for items list");
                
                //SetRefreshState(false);
            });
        });
    }

    partial void OnShowZeroChanged(bool value)
    {
        foreach (var a in Models)
        {
            a.ShowZero = value;
        }
        
        Debug("Finished setting zero visibility for items list: {0}", value);
    }

    [RelayCommand]
    private void OnClearSelection()
    {
        SelectedItem = null;
        SelectionTaskSource.TrySetResult((true, null));
    }
    
    [RelayCommand]
    private void OnSelect()
    {
        if (!Models.Any(s => s.IsSelected).Stop("ValidationFailed", "PleaseSelectAtLeastOneItem"))
            return;
        
        SelectionTaskSource.TrySetResult((true, Models.Where(m => m is { IsSelected: true, IsVisible: true })
            .ToList()));
        ContentHelper.CloseWindow<ItemsSelectionWindow>();
    }
    
    [RelayCommand]
    private void OnToggleSelection(ItemsSelectionModel? model)
    {
        if (model is null)
            return;

        model.IsSelected = !model.IsSelected;
    }
    
    [RelayCommand]
    private void OnClose()
    {
        SelectionTaskSource.TrySetResult((false, null));
        ContentHelper.CloseWindow<ItemsSelectionWindow>();
    }
    
    [RelayCommand]
    private async Task OnNewAsync()
    {
        var locale = "ItemsPageViewModelAdd";
        var itemEntry = new ItemEntryWindow(new(new(),
            EntryType.Add, GetType()));
        
        var windowTask = ContentHelper.ShowCrossPlatformContentAsync(itemEntry,
            new(this[locale], true, true, false, true));

        var idTask = itemEntry.ViewModel.WaitUntilUserSavesAsync();

        await Task.WhenAny(windowTask, idTask);

        if (!idTask.IsCompletedSuccessfully)
            return;

        var id = idTask.Result;
        
        if (!id.IsNullOrEmpty())
        {
            await RefreshUiAsync(true);
            var item = Models.FirstOrDefault(m => m.ItemId == id);

            if (item is null)
                return;
            
            var index = Models.IndexOf(item);
            Models[index].IsSelected = true;
            
            OnSearch(item.ItemName);
        }
    }
}