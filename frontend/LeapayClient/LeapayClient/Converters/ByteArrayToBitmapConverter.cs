using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Leapay.Client.Helpers;

namespace Leapay.Client.Converters;

public class ByteArrayToBitmapConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is byte[] bytes)
        {
            try
            {
                if (bytes.Length == 0)
                    bytes = DataHelper.GetDefaultProductImage();
                
                return new Bitmap(new MemoryStream(bytes));
            }
            catch
            {
                return null;
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
