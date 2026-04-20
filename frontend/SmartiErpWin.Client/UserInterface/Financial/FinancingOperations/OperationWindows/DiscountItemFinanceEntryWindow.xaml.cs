using System.Windows.Controls;
using Fly.Shared.Modules.Financial.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;

public partial class DiscountItemFinanceEntryWindow
{
    public DiscountItemFinanceEntryViewModel ViewModel { get; }
    
    public DiscountItemFinanceEntryWindow(LocalisationService localisationService,
        DiscountItemBasedRequest model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new DiscountItemFinanceEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }

    private void ItemSelected(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetUnits();
    }

    private void MarketerSelected(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetMarketerInfo();

    }
}