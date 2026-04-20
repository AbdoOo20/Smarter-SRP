using System.Collections.ObjectModel;
using System.ComponentModel;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Marketer.MarketerEntry;

public partial class MarketerEntryViewModel : BaseEntryViewModel<DtoMarketer>
{
    private readonly bool _isSilent;
    protected override string Endpoint => "Marketer";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedClients))]
    [field: Bindable(true)]
    public partial ObservableCollection<DtoClient> Clients { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<DtoClient> SelectedClients { get; set; } = [];

    public MarketerEntryViewModel(
        DtoMarketer? model,
        EntryType entryType,
        Type pageType,
        bool isSilent = false)
    {
        _isSilent = isSilent;
        SetDetails(model, entryType, pageType);
        
        Startup();
    }

    private async void Startup()
    {
        if(_isSilent)
            return;
        
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