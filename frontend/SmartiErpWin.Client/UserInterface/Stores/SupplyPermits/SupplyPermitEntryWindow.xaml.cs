using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.SupplyPermits;

public partial class SupplyPermitEntryWindow
{
    public SupplyPermitPageEntryViewModel ViewModel { get; }
    
    public SupplyPermitEntryWindow(LocalisationService localisationService,
        DtoSupplyPermit model,
        EntryType type,
        HttpClient client,
        IMapper mapper,
        Type pageType)
    {
        ViewModel = new SupplyPermitPageEntryViewModel(this,
            localisationService, model, type, client, mapper, pageType);

        ViewModel.EntryState = EntryState;
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
    }
    
    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DataScroller.MaxHeight = e.NewSize.Height * DataScrollerHeightMultiplier;
        DataScroller.MaxWidth = e.NewSize.Width * DataScrollerWidthMultiplier;
    }
}