namespace SmartiErpWin.Client.Helpers;

public static class UiExtensions
{
    public static Visibility ApplyVisibility(this bool condition, bool hidden = false)
    {
        return condition ? Visibility.Visible : hidden ? Visibility.Hidden : Visibility.Collapsed;
    }
}