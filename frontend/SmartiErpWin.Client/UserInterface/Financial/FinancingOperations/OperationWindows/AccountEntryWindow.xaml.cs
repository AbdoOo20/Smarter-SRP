using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class AccountEntryWindow
{
    public AccountEntryViewModel ViewModel { get; }
    
    public AccountEntryWindow(LocalisationService localisationService,
        RSimpleRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new AccountEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}