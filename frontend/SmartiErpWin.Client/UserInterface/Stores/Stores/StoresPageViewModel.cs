using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.Stores;

public partial class StoresPageViewModel : BasePageViewModel<DtoStore>
{
    protected override string Endpoint => "Store";
    
    public StoresPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoStore>(sender);

        if (model is null)
            return;
        
        var entry = new StoreEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new StoreEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}