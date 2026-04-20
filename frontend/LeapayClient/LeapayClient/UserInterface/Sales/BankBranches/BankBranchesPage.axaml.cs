using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.BankBranches;

public partial class BankBranchesPage : UserControlBase
{
    public BankBranchesPageViewModel ViewModel { get; set; }
    
    public BankBranchesPage(BankBranchesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}