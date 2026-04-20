using System.Collections;
using System.Windows.Controls;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Sales.Clients;

namespace SmartiErpWin.Client.Prefabs;

public partial class ComboPrefabObsolete
{
    public ComboPrefabObsolete()
    {
        InitializeComponent();
        
        Loaded += OnLoaded;
    }

    public event EventHandler<string>? AddWindowClosed;
    
    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(DestinationWindow))
            return;

        var judge = await App.GetService<SessionService>().JudgeAccessAsync(DestinationWindow, ShowPermission);
        
        AddVisibility = judge.ApplyVisibility();
    }

    public EventHandler? SelectionChanged;

    public event EventHandler? OnSelectionChanged;
    
    public static readonly DependencyProperty TitleTextProperty =
        DependencyProperty.Register(nameof(TitleText),
            typeof(string),
            typeof(ComboPrefabObsolete),
            new PropertyMetadata(string.Empty));

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }
    
    public static readonly DependencyProperty DestinationWindowProperty =
        DependencyProperty.Register(nameof(DestinationWindow),
            typeof(string),
            typeof(ComboPrefabObsolete),
            new PropertyMetadata(string.Empty));

    public string DestinationWindow
    {
        get => (string)GetValue(DestinationWindowProperty);
        set => SetValue(DestinationWindowProperty, value);
    }
    
    public static readonly DependencyProperty AddVisibilityProperty =
        DependencyProperty.Register(nameof(AddVisibility),
            typeof(Visibility),
            typeof(ComboPrefabObsolete),
            new PropertyMetadata(Visibility.Collapsed));

    public Visibility AddVisibility
    {
        get => (Visibility)GetValue(AddVisibilityProperty);
        set => SetValue(AddVisibilityProperty, value);
    }
    
    public static readonly DependencyProperty MinComboWidthProperty =
        DependencyProperty.Register(nameof(MinComboWidth),
            typeof(double),
            typeof(ComboPrefabObsolete),
            new PropertyMetadata(300d));

    public double MinComboWidth
    {
        get => (double)GetValue(MinComboWidthProperty);
        set => SetValue(MinComboWidthProperty, value);
    }
    
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(ComboPrefabObsolete),
            new PropertyMetadata(null));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem),
            typeof(ComboModel),
            typeof(ComboPrefabObsolete),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public ComboModel SelectedItem
    {
        get => (ComboModel)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    private void SelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        OnSelectionChanged?.Invoke(sender, e);
    }

    private void OnAdd(object sender, RoutedEventArgs e)
    {
        var locale = App.GetService<LocalisationService>();
        var httpClient = App.GetService<HttpClient>();

        Window window = DestinationWindow switch
        {
            "Clients" => new ClientEntryWindow(locale, null, EntryType.Add, httpClient, typeof(ClientsPageViewModel)),
            _ => null
        };

        if (window is null)
        {
            _ = DialogueServices.ShowMessageAsync("ERROR", $"WINDOW is UNDEFINED for {DestinationWindow}. Source: ComboPrefab");
            return;
        }
        
        window.ShowDialog();
        
        AddWindowClosed?.Invoke(this, CodeBook.RefData);
    }
}