using System.Collections.ObjectModel;

namespace SmartiErpWin.Client.DtoModels.UiModels;

public partial class ExpanderModel: ObservableObject
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private Visibility _visibility = Visibility.Collapsed;
    
    [ObservableProperty]
    private ObservableCollection<CardModel> _cards = [];
}