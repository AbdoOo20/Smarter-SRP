using System.Globalization;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class ReverseBoolConverter : IValueConverter
{
    public object? Convert(object values, Type targetType, object? parameter, CultureInfo culture)
    {
        return !(bool)values;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value;
}