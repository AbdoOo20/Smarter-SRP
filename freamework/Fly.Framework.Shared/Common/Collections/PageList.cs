namespace Fly.Framework.Shared.Common.Collections;

public class PageList<T>
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    public static PageList<T> Create(IEnumerable<T> data, int curremtPage, int? totalCount = null)
    {
        totalCount ??= data.Count();

        return new PageList<T>()
        {
            Items = data,
            TotalCount = totalCount.Value,
            CurrentPage = curremtPage,
        };
    }

    public static PageList<T> Empity(int curremtPage = 1)
    {
        return new PageList<T>()
        {
            Items = Enumerable.Empty<T>(),
            TotalCount = 0,
            CurrentPage = curremtPage,
        };
    }

    public static PageList<T> operator +(PageList<T> left, PageList<T> right)
    {
        var all = new PageList<T>
        {
            CurrentPage = left.CurrentPage,
            Items = left.Items.Concat(right.Items),
            TotalCount = left.TotalCount + right.TotalCount,
        };
        return all;
    }

}
