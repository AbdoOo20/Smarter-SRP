using Fly.Shared.Modules.Global.Dtos;

namespace SmartiErpWin.Client.UserInterface.Management.Users;

public partial class UserEntryWindow
{
    public UserPageEntryViewModel ViewModel { get; }
    
    public UserEntryWindow(LocalisationService localisationService,
        DtoUser model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new UserPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}