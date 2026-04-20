using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Sales.SellReturnInvoice;

public partial class SellReturnInvoiceDetailsEntryWindow
{
    public SellReturnInvoiceDetailsEntryViewModel ViewModel { get; }
    
    public SellReturnInvoiceDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(SellReturnInvoiceEntryViewModel);

        ViewModel = new SellReturnInvoiceDetailsEntryViewModel(
            localisationService, client, pageType);

        ViewModel.OnClear(Clear);
        InitializeComponent();

        DataContext = this;
    }

    private void Clear()
    {
        //FocusScapegoatCombo.Search = "";
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
        Clear();
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
    
    /*private void FocusScapegoatComboOnTextChanged(AutoSuggestBox autoSuggestBox, AutoSuggestBoxTextChangedEventArgs autoSuggestBoxTextChangedEventArgs)
    {
        var search = SearchHelper.Search(autoSuggestBox.Text, ViewModel.ComboModelBase.GetApiCollectionByIndex(0).ToList());
        autoSuggestBox.ItemsSource = search;
    }
    
    private void FocusScapegoatCombo_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is not ComboModel selectedItem)
            return;
        
        ViewModel.OnItemSuggestionChosen(selectedItem);
    }*/
}