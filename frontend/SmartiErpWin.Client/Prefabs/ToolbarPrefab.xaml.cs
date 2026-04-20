using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartiErpWin.Client.Prefabs;

public partial class ToolbarPrefab : UserControl
{
    public ToolbarPrefab()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty SearchBarProperty =
        DependencyProperty.Register(nameof(SearchBar),
            typeof(string), typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public string SearchBar
    {
        get => (string)GetValue(SearchBarProperty);
        set => SetValue(SearchBarProperty, value);
    }

    public static readonly DependencyProperty SearchTextProperty =
        DependencyProperty.Register(nameof(SearchText),
            typeof(string), typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    public static readonly DependencyProperty PrintTextProperty =
        DependencyProperty.Register(nameof(PrintText),
            typeof(string), typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public string PrintText
    {
        get => (string)GetValue(PrintTextProperty);
        set => SetValue(PrintTextProperty, value);
    }
    
    public static readonly DependencyProperty ExtraTextProperty =
        DependencyProperty.Register(nameof(ExtraText),
            typeof(string), typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public string ExtraText
    {
        get => (string)GetValue(ExtraTextProperty);
        set => SetValue(ExtraTextProperty, value);
    }
    
    public static readonly DependencyProperty ActionsEnabledProperty =
        DependencyProperty.Register(nameof(ActionsEnabled),
            typeof(bool),
            typeof(ToolbarPrefab),
            new PropertyMetadata(false));

    public bool ActionsEnabled
    {
        get => (bool)GetValue(ActionsEnabledProperty);
        set => SetValue(ActionsEnabledProperty, value);
    }

    public static readonly DependencyProperty RefreshCommandProperty =
        DependencyProperty.Register(nameof(RefreshCommand),
            typeof(ICommand),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public ICommand RefreshCommand
    {
        get => (ICommand)GetValue(RefreshCommandProperty);
        set => SetValue(RefreshCommandProperty, value);
    }

    public static readonly DependencyProperty PrintCommandProperty =
        DependencyProperty.Register(nameof(PrintCommand),
            typeof(ICommand),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public ICommand PrintCommand
    {
        get => (ICommand)GetValue(PrintCommandProperty);
        set => SetValue(PrintCommandProperty, value);
    }
    
    public static readonly DependencyProperty ExtraCommandProperty =
        DependencyProperty.Register(nameof(ExtraCommand),
            typeof(ICommand),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public ICommand ExtraCommand
    {
        get => (ICommand)GetValue(ExtraCommandProperty);
        set => SetValue(ExtraCommandProperty, value);
    }
    
    public static readonly DependencyProperty AddCommandProperty =
        DependencyProperty.Register(nameof(AddCommand),
            typeof(ICommand),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public ICommand AddCommand
    {
        get => (ICommand)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }

    public static readonly DependencyProperty PrimaryButtonColourProperty =
        DependencyProperty.Register(nameof(PrimaryButtonColour),
            typeof(Brush),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public Brush PrimaryButtonColour
    {
        get => (Brush)GetValue(PrimaryButtonColourProperty);
        set => SetValue(PrimaryButtonColourProperty, value);
    }

    public static readonly DependencyProperty SecondaryButtonColourProperty =
        DependencyProperty.Register(nameof(SecondaryButtonColour),
            typeof(Brush),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public Brush SecondaryButtonColour
    {
        get => (Brush)GetValue(SecondaryButtonColourProperty);
        set => SetValue(SecondaryButtonColourProperty, value);
    }

    public static readonly DependencyProperty SearchButtonProperty =
        DependencyProperty.Register(nameof(SearchButton),
            typeof(string),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public string SearchButton
    {
        get => (string)GetValue(SearchButtonProperty);
        set => SetValue(SearchButtonProperty, value);
    }

    public static readonly DependencyProperty NewButtonProperty =
        DependencyProperty.Register(nameof(NewButton),
            typeof(string),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public string NewButton
    {
        get => (string)GetValue(NewButtonProperty);
        set => SetValue(NewButtonProperty, value);
    }

    public static readonly DependencyProperty NewVisibilityProperty =
        DependencyProperty.Register(nameof(NewVisibility),
            typeof(Visibility),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public Visibility NewVisibility
    {
        get => (Visibility)GetValue(NewVisibilityProperty);
        set => SetValue(NewVisibilityProperty, value);
    }

    public static readonly DependencyProperty AddVisibilityProperty =
        DependencyProperty.Register(nameof(AddVisibility),
            typeof(Visibility),
            typeof(ToolbarPrefab),
            new PropertyMetadata(null));

    public Visibility AddVisibility
    {
        get => (Visibility)GetValue(AddVisibilityProperty);
        set => SetValue(AddVisibilityProperty, value);
    }
    
    public static readonly DependencyProperty PrintVisibilityProperty =
        DependencyProperty.Register(nameof(PrintVisibility),
            typeof(Visibility),
            typeof(ToolbarPrefab),
            new PropertyMetadata(Visibility.Visible));

    public Visibility PrintVisibility
    {
        get => (Visibility)GetValue(PrintVisibilityProperty);
        set => SetValue(PrintVisibilityProperty, value);
    }
    
    public static readonly DependencyProperty ExtraVisibilityProperty =
        DependencyProperty.Register(nameof(ExtraVisibility),
            typeof(Visibility),
            typeof(ToolbarPrefab),
            new PropertyMetadata(Visibility.Collapsed));

    public Visibility ExtraVisibility
    {
        get => (Visibility)GetValue(ExtraVisibilityProperty);
        set => SetValue(ExtraVisibilityProperty, value);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void DataSizeChanged(object sender, SizeChangedEventArgs e, double? width = null)
    {
        DockPanel.MinWidth = e.NewSize.Width * (width ?? ControlScrollerWidthMultiplier);
    }
}