using Avalonia.Controls.Notifications;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.UserInterface.Common.Items.Models;
using SukiUI.Controls;

namespace Leapay.Client.Polymorphism.Bases;

public partial class BaseItemPageEntryViewModel<T, TD> : BasePageEntryViewModel<T, TD>
    where TD: class, new()
    where T: class, new()
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyItemSelected))]
    public partial ComboModel? SelectedItem { get; set; }

    [ObservableProperty]
    public partial List<ItemsSelectionModel> SelectedItems { get; set; } = [];
    
    [ObservableProperty]
    public partial ComboModel? SelectedItemComposition { get; set; }
    
    [ObservableProperty]
    public partial string? SelectedItemStatus { get; set; }
    
    [ObservableProperty]
    public partial string? SelectedItemCompositionStatus { get; set; }
    
    public bool IsSelectionWindowOpen { get; set; }

    protected virtual bool HideItemsSelectionPrice { get; set; }

    public bool IsCompositionSelectionWindowOpen { get; set; }

    public bool IsAnyItemSelected => SelectedItem is not null;
    
    public InfoBar? ItemInfoBar
    {
        get;
        set
        {
            field = value;
            RefreshSelectedItemStatus();
        }
    }
    
    public InfoBar? ItemCompositionInfoBar
    {
        get;
        set
        {
            field = value;
            RefreshSelectedItemCompositionStatus();
        }
    }
    
    protected void RefreshSelectedItemStatus()
    {
        if (ItemInfoBar is null)
            return;
        
        if (SelectedItem is null || !SelectedItem.HasValue())
        {
            ItemInfoBar.Severity = NotificationType.Information;
            SelectedItemStatus = this["PleaseSelectAnItem"];
        }
        else
        {
            ItemInfoBar.Severity = NotificationType.Success;
            SelectedItemStatus = string.Format(this["TheFollowingItemIsSelected"],
                SelectedItem.Display ?? this["ItemWithNoName"]);
        }
    }
    
    protected void RefreshSelectedItemCompositionStatus()
    {
        if (ItemCompositionInfoBar is null)
            return;
        
        if (SelectedItemComposition is null || !SelectedItemComposition.HasValue())
        {
            ItemCompositionInfoBar.Severity = NotificationType.Information;
            SelectedItemCompositionStatus = this["PleaseSelectAComposition"];
        }
        else
        {
            ItemCompositionInfoBar.Severity = NotificationType.Success;
            SelectedItemCompositionStatus = string.Format(this["TheFollowingItemCompositionIsSelected"],
                SelectedItemComposition.Display ?? this["ItemWithNoName"]);
        }
    }
    
    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task OnSelectItemAsync(string? previousId = null)
    {
        if (IsSelectionWindowOpen)
            return;
        
        previousId ??= SelectedItem?.Value;
        
        var window = await ContentHelper.OpenItemSelectionWindowAsync(previousId, Endpoint);
        
        IsSelectionWindowOpen = true;

        if (!window.result)
        {
            IsSelectionWindowOpen = false;
            return;
        }
        
        IsSelectionWindowOpen = false;

        SelectedItem = window.item.FirstOrDefault();
        ContentHelper.ExecuteLastOpen(w => w.Activate());
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task OnSelectItemsAsync()
    {
        if (IsSelectionWindowOpen)
            return;
        
        var window = await ContentHelper.OpenItemsSelectionWindowAsync(null, HideItemsSelectionPrice);
        
        IsSelectionWindowOpen = true;

        if (!window.result)
        {
            IsSelectionWindowOpen = false;
            return;
        }
        
        IsSelectionWindowOpen = false;

        SelectedItems = window.items;
        OnAfterItemsAreSelected();
    }
    
    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task OnSelectItemCompositionAsync(string? previousId = null)
    {
        if (IsCompositionSelectionWindowOpen)
            return;
        
        previousId ??= SelectedItemComposition?.Value;
        
        var window = await ContentHelper.OpenItemSelectionWindowAsync(previousId,
            Endpoint,
            "ItemCompositions",
            "SelectItemComposition");
        
        IsCompositionSelectionWindowOpen = true;

        if (!window.result)
        {
            IsCompositionSelectionWindowOpen = false;
            return;
        }
        
        IsCompositionSelectionWindowOpen = false;

        SelectedItemComposition = window.item.FirstOrDefault();
        ContentHelper.ExecuteLastOpen(w => w.Activate());
    }
    
    partial void OnSelectedItemChanged(ComboModel? value)
    {
        OnAfterItemIsSelected();
    }

    partial void OnSelectedItemCompositionChanged(ComboModel? value)
    {
        OnAfterItemCompositionIsSelected();
    }
    
    protected virtual void OnAfterItemCompositionIsSelected() { }
    
    protected virtual void OnAfterItemIsSelected() { }
    
    protected virtual void OnAfterItemsAreSelected() { }
}