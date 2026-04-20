using System.Windows.Controls;
using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Marketing.Marketer;

public partial class MarketerDisbursementPermitDetailsEntryWindow
{
    public MarketerDisbursementPermitDetailsEntryViewModel ViewModel { get; }
    
    public MarketerDisbursementPermitDetailsEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(MarketerDisbursementPermitEntryViewModel);

        ViewModel = new MarketerDisbursementPermitDetailsEntryViewModel(
            localisationService, client, pageType);

        InitializeComponent();

        DataContext = this;
    }
    
    private void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetUnits();
        ViewModel.SetExpireDate();
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