using Material.Icons.Avalonia;

namespace Leapay.Client.Features;

public interface INavigationPage
{
    public string DisplayName { get; set; }

    public MaterialIcon Icon { get; set; }
    
    public int Index { get; set; }
}