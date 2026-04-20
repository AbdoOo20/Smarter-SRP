using System.Text.Json.Serialization;
using SmartErpWin.Core.Interfaces;

namespace SmartiErpWin.Client.DtoModels;

public partial class ComboModel: ObservableObject, ISearchable
{
    [JsonPropertyName("value")]
    [ObservableProperty]
    private string? _value;

    [JsonPropertyName("display")]
    [ObservableProperty]
    private string? _display;

    [JsonIgnore]
    [ObservableProperty]
    private bool _loading;
}