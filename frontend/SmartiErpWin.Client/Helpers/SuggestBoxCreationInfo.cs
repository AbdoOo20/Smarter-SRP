using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.Helpers;

public class SuggestBoxCreationInfo<T>
{
    public AutoSuggestBox? Box { get; set; }

    public event EventHandler<T> OnSuggestionChosen;

    public void RaiseSuggestionChosen(T? suggestion)
    {
        if (suggestion is null)
            return;
        
        OnSuggestionChosen.Invoke(this, suggestion);
    }
}