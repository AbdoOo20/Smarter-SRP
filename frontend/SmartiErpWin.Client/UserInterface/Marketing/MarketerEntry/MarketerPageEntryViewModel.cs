using System.Collections.ObjectModel;
using System.ComponentModel;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Requests;

namespace SmartiErpWin.Client.UserInterface.Marketing.MarketerEntry;

public sealed partial class MarketerPageEntryViewModel : BaseEntryViewModel<DtoMarketer>
{
    protected override string Endpoint => "Marketer";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedClients))]
    [Bindable(true)]
    private ObservableCollection<DtoClient> _clients = new();
    
    [ObservableProperty]
    private ObservableCollection<DtoClient> _selectedClients = new();
    
    public MarketerPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoMarketer? model,
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

        Startup();
    }

    private async void Startup()
    {
        Clients.Clear();

        var clients = await GetListInnerByQueryAsync<DtoClient>("Clients", new ClientQueryRequest()
        {
            MarketerId = Model.Id,
        });

        foreach (var dto in clients.Data)
        {
            Clients.Add(dto);
        }
        
        SelectedClients.Clear();

        var selected = Clients.Where(client => Model.MarketerClients.Any(m => m.ClientId == client.Id)).ToList();

        foreach (var client in selected)
        {
            SelectedClients.Add(client);
        }
    }

    protected override void PreSaveActions()
    {
        Model.MarketerClients.Clear();

        foreach (var selected in SelectedClients)
        {
            Model.MarketerClients.Add(new()
            {
                ClientId = selected.Id
            });
        }
    }
}