using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.UserInterface.Accounting.Assets;

public partial class AssetEntryWindow
{
    public AssetPageEntryViewModel ViewModel { get; }
    
    public AssetEntryWindow(LocalisationService localisationService,
        DtoAssets model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new AssetPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}