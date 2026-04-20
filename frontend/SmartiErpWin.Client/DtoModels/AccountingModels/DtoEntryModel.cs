using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.DtoModels.AccountingModels;

public partial class DtoEntryModel: ObservableObject
{
    [ObservableProperty]
    private DtoEntry? _dtoEntry;
    
    public List<DtoEntryDetailModel> EntryDetailModels { get; set; } = new();
}