using System.Globalization;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class BoolAndConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count == 0)
            return false;

        var param = parameter as string ?? "";
        // Default: all AND
        int orCount = 0;
        bool orLastN = false;

        // Parse something like "^2|"
        // '^' + number + operator (| or &)
        // We'll parse simple format: "^2|" means last 2 OR
        if (param.StartsWith("^") && param.Length >= 3)
        {
            if (int.TryParse(param.Substring(1, param.Length - 2), out int n))
            {
                orCount = n;
                orLastN = param[^1] == '|'; // last char is '|' means OR
            }
        }

        // Sanitise orCount
        orCount = Math.Min(orCount, values.Count);

        // Convert all values to bool, ignore invalids (treat as false)
        var bools = values.Select(v => v is bool b && b).ToList();

        bool result = true;
        int limit = values.Count - orCount;
        for (int i = 0; i < limit; i++)
            result = result && bools[i];

        if (orCount > 0)
        {
            // Combine last orCount with OR or AND based on orLastN flag
            // todo: handle first group
            bool lastGroup;
            if (orLastN)
                lastGroup = bools.Skip(limit).Aggregate(false, (acc, b) => acc || b);
            else
                lastGroup = bools.Skip(limit).Aggregate(true, (acc, b) => acc && b);

            result = result && lastGroup;
        }

        return result;
    }

    public IList<object?> ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}