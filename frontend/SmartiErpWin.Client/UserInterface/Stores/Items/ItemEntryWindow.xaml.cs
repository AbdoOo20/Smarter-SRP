using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.Items;

public partial class ItemsEntryWindow
{
    public ItemPageEntryViewModel ViewModel { get; }
    
    public ItemsEntryWindow(LocalisationService localisationService,
        DtoItem model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new ItemPageEntryViewModel(this,
            localisationService, model, type, client, mapper, pageType);
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.EntryState = EntryState;
        ViewModel.ShowData(DataGrid);
    }
}