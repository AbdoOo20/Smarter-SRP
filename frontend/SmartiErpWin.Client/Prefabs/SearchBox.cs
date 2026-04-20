using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace SmartiErpWin.Client.Prefabs;

public class SearchBox : ComboBox
{
    private readonly DispatcherTimer _debounceTimer;

    private TextBox? _searchTermTextBox;
        
    private CollectionViewSource? _collectionViewSource;
        
    private object? _lastSelectedItem;

    private bool _isDropDownOpen;
        
    private TextBlock? _placeholderTextBlock;

    public SearchBox()
    {
        _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };
            
        _debounceTimer.Tick += OnDebounceTimerTick;
        DropDownClosed += OnDropDownClosed;
    }

    static SearchBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox),
            new FrameworkPropertyMetadata(typeof(SearchBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _searchTermTextBox = Template.FindName("SearchTermTextBox", this) as TextBox;
        _placeholderTextBlock = Template.FindName("PlaceholderTextBlock", this) as TextBlock;
        UpdatePlaceholderVisibility();
    }

    public static readonly DependencyProperty CustomSortProperty =
        DependencyProperty.Register(
            nameof(CustomSort),
            typeof(IComparer),
            typeof(SearchBox),
            new PropertyMetadata(null, OnCustomSortChanged));

    public IComparer CustomSort
    {
        get => (IComparer)GetValue(CustomSortProperty);
        set => SetValue(CustomSortProperty, value);
    }

    private static void OnCustomSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var comboBox = d as SearchBox;
            
        if (comboBox?._collectionViewSource is { View: ListCollectionView lcv })
        {
            lcv.CustomSort = e.NewValue as IComparer;
        }
    }

    public static readonly DependencyProperty IsSearchEnabledProperty =
        DependencyProperty.Register(
            nameof(IsSearchEnabled),
            typeof(bool),
            typeof(SearchBox),
            new PropertyMetadata(true));

    public bool IsSearchEnabled
    {
        get => (bool)GetValue(IsSearchEnabledProperty);
        set => SetValue(IsSearchEnabledProperty, value);
    }

    public static readonly DependencyProperty PlaceholderStyleProperty =
        DependencyProperty.Register(nameof(PlaceholderStyle), typeof(Style), typeof(SearchBox),
            new PropertyMetadata(null));

    public Style PlaceholderStyle
    {
        get => (Style)GetValue(PlaceholderStyleProperty);
        set => SetValue(PlaceholderStyleProperty, value);
    }

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(SearchBox),
            new PropertyMetadata(string.Empty));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    private void UpdatePlaceholderVisibility()
    {
        if (_placeholderTextBlock == null) return;

        if (SelectedItem == null || SelectedItem.Equals(string.Empty))
        {
            _placeholderTextBlock.Visibility = Visibility.Visible;
        }
        else
        {
            _placeholderTextBlock.Visibility = Visibility.Collapsed;
        }
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);

        if (_collectionViewSource != null)
            return;
            
        _collectionViewSource = new CollectionViewSource
        {
            Source = newValue,
            View =
            {
                Filter = FilterPredicate
            }
        };

        if (_collectionViewSource.View is ListCollectionView lcv)
        {
            lcv.CustomSort = CustomSort;
        }

        ItemsSource = _collectionViewSource.View;
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        if (_isDropDownOpen && e.AddedItems.Count == 0 && e.RemovedItems.Count > 0)
        {
            SelectedItem = _lastSelectedItem;
            return;
        }

        if (e.AddedItems.Count > 0)
        {
            _lastSelectedItem = e.AddedItems[0];
        }

        UpdatePlaceholderVisibility();
        base.OnSelectionChanged(e);
    }

    public static readonly DependencyProperty NotFoundLabelVisibilityProperty =
        DependencyProperty.Register(nameof(NotFoundLabelVisibility), typeof(Visibility), typeof(SearchBox),
            new PropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty DropDownBorderVisibilityProperty =
        DependencyProperty.Register(nameof(DropDownBorderVisibility), typeof(Visibility), typeof(SearchBox),
            new PropertyMetadata(Visibility.Visible));

    public Visibility NotFoundLabelVisibility
    {
        get => (Visibility)GetValue(NotFoundLabelVisibilityProperty);
        set => SetValue(NotFoundLabelVisibilityProperty, value);
    }

    public Visibility DropDownBorderVisibility
    {
        get => (Visibility)GetValue(DropDownBorderVisibilityProperty);
        set => SetValue(DropDownBorderVisibilityProperty, value);
    }

    private void UpdateVisibility()
    {
        if (_collectionViewSource != null && _collectionViewSource.View.IsEmpty)
        {
            NotFoundLabelVisibility = Visibility.Visible;
            DropDownBorderVisibility = Visibility.Collapsed;
        }
        else
        {
            NotFoundLabelVisibility = Visibility.Collapsed;
            DropDownBorderVisibility = Visibility.Visible;
        }
    }

    public static readonly DependencyProperty SearchTermProperty =
        DependencyProperty.Register(nameof(SearchTerm), typeof(string), typeof(SearchBox),
            new PropertyMetadata(string.Empty, OnSearchTermChanged));
        
    public string SearchTerm
    {
        get => (string)GetValue(SearchTermProperty);
        set => SetValue(SearchTermProperty, value);
    }

    private static void OnSearchTermChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SearchBox searchableComboBox)
        {
            searchableComboBox.DebounceFilter();
        }
    }

    private void DebounceFilter()
    {
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private void OnDebounceTimerTick(object? sender, EventArgs e)
    {
        _debounceTimer.Stop();

        _collectionViewSource?.View.Refresh();
        UpdateVisibility();
    }

    private bool FilterPredicate(object item)
    {
        if (string.IsNullOrEmpty(SearchTerm))
        {
            return true;
        }

        var strItem = item.ToString();

        return strItem?.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    protected override void OnDropDownOpened(EventArgs e)
    {
        _isDropDownOpen = true;
        _lastSelectedItem = SelectedItem;

        Dispatcher.BeginInvoke(delegate { _searchTermTextBox?.Focus(); },
            DispatcherPriority.ContextIdle);

        base.OnDropDownOpened(e);
    }

    private void OnDropDownClosed(object? sender, EventArgs e)
    {
        SearchTerm = string.Empty;
        _collectionViewSource?.View.Refresh();

        if (_lastSelectedItem != null)
        {
            SelectedItem = _lastSelectedItem;
        }

        _isDropDownOpen = false;
    }
}