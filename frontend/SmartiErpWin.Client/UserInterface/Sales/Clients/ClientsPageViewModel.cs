using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Sales.Clients;

public partial class ClientsPageViewModel : BasePageViewModel<DtoClient>
{
    protected override string Endpoint => "Client";
    
    public ClientsPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoClient>(sender);

        if (model is null)
            return;
        
        var entry = new ClientEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new ClientEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}