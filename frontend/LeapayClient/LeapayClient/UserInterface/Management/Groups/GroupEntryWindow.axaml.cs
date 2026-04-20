using Avalonia.Interactivity;
using Fly.Shared.Modules.Global.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Management.Groups;

public partial class GroupEntryWindow: UserControlBase
{
    public GroupPageEntryViewModel ViewModel { get; }
    
    public GroupEntryWindow(WindowEntryModel<DtoRoleGroup> window)
    {
        ViewModel = new GroupPageEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
    
    private void OnStateChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.OnState();
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.OnSearch((sender as TextBox)?.Text);
    }
}