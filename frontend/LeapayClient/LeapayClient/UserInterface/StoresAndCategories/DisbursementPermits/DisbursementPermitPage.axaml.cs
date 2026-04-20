using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.DisbursementPermits;

public partial class DisbursementPermitsPage : UserControlBase
{
    public DisbursementPermitsPageViewModel ViewModel { get; set; }
    
    public DisbursementPermitsPage(DisbursementPermitsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}