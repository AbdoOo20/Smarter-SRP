namespace SmartiErpWin.Client.Prefabs;

public partial class RefreshPrefab
{
    public RefreshPrefab()
    {
        InitializeComponent();
    }
    
    public Visibility UiVisibility
    {
        get => (Visibility)GetValue(UiVisibilityProperty);
        set => SetValue(UiVisibilityProperty, value);
    }

    public static readonly DependencyProperty UiVisibilityProperty =
        DependencyProperty.Register(
            nameof(UiVisibility),
            typeof(Visibility),
            typeof(RefreshPrefab),
            new PropertyMetadata(Visibility.Visible));

    public Visibility RingVisibility
    {
        get => (Visibility)GetValue(RingVisibilityProperty);
        set => SetValue(RingVisibilityProperty, value);
    }

    public static readonly DependencyProperty RingVisibilityProperty =
        DependencyProperty.Register(
            nameof(RingVisibility),
            typeof(Visibility),
            typeof(RefreshPrefab),
            new PropertyMetadata(Visibility.Visible));

    public string? LoadingInfo
    {
        get => (string?)GetValue(LoadingInfoProperty);
        set => SetValue(LoadingInfoProperty, value);
    }

    public static readonly DependencyProperty LoadingInfoProperty =
        DependencyProperty.Register(
            nameof(LoadingInfo),
            typeof(string),
            typeof(RefreshPrefab),
            new PropertyMetadata(null));

    public double LoadingTitleSize
    {
        get => (double)GetValue(LoadingTitleSizeProperty);
        set => SetValue(LoadingTitleSizeProperty, value);
    }

    public static readonly DependencyProperty LoadingTitleSizeProperty =
        DependencyProperty.Register(
            nameof(LoadingTitleSize),
            typeof(double),
            typeof(RefreshPrefab),
            new PropertyMetadata(26d));
    
    public string? LoadingTitle
    {
        get => (string?)GetValue(LoadingTitleProperty);
        set => SetValue(LoadingTitleProperty, value);
    }

    public static readonly DependencyProperty LoadingTitleProperty =
        DependencyProperty.Register(
            nameof(LoadingTitle),
            typeof(string),
            typeof(RefreshPrefab),
            new PropertyMetadata(null));

    public bool IsRefreshing
    {
        get => (bool)GetValue(IsRefreshingProperty);
        set => SetValue(IsRefreshingProperty, value);
    }

    public static readonly DependencyProperty IsRefreshingProperty =
        DependencyProperty.Register(
            nameof(IsRefreshing),
            typeof(bool),
            typeof(RefreshPrefab),
            new PropertyMetadata(false));
}