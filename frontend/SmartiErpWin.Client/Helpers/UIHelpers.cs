using System.Windows.Controls;
using Fly.Framework.Shared.Common.ValueTypes;
using SmartErpWin.Core.Helpers;
using SmartiErpWin.Client.DtoModels;
using Wpf.Ui.Controls;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace SmartiErpWin.Client.Helpers;

public static class UiHelpers
{
    public static SuggestBoxCreationInfo<T> CreateSuggestionBox<T>(
        Panel? panel,
        string title,
        List<T> models,
        string placeholder = "Search",
        double width = 250,
        Action? onClear = null) where T: class
    {
        var autoSuggestBox = new AutoSuggestBox
        {
            Width = width,
            PlaceholderText = App.GetService<LocalisationService>()?[placeholder] ?? placeholder,
            DisplayMemberPath = "Display",
            Margin = new Thickness(0, 5, 0, 0)
        };

        autoSuggestBox.GotFocus += async (sender, args) =>
        {
            autoSuggestBox.IsSuggestionListOpen = true;
        };
                
        var returnedInfo = new SuggestBoxCreationInfo<T>()
        {
            Box = autoSuggestBox,
        };

        autoSuggestBox.SuggestionChosen += (_, args) =>
        {
            returnedInfo.RaiseSuggestionChosen(args.SelectedItem as T);

            autoSuggestBox.IsSuggestionListOpen = false;
        };

        autoSuggestBox.OriginalItemsSource = models;
        autoSuggestBox.ItemsSource = models;
            
        autoSuggestBox.TextChanged += (_, _) =>
        {
            if (typeof(T) == typeof(ComboModel))
            {
                var search = SearchHelper.Search(autoSuggestBox.Text,
                    models.Cast<ComboModel>().ToList());
                autoSuggestBox.ItemsSource = search;
            }
            else if (typeof(T) == typeof(LookupOf<string, string>))
            {
                var search = SearchHelper.SearchLookup(autoSuggestBox.Text,
                    models.Cast<LookupOf<string, string>>().ToList());
                autoSuggestBox.ItemsSource = search;
            }

            if (string.IsNullOrEmpty(autoSuggestBox.Text))
            {
                onClear?.Invoke();
            }
        };

        var textBlock = new TextBlock
        {
            Text = title
        };

        var stack = new StackPanel
        {
            Margin = new Thickness(5, 0, 0, 0),
            Children =
            {
                textBlock,
                autoSuggestBox
            }
        };
        
        panel?.Children.Add(stack);

        return returnedInfo;
    }
}