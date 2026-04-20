using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.CollectionPrice;

public partial class CollectionPriceEntryWindow
{
    public CollectionPricePageEntryViewModel ViewModel { get; }
    
    public CollectionPriceEntryWindow(LocalisationService localisationService,
        DtoDisbursementPermit model, EntryType type, HttpClient client, IMapper mapper, Type pageType)
    {
        ViewModel = new CollectionPricePageEntryViewModel(this,
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