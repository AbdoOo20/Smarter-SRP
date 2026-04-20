using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;
using SukiUI.Controls;

namespace Leapay.Client.Wrappers;

public class NavigationViewItemWrapper
{
    public required SukiSideMenuItem Item { get; set; }

    public CardModel? CardModel { get; set; }

    public MaterialIconKind Symbol { get; set; }
    
    public Type? PageType { get; set; }

    public TabbedViewCardModel GetTabCardModel()
    {
        return new TabbedViewCardModel
        {
            CardName = CardModel.CardName,
            CardSymbol = CardModel.CardSymbol,
            View = Item.InnerContent,
            ViewName = Item.Name,
            IsVisible = Item.IsVisible
        };
    }
}