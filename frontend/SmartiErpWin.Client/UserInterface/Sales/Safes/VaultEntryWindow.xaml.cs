using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace SmartiErpWin.Client.UserInterface.Sales.Safes;

public partial class VaultsEntryWindow
{
    public SafePageEntryViewModel ViewModel { get; }
    
    public VaultsEntryWindow(LocalisationService localisationService,
        DtoSafe model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new SafePageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}