

using System.Globalization;
using System.Windows.Data;

namespace SmartiErpWin.Client.Converters;

public class SearchableSwitcherConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool bl)
        {
            return value;
        }
        return bl ? Visibility.Visible : Visibility.Collapsed;
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
