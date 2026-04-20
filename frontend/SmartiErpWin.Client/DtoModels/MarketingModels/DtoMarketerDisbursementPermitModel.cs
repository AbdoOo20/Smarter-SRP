using Fly.Shared.Modules.Marketers.Dtos;

namespace SmartiErpWin.Client.DtoModels.MarketingModels;

public partial class DtoMarketerDisbursementPermitModel: ObservableObject
{
    [ObservableProperty]
    private DtoMarketerDisbursementPermit? _dtoMarketerDisbursementPermit;
    
    public List<DtoMarketerDisbursementPermitDetailsModel> MarketerDisbursementPermitDetailsModels { get; set; } = new();
}