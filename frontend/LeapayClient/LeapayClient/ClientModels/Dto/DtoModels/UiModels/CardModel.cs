using SukiUI.Controls;

namespace Leapay.Client.ClientModels.Dto.DtoModels.UiModels;

public partial class CardModel: ObservableObject
{
    [ObservableProperty]
    public partial string? CardName { get; set; }

    [ObservableProperty]
    public partial MaterialIconKind CardSymbol { get; set; } = MaterialIconKind.Card;

    [ObservableProperty]
    public partial SukiSideMenuItem? Item { get; set; }
    
    [ObservableProperty]
    public partial bool IsVisible { get; set; }
    
    public required Action OnClick;
    
    [RelayCommand]
    private void OnAction()
    {
        OnClick.Invoke();
    }
}