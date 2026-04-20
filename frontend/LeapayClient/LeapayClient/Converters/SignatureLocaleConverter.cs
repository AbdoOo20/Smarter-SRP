using System.Globalization;
using Avalonia.Data.Converters;
using Leapay.Client.Services;
using SmartErpWin.Core;

namespace Leapay.Client.Converters;

public class SignatureLocaleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string loc)
        {
            return value;
        }

        if (!loc.StartsWith(TranslateSignature))
            return loc;

        var actualString = loc[TranslateSignature.Length..];
        var localeService = App.GetService<LocalisationService>();
        
        return localeService[actualString];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
