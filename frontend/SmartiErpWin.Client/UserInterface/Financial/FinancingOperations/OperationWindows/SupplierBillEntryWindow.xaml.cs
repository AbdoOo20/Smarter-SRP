using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class SupplierBillEntryWindow
{
    public SupplierBillEntryViewModel ViewModel { get; }
    
    public SupplierBillEntryWindow(LocalisationService localisationService,
        RSimpleRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new SupplierBillEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}