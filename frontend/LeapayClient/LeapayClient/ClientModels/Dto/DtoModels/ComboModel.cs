using System.Text.Json.Serialization;
using Fly.Framework.Shared.Common;
using SmartErpWin.Core.Interfaces;

namespace Leapay.Client.ClientModels.Dto.DtoModels;

public partial class ComboModel: ObservableObject, ISearchable
{
    [field: JsonPropertyName("value")]
    [ObservableProperty]
    public partial string? Value { get; set; }

    [field: JsonPropertyName("display")]
    [ObservableProperty]
    public partial string? Display { get; set; }

    [field: JsonIgnore]
    [ObservableProperty]
    public partial bool Loading { get; set; }

    public override string ToString()
    {
        return Display ?? "...";
    }
    
    [JsonIgnore]
    public Identifier Id => Identifier.Parse(Value ?? string.Empty);

    public bool HasValue()
    {
        return !string.IsNullOrEmpty(Value) && Identifier.Parse(Value) != Identifier.Empty;
    }
}