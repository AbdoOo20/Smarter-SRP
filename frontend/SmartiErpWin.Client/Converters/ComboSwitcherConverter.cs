

using System.Globalization;
using System.Windows.Data;

namespace SmartiErpWin.Client.Converters;

public class ComboSwitcherConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool bl)
        {
            return value;
        }
        return bl ? Visibility.Collapsed : Visibility.Visible;
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
