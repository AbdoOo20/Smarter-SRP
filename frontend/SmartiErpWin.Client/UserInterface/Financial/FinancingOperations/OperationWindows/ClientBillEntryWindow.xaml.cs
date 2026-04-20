using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class ClientBillEntryWindow
{
    public ClientBillEntryViewModel ViewModel { get; }
    
    public ClientBillEntryWindow(LocalisationService localisationService,
        RSimpleRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new ClientBillEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }

    private void OnAddWindowClosed(object? sender, string e)
    {
        ViewModel.Refresh();
    }
}