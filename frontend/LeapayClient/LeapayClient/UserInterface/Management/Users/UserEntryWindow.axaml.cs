using Fly.Shared.Modules.Global.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Management.Users;

public partial class UserEntryWindow: UserControlBase
{
    public UserPageEntryViewModel ViewModel { get; }
    
    public UserEntryWindow(WindowEntryModel<DtoUser> window)
    {
        ViewModel = new UserPageEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}