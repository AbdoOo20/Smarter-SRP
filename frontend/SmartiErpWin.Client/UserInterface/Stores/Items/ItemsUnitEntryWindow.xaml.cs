using System.Windows.Input;

namespace SmartiErpWin.Client.UserInterface.Stores.Items;

public partial class ItemsUnitEntryWindow
{
    public ItemsUnitEntryViewModel ViewModel { get; }
    
    public ItemsUnitEntryWindow()
    {
        var localisationService = App.GetService<LocalisationService>();
        var client = App.GetService<HttpClient>();
        var pageType = typeof(ItemPageEntryViewModel);

        ViewModel = new ItemsUnitEntryViewModel(
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