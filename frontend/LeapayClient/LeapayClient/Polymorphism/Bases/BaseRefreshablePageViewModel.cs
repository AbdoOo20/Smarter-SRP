using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;

namespace Leapay.Client.Polymorphism.Bases;

public abstract partial class BaseRefreshablePageViewModel<T> : BasePageViewModel<T>,
    IRefreshable
    where T: class, new()
{
    #region Observability

    [ObservableProperty]
    public partial bool UiVisibility { get; set; }

    [ObservableProperty]
    public partial bool RingVisibility { get; set; }

    [ObservableProperty]
    public partial string? LoadingInfo { get; set; }

    [ObservableProperty]
    public partial string? LoadingTitle { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

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

        UiVisibility = !IsRefreshing;
        RingVisibility = IsRefreshing;
    }

    public async void RefreshState(Func<Task> action)
    {
        if (IsRefreshing)
            return;

        await As<IWrappable>().WrapOneAsync(action);
    }
}