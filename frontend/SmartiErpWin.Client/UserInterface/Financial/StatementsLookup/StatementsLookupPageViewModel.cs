using Fly.Framework.Shared.Common.Dtos;
using SmartiErpWin.Client.UserInterface.Financial.StatementsLookup.OperationWindows;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Financial.StatementsLookup;

public partial class StatementsLookupPageViewModel : BaseViewModel<DtoBase>
{
    public StatementsLookupPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }
    
    [RelayCommand]
    private void OnAddForItemUnit()
    {
        var entry = new ItemUnitStatementLookupWindow(LocaleService,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}