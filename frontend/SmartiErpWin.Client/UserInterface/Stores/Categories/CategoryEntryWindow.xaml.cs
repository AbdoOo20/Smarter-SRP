using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.Categories;

public partial class CategoryEntryWindow
{
    public CategoryPageEntryViewModel ViewModel { get; }
    
    public CategoryEntryWindow(LocalisationService localisationService,
        DtoCategory model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CategoryPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}