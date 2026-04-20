using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Sales.Banks;

public partial class BanksPageViewModel : BasePageViewModel<DtoBank>
{
    protected override string Endpoint => "Bank";

    public BanksPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoBank>(sender);

        if (model is null)
            return;
        
        var entry = new BankEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new BankEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}