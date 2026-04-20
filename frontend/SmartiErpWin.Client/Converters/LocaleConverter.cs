

using System.Globalization;
using System.Windows.Data;

namespace SmartiErpWin.Client.Converters;

public class LocaleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string loc)
        {
            return value;
        }

        var localeService = App.GetService<LocalisationService>();
        var parts = loc.Split('.');
        if (parts.Length  == 2)
        {
            return localeService[parts[1]]  + " " + localeService[parts[0]];
        }
        return localeService[loc];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
