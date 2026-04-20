using System.Globalization;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class BoolOpacityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => (bool)(value ?? true) ? 1.0 : 0.4;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value;
}