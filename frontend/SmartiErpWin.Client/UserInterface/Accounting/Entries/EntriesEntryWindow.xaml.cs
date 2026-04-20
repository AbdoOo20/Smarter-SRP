using AutoMapper;
using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.UserInterface.Accounting.Entries;

public partial class EntriesEntryWindow
{
    public EntriesEntryViewModel ViewModel { get; }
    
    public EntriesEntryWindow(LocalisationService localisationService,
        DtoEntry model, EntryType type, HttpClient client, IMapper mapper, Type pageType)
    {
        ViewModel = new EntriesEntryViewModel(this,
            localisationService, model, type, client, mapper, pageType);
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.EntryState = EntryState;
        ViewModel.ShowData(DataGrid);
    }
    
    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DataScroller.MaxHeight = e.NewSize.Height * DataScrollerHeightMultiplier;
        DataScroller.MaxWidth = e.NewSize.Width * DataScrollerWidthMultiplier;
    }
}