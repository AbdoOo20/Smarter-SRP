using Fly.Shared.Modules.Accounting.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.AccountingModels;

public partial class DtoEntryModel: ObservableObject
{
    [ObservableProperty]
    private DtoEntry? _dtoEntry;
    
    public List<DtoEntryDetailModel> EntryDetailModels { get; set; } = [];
}