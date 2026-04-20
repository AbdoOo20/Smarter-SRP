using System.Diagnostics.CodeAnalysis;

namespace Fly.Framework.Shared.Common.Helpers;

public class ChildListUpdater<T> where T : class // IEquatable<T>
{
    private readonly IEnumerable<T> _oldList;
    private readonly IEnumerable<T> _newList;

    public ChildListUpdater(IEnumerable<T> oldList, IEnumerable<T> newList)
    {
        _oldList = oldList;
        _newList = newList;
    }

    public (List<T> deleted, List<T> added, List<T> unchanged) GetChanges(Func<T, T, bool> comparer)
    {
        //  TODO: use `comparer`
        var added = new List<T>();
        var deleted = new List<T>();
        var unchanged = new List<T>();
        foreach (var oldItem in _oldList)
        {
            var newItem = _newList.FirstOrDefault(newItem => comparer(oldItem, newItem));
            if (newItem == null)
            {
                deleted.Add(oldItem);
            }
            else
            {
                //if (!unchanged.Contains(newItem, comparer))
                //{
                //}
                unchanged.Add(newItem);
            }
        }

        foreach (var newItem in _newList)
        {
            if (!_oldList.Any(oldItem => comparer(oldItem, newItem)))
            {
                added.Add(newItem);
            }
        }

        //var deleted = _oldList.Except(_newList).ToList();
        //var added = _newList.Except(_oldList).ToList();
        //var unchanged = _oldList.Intersect(_newList).ToList();

        return (deleted, added, unchanged);
    }
}

public class Class1 : IEqualityComparer<Class1>
{
    public bool Equals(Class1? x, Class1? y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode([DisallowNull] Class1 obj)
    {
        throw new NotImplementedException();
    }
}