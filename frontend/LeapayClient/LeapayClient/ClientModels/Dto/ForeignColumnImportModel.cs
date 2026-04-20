using System.Collections.ObjectModel;

namespace Leapay.Client.ClientModels.Dto;

public partial class ForeignColumnImportModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Header { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<ColumnImportModel> ForeignColumns { get; set; } = [];
    
    [ObservableProperty]
    public partial string? State { get; set; }
}