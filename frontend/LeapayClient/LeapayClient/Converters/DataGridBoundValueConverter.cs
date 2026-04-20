using System.Globalization;
using Avalonia.Data.Converters;
using Leapay.Client.Services;

namespace Leapay.Client.Converters;

public class DataGridBoundValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var locale = App.GetService<LocalisationService>();
        var propName = parameter as string;
        
        return value switch
        {
            bool toggle => locale[toggle.ToString()],
            Enum e => locale[e.ToString()],
            DateTime date => propName is "ExpireDate" ? date.ToString("d") : date,
            _ => value 
        } ?? "" /*(parameter is "-" ? "-" : " ")*/;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ?? " ";
    }
}
