using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.Banks;

public partial class BanksPage : UserControlBase
{
    public BanksPageViewModel ViewModel { get; set; }
    
    public BanksPage(BanksPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}