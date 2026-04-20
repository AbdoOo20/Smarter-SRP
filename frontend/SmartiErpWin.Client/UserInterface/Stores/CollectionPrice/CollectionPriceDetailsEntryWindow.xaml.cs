using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Stores.CollectionPrice;

public partial class CollectionPriceDetailsEntryWindow
{
    public CollectionPriceDetailsEntryViewModel ViewModel { get; }

    public CollectionPriceDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(CollectionPricePageEntryViewModel);

        ViewModel = new CollectionPriceDetailsEntryViewModel(
            localisationService, client, pageType);

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
