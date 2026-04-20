using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.Entries;

public partial class EntryPage : UserControlBase
{
    public EntryPageViewModel ViewModel { get; set; }
    
    public EntryPage(EntryPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}