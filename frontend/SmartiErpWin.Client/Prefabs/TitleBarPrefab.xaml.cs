namespace SmartiErpWin.Client.Prefabs;

public partial class TitleBarPrefab
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(TitleProperty),
            typeof(string),
            typeof(TitleBarPrefab),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public TitleBarPrefab()
    {
        InitializeComponent();
    }
}