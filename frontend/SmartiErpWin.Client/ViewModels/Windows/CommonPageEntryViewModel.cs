namespace SmartiErpWin.Client.ViewModels.Windows;

public class CommonPageEntryViewModel<T>: BaseEntryViewModel<T> where T: class, new()
{
    protected override string? Endpoint { get; set; }

    public CommonPageEntryViewModel(Window window,
        LocalisationService locale,
        T? model,
        EntryType entryType,
        string endpoint, HttpClient client,
        Type pageType): base(client)
    {
        EntryType = entryType;
        EntryWindow = window;
        Model = model ?? new();
        PageType = pageType;

        Endpoint = endpoint;
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);
    }
}