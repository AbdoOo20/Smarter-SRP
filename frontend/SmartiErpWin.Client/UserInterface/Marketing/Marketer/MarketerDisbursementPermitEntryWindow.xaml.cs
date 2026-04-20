using AutoMapper;
using Fly.Shared.Modules.Marketers.Dtos;

namespace SmartiErpWin.Client.UserInterface.Marketing.Marketer;

public partial class MarketerDisbursementPermitEntryWindow
{
    public MarketerDisbursementPermitEntryViewModel ViewModel { get; }
    
    public MarketerDisbursementPermitEntryWindow(LocalisationService localisationService,
        DtoMarketerDisbursementPermit model, EntryType type, HttpClient client, Type pageType, IMapper mapper)
    {
        ViewModel = new MarketerDisbursementPermitEntryViewModel(this,
            localisationService, model, type, client, mapper, pageType);
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.EntryState = EntryState;
        ViewModel.ShowData(DataGrid);
    }

    private void SecAutoSuggestPrefab_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        ViewModel.OnStorageChanged();
    }
}