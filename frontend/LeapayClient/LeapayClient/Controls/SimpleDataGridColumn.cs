using Avalonia.Controls.Templates;

namespace Leapay.Client.Controls;

public class SimpleDataGridColumn
{
    public object? Header { get; set; }
    
    public string? HeaderId { get; set; }
    
    public IDataTemplate? CellTemplate { get; set; }
    
    public IEnumerable<Control> ConcreteControls { get; set; }
    
    public GridLength Width { get; set; } = GridLength.Auto;
    
    public bool IsButtonColumn { get; set; }
}