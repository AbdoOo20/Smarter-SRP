using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.DtoModels.UiModels;

public partial class CardModel: ObservableObject
{
    [ObservableProperty]
    private string? _cardName;

    [ObservableProperty]
    private SymbolRegular _cardSymbol = SymbolRegular.CardUi20;

    [ObservableProperty]
    private Visibility _isVisible;
    
    public required Action OnClick;

    [RelayCommand]
    private void OnAction()
    {
        OnClick.Invoke();
    }
}