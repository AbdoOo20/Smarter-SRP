using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public partial class PurchasesInvoiceCostsEntryWindow
{
    public PurchasesInvoiceCostsEntryViewModel ViewModel { get; }
    
    public PurchasesInvoiceCostsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(PurchasesInvoiceEntryViewModel);

        ViewModel = new PurchasesInvoiceCostsEntryViewModel(
            localisationService, client, pageType);

        InitializeComponent();

        DataContext = this;
    }
    
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        HelperServices.HandlePreviewKey(e);
    }

    public void FocusFirst()
    {
        FocusScapegoatBox.Focus();
    }
}