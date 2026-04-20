

using System.Globalization;
using System.Windows.Data;

namespace SmartiErpWin.Client.Converters;

public class TextBoxConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            decimal dec => dec is 0m ? string.Empty : dec.ToString(CultureInfo.InvariantCulture),
            int integer => integer is 0 ? string.Empty : integer.ToString(CultureInfo.InvariantCulture),
            double dbl => dbl is 0d ? string.Empty : dbl.ToString(/*CultureInfo.InvariantCulture*/),
            _ => value?.ToString() ?? string.Empty
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string strValue)
            return value ?? string.Empty;

        if (strValue.Contains(".") && strValue.Split('.').Length == 2 && strValue[strValue.Length - 1] == '.')
        {
            strValue += "0";
        }

        return decimal.TryParse(strValue, out var result) ? result : value;
    }
}
