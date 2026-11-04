using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class TextBoxConverter : IValueConverter
{
    // Only a decimal point and a sign are allowed: no thousands separators, so a
    // typed "." can never be read as a group separator.
    private const NumberStyles DecimalInput =
        NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite |
        NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            decimal dec => dec is 0m ? string.Empty : dec.ToString(CultureInfo.InvariantCulture),
            int integer => integer is 0 ? string.Empty : integer.ToString(CultureInfo.InvariantCulture),
            double dbl => dbl is 0d ? string.Empty : dbl.ToString(CultureInfo.InvariantCulture),
            _ => value?.ToString() ?? string.Empty
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string strValue)
            return value ?? string.Empty;

        // Accept "." and also "," or the Arabic decimal separator "٫" as the decimal point.
        var text = strValue.Trim().Replace('٫', '.').Replace(',', '.');

        if (text.Length == 0)
            return 0m;

        // While the user is still typing ("12." or "-"), keep the text as it is
        // instead of pushing a rounded value back into the box.
        if (text.EndsWith('.') || text == "-")
            return BindingOperations.DoNothing;

        // Parse with the invariant culture, matching how Convert displays the value.
        // On Libyan Windows (ar-LY) the current culture reads "." as a thousands
        // separator, which turned 12.5 into 125.
        if (decimal.TryParse(text, DecimalInput, CultureInfo.InvariantCulture, out var result))
            return result;

        return BindingOperations.DoNothing;
    }
}
