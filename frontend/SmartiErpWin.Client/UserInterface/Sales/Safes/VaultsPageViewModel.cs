using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Sales.Safes;

public partial class VaultsPageViewModel : BasePageViewModel<DtoSafe>
{
    protected override string Endpoint => "Safe";

    public VaultsPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoSafe>(sender);

        if (model is null)
            return;
        
        var entry = new VaultsEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new VaultsEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}