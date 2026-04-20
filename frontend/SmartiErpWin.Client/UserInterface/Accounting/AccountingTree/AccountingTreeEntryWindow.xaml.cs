using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.UserInterface.Accounting.AccountingTree;

public partial class AccountingTreeEntryWindow
{
    public AccountingTreePageEntryViewModel ViewModel { get; }
    
    public AccountingTreeEntryWindow(LocalisationService localisationService,
        DtoAccount? model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new AccountingTreePageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}