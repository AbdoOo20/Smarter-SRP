using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace SmartiErpWin.Client.UserInterface.Sales.BankBranches;

public partial class BankBranchEntryWindow
{
    public BankBranchPageEntryViewModel ViewModel { get; }
    
    public BankBranchEntryWindow(LocalisationService localisationService,
        DtoBankBranch model,
        EntryType type,
        HttpClient client,
        Type pageType)
    {
        ViewModel = new BankBranchPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}