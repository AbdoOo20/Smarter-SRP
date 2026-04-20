using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Stores.SupplyPermits;

public partial class SupplyPermitDetailsEntryWindow
{
    public SupplyPermitDetailsEntryViewModel ViewModel { get; }
    
    public SupplyPermitDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(SupplyPermitPageEntryViewModel);

        ViewModel = new SupplyPermitDetailsEntryViewModel(
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