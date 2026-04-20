using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Accounting.Entries;

public partial class EntryDetailEntryWindow
{
    public EntriesDetailPageEntryViewModel ViewModel { get; }
    
    public EntryDetailEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(EntriesEntryViewModel);

        ViewModel = new EntriesDetailPageEntryViewModel(
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
        FocusScapegoatCombo.Focus();
    }
}