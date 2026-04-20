namespace SmartiErpWin.Client.DtoModels;

public partial class StaticComboModel<V>: ObservableObject
{
    [ObservableProperty]
    private string _display;

    [ObservableProperty]
    private V _type;

    public StaticComboModel(string display, V type)
    {
        Type = type;
        Display = display;
    }
}