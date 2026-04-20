using Fly.Shared.Modules.Marketers.Dtos;

namespace SmartiErpWin.Client.UserInterface.Marketing.MarketerEntry;

public partial class MarketerEntryWindow
{
    public MarketerPageEntryViewModel ViewModel { get; }
    
    public MarketerEntryWindow(LocalisationService localisationService,
        DtoMarketer model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new MarketerPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}