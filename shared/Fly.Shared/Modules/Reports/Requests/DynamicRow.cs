namespace Fly.Shared.Modules.Reports.Requests;

public class DynamicApiRow
{
    public ICollection<string> Row { get; set; } = [];

    public DynamicApiRow()
    {
        Row = [];
    }

    public DynamicApiRow(ICollection<string> columns)
    {
        Row = columns;
    }
}
