namespace Leapay.Client.ClientModels.Dto;

public partial class ItemImportErrorModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Header { get; set; }

    [ObservableProperty]
    public partial string? Error { get; set; }
}