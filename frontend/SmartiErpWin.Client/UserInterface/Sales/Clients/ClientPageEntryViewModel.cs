using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace SmartiErpWin.Client.UserInterface.Sales.Clients;

public class ClientPageEntryViewModel : BaseEntryViewModel<DtoClient>
{
    protected override string Endpoint => "Client";
    
    public ClientPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoClient? model,
        EntryType entryType,
        HttpClient client,
        Type pageType): base(client)
    {
        Model = model ?? new();
        EntryWindow = window;
        EntryType = entryType;
        PageType = pageType;
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        Refresh();
    }

    private async void Refresh()
    {

    }

    protected override void PreSaveActions()
    {
        
    }
}