namespace Fly.Shared.Modules.Reports.Requests;

public class DynamicApiReports
{
    public ICollection<DynamicText> Uppers { get; set; } = [];
    public ICollection<DynamicText> Lowers { get; set; } = [];
    public ICollection<DynamicApiRow> Data { get; set; } = [];

    public Dictionary<string, string> CustomData { get; set; } = new();
    
    public DynamicApiReports()
    {
        Data = [];
        Lowers = [];
        Uppers = [];
    }

    public void AppendRow(string[] row)
    {
        Data.Add(new DynamicApiRow
        {
            Row = row
        });
    }
    public void AppendRow(DynamicApiRow row)
    {
        Data.Add(row);
    }
    public void AppendRows(IEnumerable<DynamicApiRow> row)
    {
        foreach (var item in row)
        {
            AppendRow(item);
        }
    }

    public void AddUpper(string v, string name)
    {
        Uppers.Add(new DynamicText
        {
            Title = v,
            Value = name
        });
    }
    public void AddLower(string v, string name)
    {
        Lowers.Add(new DynamicText
        {
            Title = v,
            Value = name
        });
    }
}
