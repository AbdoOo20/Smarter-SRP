using Avalonia.Media;
using Avalonia.Styling;

namespace Leapay.Client.Styling.Dynamic;

public class GlobalStyles
{
    public static void Apply()
    {
        ApplyDataGridStyles();
    }

    private static void ApplyDataGridStyles()
    {
        var selectedRowStyle = new Style(x => x
            .OfType<DataGridRow>()
            .Class(":selected"))
        {
            Setters =
            {
                new Setter(Border.BackgroundProperty, Brushes.LightSlateGray)
            }
        };

        Application.Current?.Styles.Add(selectedRowStyle);
    }
}