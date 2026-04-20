using Fly.Framework.Shared.Common.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.ViewModels.Pages;

public partial class DashboardPageViewModel : BaseViewModel<DtoBase>
{
    public DashboardPageViewModel(HttpClient client, IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }
}