using System.Globalization;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class StringConcatConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var first = values.Count > 0 ? values[0]?.ToString() : string.Empty;
        var second = values.Count > 1 ? values[1]?.ToString() : string.Empty;

        return $"{first} {second}".Trim();
    }
}