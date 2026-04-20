using Fly.Framework.Shared.Common.ValueTypes;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Interfaces;

namespace SmartErpWin.Core.Helpers;

public abstract class SearchHelper
{
    public static List<T> Search<T>(
        string? query,
        List<T> list)
        where T: ISearchable
    {
        if (string.IsNullOrEmpty(query))
            return list;
        
        query = query.Simplify();

        var queries = list
            .Select(q => q.Display
                .Simplify());
        
        var score = queries
            .Where(q => q
                .Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        return list
            .Where(l => score
                .Any(s => s == l.Display.Simplify()))
            .ToList();
    }
    
    public static List<T> SearchLookup<T>(
        string? query,
        List<T> list)
        where T: LookupOf<string, string>
    {
        if (string.IsNullOrEmpty(query))
            return list;
        
        query = query.Simplify();

        var queries = list
            .Select(q => q.Display
                .Simplify());
        
        var score = queries
            .Where(q => q
                .Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        return list
            .Where(l => score
                .Any(s => s == l.Display.Simplify()))
            .ToList();
    }
}