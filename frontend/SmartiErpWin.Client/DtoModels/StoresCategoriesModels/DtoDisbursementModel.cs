using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

public partial class DtoDisbursementPermitModel: ObservableObject
{
    [ObservableProperty]
    private DtoDisbursementPermit? _dtoDisbursementPermit;
    
    public List<DtoDisbursementPermitDetailModel> DisbursementPermitDetailModels { get; set; } = new();
}