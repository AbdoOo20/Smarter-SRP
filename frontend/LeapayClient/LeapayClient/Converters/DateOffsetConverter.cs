using System.Globalization;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class DateOffsetConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime loc)
        {
            return value;
        }

        return new DateTimeOffset(loc);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTimeOffset loc)
        {
            return value;
        }

        return loc.DateTime;
    }
}
