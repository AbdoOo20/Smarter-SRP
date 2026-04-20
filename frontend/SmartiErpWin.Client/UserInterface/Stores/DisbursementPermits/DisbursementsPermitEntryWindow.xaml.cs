using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.DisbursementPermits;

public partial class DisbursementsPermitEntryWindow
{
    public DisbursementsPermitPageEntryViewModel ViewModel { get; }
    
    public DisbursementsPermitEntryWindow(LocalisationService localisationService,
        DtoDisbursementPermit model, EntryType type, HttpClient client, IMapper mapper, Type pageType)
    {
        ViewModel = new DisbursementsPermitPageEntryViewModel(this,
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