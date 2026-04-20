using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class DiscountDepositFinanceEntryWindow
{
    public DiscountDepositFinanceEntryViewModel ViewModel { get; }
    
    public DiscountDepositFinanceEntryWindow(LocalisationService localisationService,
        RSimpleRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new DiscountDepositFinanceEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}