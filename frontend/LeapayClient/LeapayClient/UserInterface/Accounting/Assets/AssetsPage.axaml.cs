using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.Assets;

public partial class AssetsPage : UserControlBase
{
    public AssetsPageViewModel ViewModel { get; set; }
    
    public AssetsPage(AssetsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}