using System.Collections.ObjectModel;

namespace SmartErpWin.Core.Extensions;

#pragma warning disable CS1998

public static class CustomLinqExtensions
{
    public static IEnumerable<IEnumerable<T>> Split<T>(
        this IEnumerable<T> source,
        int parts)
    {
        if (parts <= 0)
        {
            parts = 1;
        }
        
        var enumerable = source as T[] ?? source.ToArray();
        var total = enumerable.Length;
        var size = (int)Math.Ceiling((double)total / parts);

        for (var i = 0; i < parts; i++)
        {
            yield return enumerable.Skip(i * size).Take(size);
        }
    }
    
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> input)
    {
        foreach(var value in input)
        {
            yield return value;
        }
    }

    public static void Observe<T>(this IEnumerable<T> collection, ObservableCollection<T> observableCollection)
    {
        observableCollection.Clear();

        foreach (var item in collection)
        {
            observableCollection.Add(item);
        }
    }

    public static void ObserveForEach<T>(this ObservableCollection<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action.Invoke(item);
        }
    }
    
    public static List<TAnother> CastAs<TAnother>(this IEnumerable<object> list)
    {
        return list.Cast<TAnother>().ToList();
    }
}
