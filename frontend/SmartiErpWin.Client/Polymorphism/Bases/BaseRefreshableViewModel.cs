using SmartiErpWin.Client.Polymorphism.Interfaces;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public abstract partial class BaseRefreshableViewModel<T>(HttpClient client) : BaseViewModel<T>(client),
                                                                               IRefreshable
                                                                               where T: class, new()
{
    #region Observability

    [ObservableProperty]
    private Visibility _uiVisibility;

    [ObservableProperty]
    private Visibility _ringVisibility;
    
    [ObservableProperty]
    private string? _loadingInfo;
    
    [ObservableProperty]
    private string? _loadingTitle;
    
    [ObservableProperty]
    private bool _isRefreshing;

    #endregion

    public void SetInfo(string title, string info = "")
    {
        LoadingTitle = title;
        LoadingInfo = info;
    }
    
    public virtual void SetRefreshState(bool isRefreshing)
    {
        var localeService = App.GetService<LocalisationService>();
        
        SetInfo(isRefreshing ? localeService?["Loading"] ?? string.Empty : string.Empty);
        IsRefreshing = isRefreshing;

        UiVisibility = IsRefreshing ? Visibility.Hidden : Visibility.Visible;
        RingVisibility = IsRefreshing ? Visibility.Visible : Visibility.Collapsed;
    }

    public async void RefreshState(Func<Task> action)
    {
        if (IsRefreshing)
            return;

        await As<IWrappable>().WrapOneAsync(action);
    }
}