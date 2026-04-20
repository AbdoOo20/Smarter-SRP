using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;

namespace Leapay.Client.ClientModels.Dto.DtoModels.FastSales;

public partial class CategoryModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial Identifier Id { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<ItemModel> Items { get; set; } = [];
}