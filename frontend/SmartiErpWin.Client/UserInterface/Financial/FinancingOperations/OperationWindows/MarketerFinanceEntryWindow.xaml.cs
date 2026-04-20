using System.Windows.Controls;
using Fly.Shared.Modules.Financial.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class MarketerFinanceEntryWindow
{
    public MarketerFinanceEntryViewModel ViewModel { get; }
    
    public MarketerFinanceEntryWindow(LocalisationService localisationService,
        MarketerPercentRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new MarketerFinanceEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }

    private void MarketerSelected(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetMarketerInfo();
    }
}