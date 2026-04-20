using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoiceDetailsEntryWindow
{
    public SalesInvoiceDetailsEntryViewModel ViewModel { get; }
    
    public SalesInvoiceDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(SalesInvoiceEntryViewModel);
        
        ViewModel = new SalesInvoiceDetailsEntryViewModel(
            localisationService, client, pageType, this);
        ViewModel.OnClear(() =>
        {
            //FocusScapegoatCombo.Search = "";
        });

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
    
    /*private void CategorySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetItemsByCategory();
    }*/
    
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (ViewModel.IsSelectionWindowOpen)
            return;
        
        HelperServices.HandlePreviewKey(e);
    }

    public void FocusFirst()
    {
        FocusTarget.Focus();
    }
    
    /*private void FocusScapegoatComboOnTextChanged(AutoSuggestBox autoSuggestBox, Wpf.Ui.Controls.AutoSuggestBoxTextChangedEventArgs autoSuggestBoxTextChangedEventArgs)
    {
        var search = SearchHelper.Search(autoSuggestBox.Text, ViewModel.ComboModelBase.GetApiCollectionByIndex(0).ToList());
        autoSuggestBox.ItemsSource = search;

        if (string.IsNullOrEmpty(FocusScapegoatCombo.Text))
        {
            ViewModel.ClearItem();
        }
    }*/

    /*private void FocusScapegoatCombo_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is not ComboModel selectedItem)
            return;
        
        ViewModel.OnItemSuggestionChosen(selectedItem);
        FocusScapegoatCombo.IsSuggestionListOpen = false;
    }*/

    /*private void FocusScapegoatCombo_OnGotFocus(object sender, RoutedEventArgs e)
    {
        FocusScapegoatCombo.IsSuggestionListOpen = true;
    }*/
}