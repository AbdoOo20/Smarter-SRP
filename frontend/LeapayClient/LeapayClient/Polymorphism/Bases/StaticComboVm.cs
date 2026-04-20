using System.Collections.ObjectModel;
using Leapay.Client.Services;

namespace Leapay.Client.Polymorphism.Bases;

public partial class StaticComboVm<TEnum>: ObservableObject where TEnum: struct, Enum
{
    [ObservableProperty]
    public partial ObservableCollection<string> Items { get; set; } = [];

    [ObservableProperty]
    public partial string? SelectedItem { get; set; }
    
    public Dictionary<int, string?> ConversionKit { get; set; } = new();
    
    public Action<TEnum>? OnSelectionChanged { get; set; }
    
    public void Set(TEnum? def = null,
        Action<TEnum>? onSelectionChanged = null)
    {
        Information("Setting static combo items with {0}", typeof(TEnum).Name);
        
        Items.Clear();

        var locale = App.GetService<LocalisationService>();
        
        ConversionKit = Enum.GetValues(typeof(TEnum)).Cast<object?>()
            .ToDictionary(Convert.ToInt32, item => locale[item.ToString()].ToString());

        var localisedValues = ConversionKit.Values.ToArray();
                
        OnSelectionChanged = onSelectionChanged;
        
        foreach (var value in localisedValues)
        {
            Items.Add(value);
            
            if (value == locale[def.GetValueOrDefault().ToString()])
                SelectedItem = value;
        }
    }

    partial void OnSelectedItemChanged(string? value)
    {
        OnSelectionChanged?.Invoke(ConvertToEnum(value));
    }

    public TEnum GetSelectedItem() => ConvertToEnum(SelectedItem);
    
    public bool Is(TEnum value) => GetSelectedItem().Equals(value); 
    
    private TEnum ConvertToEnum(string? display)
    {
        var cast = ConversionKit.Count == 0 ? 0 : ConversionKit.Keys
            .FirstOrDefault(k => ConversionKit[k] == display);

        if (!Enum.IsDefined(typeof(TEnum), cast))
            return default;

        var type = (TEnum)(object)cast;
        return type;
    }
    
    public void Reset()
    {
        Items.Clear();
        SelectedItem = null;
    }
}