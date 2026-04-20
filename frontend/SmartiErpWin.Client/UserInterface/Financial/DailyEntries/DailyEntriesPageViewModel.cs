using Fly.Shared.Modules.Financial.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Financial.DailyEntries;

public partial class DailyEntriesPageViewModel : BasePageViewModel<DtoStatment>
{
    protected override string Endpoint => "DailyEntries";

    public DailyEntriesPageViewModel (HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }
}