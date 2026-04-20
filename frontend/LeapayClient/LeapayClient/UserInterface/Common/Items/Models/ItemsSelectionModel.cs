using Fly.Shared.Modules.Store.Requests;

namespace Leapay.Client.UserInterface.Common.Items.Models;

public partial class ItemsSelectionModel: ItemsSaleDetailsQueryResponse
{
    [ObservableProperty]
    public partial bool IsSelected { get; set; }
    
    [ObservableProperty]
    public partial string? NotesText { get; set; }
    
    [ObservableProperty]
    public partial UnitSaleDetailsQueryResponse? SelectedUnit { get; set; }

    [ObservableProperty] 
    public partial int Quantity { get; set; } = 1;
    
    [ObservableProperty]
    public partial string? ImageLink { get; set; }

    [ObservableProperty]
    public partial bool IsVisible { get; set; }

    [ObservableProperty]
    public partial bool ShowZero { get; set; }
    
    partial void OnSelectedUnitChanged(UnitSaleDetailsQueryResponse? value)
    {
        SetIsVisible();
    }

    partial void OnShowZeroChanged(bool value)
    {
        SetIsVisible();
    }

    partial void OnIsSelectedChanged(bool value)
    {
        SetIsVisible();
    }
    
    private void SetIsVisible()
    {
        IsVisible = IsSelected || SelectedUnit?.QuantityInStorage > 0;
    }
}