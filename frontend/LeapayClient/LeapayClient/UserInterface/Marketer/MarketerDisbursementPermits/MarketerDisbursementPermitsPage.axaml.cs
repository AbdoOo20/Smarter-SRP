using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Marketer.MarketerDisbursementPermits;

public partial class MarketerDisbursementPermitsPage : UserControlBase
{
    public MarketerDisbursementPermitsPageViewModel ViewModel { get; set; }
    
    public MarketerDisbursementPermitsPage(MarketerDisbursementPermitsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}