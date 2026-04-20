using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class EmployeeFinanceEntryWindow
{
    public EmployeeFinanceEntryViewModel ViewModel { get; }
    
    public EmployeeFinanceEntryWindow(LocalisationService localisationService,
        RSimpleRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new EmployeeFinanceEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}