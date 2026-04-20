using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace SmartiErpWin.Client.UserInterface.Sales.Clients;

public partial class ClientEntryWindow
{
    public ClientPageEntryViewModel ViewModel { get; }
    
    public ClientEntryWindow(LocalisationService localisationService,
        DtoClient model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new ClientPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}