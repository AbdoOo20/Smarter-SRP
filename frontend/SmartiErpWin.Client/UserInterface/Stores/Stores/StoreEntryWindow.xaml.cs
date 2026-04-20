using Fly.Shared.Modules.Store.Dtos;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.UserInterface.Stores.Stores;

public partial class StoreEntryWindow
{
    public CommonPageEntryViewModel<DtoStore> ViewModel { get; }
    
    public StoreEntryWindow(LocalisationService localisationService,
        DtoStore model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CommonPageEntryViewModel<DtoStore>(this,
            localisationService, model, type, "Store", client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}