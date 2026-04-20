using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.Suppliers;

public partial class SuppliersPageViewModel : BasePageViewModel<DtoSupplier>
{
    protected override string Endpoint => "Supplier";
    
    public SuppliersPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoSupplier>(sender);

        if (model is null)
            return;
        
        var entry = new SupplierEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new SupplierEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}