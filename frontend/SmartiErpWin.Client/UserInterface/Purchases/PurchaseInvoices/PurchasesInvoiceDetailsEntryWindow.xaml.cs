using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public partial class PurchasesInvoiceDetailsEntryWindow
{
    public PurchasesInvoiceDetailsEntryViewModel ViewModel { get; }
    
    public PurchasesInvoiceDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(PurchasesInvoiceEntryViewModel);

        ViewModel = new PurchasesInvoiceDetailsEntryViewModel(
            localisationService, client, pageType);
        ViewModel.OnClear(() => FocusScapegoatCombo.Search = "");
        InitializeComponent();

        DataContext = this;
    }

    private void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetUnits();
    }
    
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        HelperServices.HandlePreviewKey(e);
    }

    public void FocusFirst()
    {
        FocusScapegoatCombo.Focus();
    }
}