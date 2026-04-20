using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingDetailsEntryWindow
{
    public QuantityCreditingDetailsEntryViewModel ViewModel { get; }
    
    public QuantityCreditingDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(QuantityCreditingEntryViewModel);

        ViewModel = new QuantityCreditingDetailsEntryViewModel(
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