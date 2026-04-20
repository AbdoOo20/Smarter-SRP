using System.Globalization;
using Avalonia.Data.Converters;

namespace SukiUI.Converters;

public class IsNotNullOrEmptyToDouble : IValueConverter
{
    public static readonly IsNotNullOrEmptyToDouble Instance = new IsNotNullOrEmptyToDouble();
        
        
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string)
            return !string.IsNullOrEmpty((string)value) ? 1 : 0;

        return 1;
    }
        
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}