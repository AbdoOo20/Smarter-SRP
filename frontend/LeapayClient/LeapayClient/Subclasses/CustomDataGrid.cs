using Avalonia.Input;

namespace Leapay.Client.Subclasses;

public class CustomDataGrid: DataGrid
{
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        e.Handled = true;
    }
}