using System.Collections.ObjectModel;

namespace Leapay.Client.ClientModels.Dto.DtoModels.UiModels;

public partial class ExpanderModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; set; }
    [ObservableProperty]
    public partial bool Visibility { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<CardModel> Cards { get; set; } = [];
}