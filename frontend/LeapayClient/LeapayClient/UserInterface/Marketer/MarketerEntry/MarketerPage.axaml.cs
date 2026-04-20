using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Marketer.MarketerEntry;

public partial class MarketerPage : UserControlBase
{
    public MarketerPageViewModel ViewModel { get; set; }
    
    public MarketerPage(MarketerPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}