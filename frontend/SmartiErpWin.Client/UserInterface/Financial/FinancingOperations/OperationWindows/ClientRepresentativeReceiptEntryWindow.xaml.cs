using Fly.Shared.Modules.Financial.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;
using System.Windows.Controls;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class ClientRepresentativeReceiptEntryWindow
{
    public ClientRepresentativeReceiptEntryViewModel ViewModel { get; }
    
    public ClientRepresentativeReceiptEntryWindow(LocalisationService localisationService,
        RepresentativeFinancialRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new ClientRepresentativeReceiptEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }

    private void MarketerSelected(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetMarketerInfo();
    }
}