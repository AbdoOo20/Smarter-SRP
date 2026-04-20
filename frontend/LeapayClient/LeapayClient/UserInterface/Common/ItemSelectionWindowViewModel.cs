using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.StoresAndCategories.ItemCompositions;
using Leapay.Client.UserInterface.StoresAndCategories.Items;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.UserInterface.Common;

public partial class ItemSelectionWindowViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    public ItemSelectionWindow? Window { get; set; }

    [ObservableProperty]
    public partial string? ActionText { get; set; }
    
    [ObservableProperty]
    public partial string? SearchText { get; set; }
    
    [ObservableProperty]
    public partial ComboModel? SelectedItem { get; set; }
    
    [ObservableProperty]
    public partial string? InfoText { get; set; }
    
    [ObservableProperty]
    public partial bool AddVisibility { get; set; }
    
    private const string GlobalEndpoint = "Sales";

    [ObservableProperty]
    public partial ObservableCollection<ComboModel> ModelsInView { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ComboModel> Models { get; set; } = [];

    private string? _endpoint;
    
    private string? _itemEndpoint;
    
    private string? _previousId;
    
    public event EventHandler? ItemsFetched;
    
    [ObservableProperty]
    public partial SuperComboVm<DtoBase> Categories { get; set; } = new();
    
    public TaskCompletionSource<(bool Success, ComboModel? item)> SelectionTaskSource { get; } = new();
    
    public ItemSelectionWindowViewModel(
        DtoBase? model,
        EntryType entryType,
        string previousId,
        string? endpoint,
        string? itemEndpoint = null)
    {
        SetDetails(model, entryType, GetType());

        _endpoint = endpoint;
        _previousId = previousId;
        _itemEndpoint = itemEndpoint ?? "Items";

        var key = _itemEndpoint is "Items" ? "Item" : "ItemComposition";
        AddVisibility = Session.JudgeSpecificAccess($"{key}.Create");
        
        PrepareAsync();
    }
    
    private async void PrepareAsync()
    {
        SetRefreshState(true);

        Endpoint = _endpoint;
        
        await WrapEndpointAsync(GlobalEndpoint,
            async () => await Categories.LoadAsync(this, "Categories", 
                defaultText: this["All"], 
                onSelectionChanged: s => _ = SetItemsByCategoryAsync()));

        await SetAllItemsAsync();

        SetRefreshState(false);
    }
    
    private async Task SetAllItemsAsync()
    {
        var items = await RequestComboModelsAsync<ComboModel>(_itemEndpoint);
        
        items.Data.Observe(Models);
        OnSearch(string.Empty);

        if (_previousId != null)
        {
            SelectedItem = Models.FirstOrDefault(m => m.Value == _previousId);
        }
        
        ItemsFetched?.Invoke(this, EventArgs.Empty);
    }

    public async Task SetItemsByCategoryAsync()
    {
        if (string.IsNullOrEmpty(Categories.SelectedItem?.Value) 
            || Identifier.Parse(Categories.SelectedItem?.Value) == Identifier.Empty)
        {
            await SetAllItemsAsync();
            return;
        }
        
        var selectedCat = Categories.SelectedItem?.Value;

        var items = await RequestComboModelsAsync<ComboModel>(_itemEndpoint, selectedCat);

        items.Data.Observe(Models);
        OnSearch(string.Empty);
        
        ItemsFetched?.Invoke(this, EventArgs.Empty);
    }

    public async Task<(bool result, List<ComboModel>? model)> WaitForUserToSelectAsync()
    {
        var result = await SelectionTaskSource.Task;

        if (result.Success)
        {
            return (true, [result.item]);
        }
        
        return (false, null);
    }
    
    public void OnSearch(string text)
    {
        var search = SearchHelper.Search(text, Models.ToList());

        search.Observe(ModelsInView);
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
        if (SelectedItem is null || !SelectedItem.HasValue()
                                 || string.IsNullOrEmpty(SelectedItem.Display))
            return;
        
        SelectionTaskSource.TrySetResult((true, SelectedItem));
        ContentHelper.CloseWindow<ItemSelectionWindow>();
    }
    
    [RelayCommand]
    private void OnClose()
    {
        SelectionTaskSource.TrySetResult((false, null));
        ContentHelper.CloseWindow<ItemSelectionWindow>();
    }
    
    [RelayCommand]
    private async Task OnNewAsync()
    {
        var isItemComposition = _itemEndpoint is "ItemCompositions";
        var locale = isItemComposition ? "ItemCompositionsPageViewModelAdd" : "ItemsPageViewModelAdd";
        var entry = !isItemComposition ? new ItemEntryWindow(new(new(),
            EntryType.Add, GetType())) : new ItemCompositionEntryWindow(new(new(),
            EntryType.Add, GetType())) as UserControlBase;
        
        var windowTask = ContentHelper.ShowCrossPlatformContentAsync(entry,
            new(this[locale], true, true, false, true));

        var idTask = entry switch
        {
            ItemEntryWindow itemEntry => itemEntry.ViewModel.WaitUntilUserSavesAsync(),
            ItemCompositionEntryWindow itemCompositionEntry => itemCompositionEntry.ViewModel
                .WaitUntilUserSavesAsync(),
            _ => throw new("Unknown ItemEndpoint. Can't select a proper window to show.")
        };

        await Task.WhenAny(windowTask, idTask);

        if (!idTask.IsCompletedSuccessfully)
            return;

        var id = idTask.Result;
        
        if (!id.IsNullOrEmpty())
        {
            await SetAllItemsAsync();
            
            SelectedItem = ModelsInView.FirstOrDefault(m 
                => m.Value == id.GetValueOrNull().ToString());
            
            OnSelect();
            
            SelectionTaskSource.TrySetResult((false, null));
            ContentHelper.CloseWindow<ItemSelectionWindow>();
        }
    }
}