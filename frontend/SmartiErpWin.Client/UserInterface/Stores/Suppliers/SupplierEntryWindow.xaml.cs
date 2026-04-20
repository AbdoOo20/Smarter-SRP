using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.Suppliers;

public partial class SupplierEntryWindow
{
    public SupplierPageEntryViewModel ViewModel { get; }
    
    public SupplierEntryWindow(LocalisationService localisationService,
        DtoSupplier model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new SupplierPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}