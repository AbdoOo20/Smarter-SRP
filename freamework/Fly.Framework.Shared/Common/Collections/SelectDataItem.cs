namespace Fly.Framework.Shared.Common.Collections;

public class SelectDataItem<TValue>
{
    public TValue? Id { get; set; }
    public string? Text { get; set; }

    public override string ToString()
    {
        return Text?.ToString() ?? string.Empty;
    }
}