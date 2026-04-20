using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Stores.DisbursementPermits;

public partial class DisbursementsPermitDetailsEntryWindow
{
    public DisbursementsPermitDetailsEntryViewModel ViewModel { get; }

    public DisbursementsPermitDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(DisbursementsPermitPageEntryViewModel);

        ViewModel = new DisbursementsPermitDetailsEntryViewModel(
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
