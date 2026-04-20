namespace Fly.Framework.Shared.Common.ValueTypes;

public class LookupOf<T, Y>
{
    public T Display { get; set; } = default!;
    public Y Value { get; set; }
}

public class TypeOf<T>
{
    public T Value { get; set; }

    public TypeOf()
    {
        Value = default!;
    }


    public TypeOf(T v)
    {
        Value = v;
    }
}