namespace SmartiErpWin.Client.Prefabs;

public partial class EntryPrefab
{
    public static readonly DependencyProperty TitleTextProperty =
        DependencyProperty.Register(nameof(TitleText),
            typeof(string),
            typeof(EntryPrefab),
            new PropertyMetadata(string.Empty));

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }
    
    public static readonly DependencyProperty EntryTextProperty =
        DependencyProperty.Register(
            nameof(EntryText),
            typeof(string),
            typeof(EntryPrefab),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string EntryText
    {
        get => (string)GetValue(EntryTextProperty);
        set => SetValue(EntryTextProperty, value);
    }
    
    public EntryPrefab()
    {
        InitializeComponent();
    }
}