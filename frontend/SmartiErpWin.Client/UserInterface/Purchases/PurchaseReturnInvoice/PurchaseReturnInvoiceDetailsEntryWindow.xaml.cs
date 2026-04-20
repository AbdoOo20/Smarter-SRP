
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoiceDetailsEntryWindow
{
    public PurchaseReturnInvoiceDetailsEntryViewModel ViewModel { get; }

    public PurchaseReturnInvoiceDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(PurchaseReturnInvoiceEntryViewModel);

        ViewModel = new PurchaseReturnInvoiceDetailsEntryViewModel(
            localisationService, client, pageType);
        ViewModel.OnClear(() => FocusScapegoatCombo.Search = "");
        InitializeComponent();

        DataContext = this;
    }

    private void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetUnits();
        ViewModel.SetExpireDate();
    }

    private void UnitSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetPrice();
    }

    private void CategorySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetItems();
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