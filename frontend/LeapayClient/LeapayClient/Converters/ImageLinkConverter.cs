using System.Globalization;
using Avalonia.Data.Converters;
using Leapay.Client.Helpers;

namespace Leapay.Client.Converters;

public class ImageLinkConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string b) return value;
        return ContentHelper.GetDirectImageLink(b);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}