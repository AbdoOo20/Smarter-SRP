using Fly.Framework.Shared.Common;

namespace Leapay.Client.ClientModels.Dto.DtoModels.FastSales;

public partial class ItemModel: ObservableObject
{
    [ObservableProperty]
    public partial Identifier Id { get; set; }
    
    [ObservableProperty]
    public partial string? Name { get; set; }
    
    [ObservableProperty]
    public partial decimal Price { get; set; }
    
    [ObservableProperty]
    public partial string? Photo { get; set; }

    [ObservableProperty]
    public partial byte[] PhotoData { get; set; } = [];
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FavouriteContextCommandName))]
    public partial bool IsFavourited { get; set; }

    public string FavouriteContextCommandName => IsFavourited ? "RemoveFromFavourites" : "AddToFavourites";
}