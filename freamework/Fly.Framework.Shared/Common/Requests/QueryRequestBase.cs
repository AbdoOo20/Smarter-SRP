namespace Fly.Framework.Shared.Common.Requests;

public class QueryRequestBase
{
    public int? Set { get; set; }
    public int? Page { get; set; } = null;
    public int? Liimit { get; set; } = null;
    public string? SearchText { get; set; } = null;
    public IDictionary<string, object>? Filter { get; set; } = null;
}