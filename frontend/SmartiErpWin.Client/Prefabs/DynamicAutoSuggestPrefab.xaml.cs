using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Fly.Framework.Shared.Common.ValueTypes;
using SmartErpWin.Core.Helpers;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace SmartiErpWin.Client.Prefabs;

public partial class DynamicAutoSuggestPrefab
{
    public DynamicAutoSuggestPrefab()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    public event SelectionChangedEventHandler? OnSelectionChanged;
    
    private void OnLoaded(object o, RoutedEventArgs routedEventArgs)
    {
        if (ItemsSource is ObservableCollection<LookupOf<string, string>> comboModels)
        {
            comboModels.CollectionChanged += (_, _) => SetSuggestions(Search ?? string.Empty);
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SetSuggestions((sender as TextBox)?.Text ?? string.Empty);
    }
    
    public static readonly DependencyProperty TitleTextProperty =
        DependencyProperty.Register(nameof(TitleText),
            typeof(string),
            typeof(DynamicAutoSuggestPrefab),
            new PropertyMetadata(string.Empty));

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public static readonly DependencyProperty MinComboWidthProperty =
        DependencyProperty.Register(nameof(MinComboWidth),
            typeof(double),
            typeof(DynamicAutoSuggestPrefab),
            new PropertyMetadata(double.NaN));

    public double MinComboWidth
    {
        get => (double)GetValue(MinComboWidthProperty);
        set => SetValue(MinComboWidthProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(DynamicAutoSuggestPrefab),
            new PropertyMetadata(null));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem),
            typeof(LookupOf<string, string>),
            typeof(DynamicAutoSuggestPrefab),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public LookupOf<string, string>? SelectedItem
    {
        get => (LookupOf<string, string>)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public LookupOf<string, string>? InternalSelectedItem
    {
        get => (LookupOf<string, string>?)GetValue(InternalSelectedItemProperty);
        set => SetValue(InternalSelectedItemProperty, value);
    }

    public static readonly DependencyProperty InternalSelectedItemProperty =
        DependencyProperty.Register(
            nameof(InternalSelectedItem), 
            typeof(LookupOf<string, string>), 
            typeof(DynamicAutoSuggestPrefab), 
            new PropertyMetadata(null));

    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public static readonly DependencyProperty IsPopupOpenProperty =
        DependencyProperty.Register(
            nameof(IsPopupOpen), 
            typeof(bool), 
            typeof(DynamicAutoSuggestPrefab), 
            new PropertyMetadata(false));

    public string? Search
    {
        get => (string?)GetValue(SearchProperty);
        set => SetValue(SearchProperty, value);
    }

    public static readonly DependencyProperty SearchProperty =
        DependencyProperty.Register(
            nameof(Search), 
            typeof(string), 
            typeof(DynamicAutoSuggestPrefab), 
            new PropertyMetadata(null));

    public string? SearchPlaceholder
    {
        get => (string?)GetValue(SearchPlaceholderProperty);
        set => SetValue(SearchPlaceholderProperty, value);
    }

    public static readonly DependencyProperty SearchPlaceholderProperty =
        DependencyProperty.Register(
            nameof(SearchPlaceholder),
            typeof(string), 
            typeof(DynamicAutoSuggestPrefab), 
            new PropertyMetadata("SearchBar"));
    
    public ObservableCollection<LookupOf<string, string>> InternalItems
    {
        get => (ObservableCollection<LookupOf<string, string>>)GetValue(InternalItemsProperty);
        set => SetValue(InternalItemsProperty, value);
    }

    public static readonly DependencyProperty InternalItemsProperty =
        DependencyProperty.Register(
            nameof(InternalItems), 
            typeof(ObservableCollection<LookupOf<string, string>>), 
            typeof(DynamicAutoSuggestPrefab), 
            new PropertyMetadata(new ObservableCollection<LookupOf<string, string>>()));

    private void SetSuggestions(string query)
    {
        InternalItems.Clear();

        if (ItemsSource is null)
            return;
        
        var search = SearchHelper.SearchLookup(query, ItemsSource.Cast<LookupOf<string, string>>().ToList());

        foreach (var item in search)
        {
            InternalItems.Add(item);
        }
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.Property.Name)
        {
            case nameof(InternalSelectedItem):
            {
                OnClose();
                // This will invoke the second case automatically.
                SelectedItem = InternalSelectedItem;
                break;
            }
            case nameof(SelectedItem):
            {
                OnSelectionChanged?.Invoke(this, null);
                break;
            }
        }
    }

    private void OnOpen()
    {
        OnState(true);
    }

    private void OnClose()
    {
        OnState(false);
    }

    private void OnState(bool state)
    {
        SetState(state);
    }
    
    private void SetState(bool state)
    {
        IsPopupOpen = state;
        
        Do(() => SearchBox.Focus()).After(0.01);
    }

    private void OpenClick(object sender, RoutedEventArgs e)
    {
        OnOpen();
    }
}