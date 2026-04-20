using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

public partial class DtoSupplyPermitModel: ObservableObject
{
    [ObservableProperty]
    private DtoSupplyPermit? _dtoSupplyPermit;
    
    public List<DtoSupplyPermitDetailModel> SupplyPermitDetailModels { get; set; } = new();
}