using System.Globalization;
using Avalonia.Data.Converters;

namespace Leapay.Client.Converters;

public class MathConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return value;

        try
        {
            double number = System.Convert.ToDouble(value);

            // Expect parameter in the form: "operation,operand"
            // Example: "+,10" or "*,2"
            string[] parts = parameter.ToString().Split(',');
            if (parts.Length != 2)
                return value;

            string op = parts[0].Trim();
            double operand = double.Parse(parts[1], CultureInfo.InvariantCulture);

            return op switch
            {
                "+" => number + operand,
                "-" => number - operand,
                "*" => number * operand,
                "/" => operand != 0 ? number / operand : number,
                _ => number
            };
        }
        catch
        {
            return value;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Optionally implement if two-way binding is needed
        throw new NotImplementedException();
    }
}