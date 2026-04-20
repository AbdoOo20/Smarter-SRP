using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;

namespace Leapay.Client.Services;

public class PageNavigationService
{
    public Action<Type>? NavigationRequested { get; set; }
    
    public Action<TabbedViewCardModel>? StandaloneNavigationRequested { get; set; }

    public Type? PreviousPage { get; set; }
    
    public void RequestNavigation<T>() where T : UserControl
    {
        NavigationRequested?.Invoke(typeof(T));
    }
    
    public void RequestNavigation(Type? type)
    {
        if (type is null)
            return;
        
        NavigationRequested?.Invoke(type);
    }
    
    public void RequestStandaloneNavigation(TabbedViewCardModel? type)
    {
        if (type is null)
            return;
        
        StandaloneNavigationRequested?.Invoke(type);
    }
}