using Fly.Shared.Modules.Store.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.StoresCategoriesModels;

public partial class DtoItemModel: ObservableObject
{
    [ObservableProperty]
    private DtoItem? _dtoItem;
    
    public List<DtoItemUnitModel> ItemUnitModels { get; set; } = new();
}