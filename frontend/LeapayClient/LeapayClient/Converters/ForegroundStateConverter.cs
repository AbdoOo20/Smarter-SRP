using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Leapay.Client.Converters;

public class ForegroundStateConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool val)
            return Brushes.Transparent;
        
        return val ? Brushes.LawnGreen : Brushes.IndianRed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}