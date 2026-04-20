namespace Leapay.Client.Helpers;

public class SuggestBoxCreationInfo<T>
{
    public AutoCompleteBox? Box { get; set; }

    public event EventHandler<T>? OnSuggestionChosen;

    public void RaiseSuggestionChosen(T? suggestion)
    {
        if (suggestion is null)
            return;
        
        OnSuggestionChosen?.Invoke(this, suggestion);
    }
}