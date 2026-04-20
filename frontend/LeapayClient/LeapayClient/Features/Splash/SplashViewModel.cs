using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;

namespace Leapay.Client.Features.Splash;

public partial class SplashViewModel(PageNavigationService nav) : BasePageViewModel<DtoBase>
{
    [ObservableProperty] private bool _dashBoardVisited;
    
    [RelayCommand]
    private void OpenDashboard()
    {
        DashBoardVisited = true;
    }
}