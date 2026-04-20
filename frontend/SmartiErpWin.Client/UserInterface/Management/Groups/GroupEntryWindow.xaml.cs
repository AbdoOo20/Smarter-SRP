using System.Windows.Controls;
using Fly.Shared.Modules.Global.Dtos;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace SmartiErpWin.Client.UserInterface.Management.Groups;

public partial class GroupEntryWindow
{
    public GroupEntryViewModel ViewModel { get; }
    
    public GroupEntryWindow(LocalisationService localisationService,
        DtoRoleGroup model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new GroupEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
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