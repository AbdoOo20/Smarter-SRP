using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.ValueTypes;

namespace Fly.Shared.Modules.Global.Requests;

public class DynamicReports
{ 
    public ICollection<DynamicText>? Uppers {get; set; }
    
    public ICollection<DynamicText>? Lowers {get; set; }
    
    public ICollection<DynamicRow>? Data {get; set;}
    
    public Dictionary<string, string> CustomData { get; set; } = new();
}

public struct DynamicText
{
    public string? Title {get; set; }
    
    public string? Value {get; set; }
}

public struct DynamicRow
{
    public ICollection<string>? Row { get; set; }
}

public record DynamicField(string Title, string QueryName, object? Value = null);

public record DynamicCombo(string Title, string DataEndpoint, string QueryName, bool IsSearchable = false, double ComboWidth = 300)
{
    public ObservableCollection<LookupOf<string, string>> Combos { get; set; } = [];
    
    public LookupOf<string, string>? SelectedCombo { get; set; }

    public int Index { get; set; }
}