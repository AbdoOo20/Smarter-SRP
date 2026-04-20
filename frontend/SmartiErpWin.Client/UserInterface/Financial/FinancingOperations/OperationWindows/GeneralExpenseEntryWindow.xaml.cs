using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class GeneralExpenseEntryWindow
{
    public GeneralExpenseEntryViewModel ViewModel { get; }
    
    public GeneralExpenseEntryWindow(LocalisationService localisationService,
        RSimpleRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new GeneralExpenseEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}