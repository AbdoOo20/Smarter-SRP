using System.Collections.ObjectModel;
using Avalonia.Interactivity;
using Leapay.Client.ClientModels.Dto.DtoModels;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.UserInterface.Reports.GeneralReport;

public partial class AutoSuggestCombo : UserControl
{
    public event EventHandler? OnSelectionChanged;
    
    public static readonly StyledProperty<string> TitleTextProperty = AvaloniaProperty.Register<AutoSuggestCombo, string>(nameof(TitleText), string.Empty);

    public string TitleText
    {
        get => GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public static readonly StyledProperty<double> MinComboWidthProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, double>(nameof(MinComboWidth), double.NaN);

    public double MinComboWidth
    {
        get => GetValue(MinComboWidthProperty);
        set => SetValue(MinComboWidthProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<ComboModel>> ItemsSourceProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, ObservableCollection<ComboModel>>(nameof(ItemsSource), new ObservableCollection<ComboModel>());

    public ObservableCollection<ComboModel> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly StyledProperty<ComboModel?> SelectedItemProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, ComboModel?>(nameof(SelectedItem));

    public ComboModel? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<ComboModel?> InternalSelectedItemProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, ComboModel?>(nameof(InternalSelectedItem), null);

    public ComboModel? InternalSelectedItem
    {
        get => GetValue(InternalSelectedItemProperty);
        set => SetValue(InternalSelectedItemProperty, value);
    }

    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, bool>(nameof(IsPopupOpen), false);

    public bool IsPopupOpen
    {
        get => GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public static readonly StyledProperty<string?> SearchProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, string?>(nameof(Search), null);

    public string? Search
    {
        get => GetValue(SearchProperty);
        set => SetValue(SearchProperty, value);
    }

    public static readonly StyledProperty<string?> SearchPlaceholderProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, string?>(nameof(SearchPlaceholder), "SearchBar");

    public string? SearchPlaceholder
    {
        get => GetValue(SearchPlaceholderProperty);
        set => SetValue(SearchPlaceholderProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<ComboModel>> InternalItemsProperty =
        AvaloniaProperty.Register<AutoSuggestCombo, ObservableCollection<ComboModel>>(nameof(InternalItems), new ObservableCollection<ComboModel>());

    public ObservableCollection<ComboModel> InternalItems
    {
        get => GetValue(InternalItemsProperty);
        set => SetValue(InternalItemsProperty, value);
    }
    
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SetSuggestions((sender as TextBox)?.Text ?? string.Empty);
    }
    
    private void SetSuggestions(string query)
    {
        var temp = ItemsSource.ToList();
        InternalItems.Clear();

        var search = SearchHelper.Search(query, temp);

        foreach (var item in search)
        {
            InternalItems.Add(item);
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
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
    }

    private void OpenClick(object sender, RoutedEventArgs e)
    {
        OnOpen();
    }
}