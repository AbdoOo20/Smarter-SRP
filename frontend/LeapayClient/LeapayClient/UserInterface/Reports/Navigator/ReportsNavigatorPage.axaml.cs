using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Reports.Navigator;

public partial class ReportsNavigatorPage : UserControlBase
{
    public ReportsNavigatorPageViewModel ViewModel { get; set; }
    
    public ReportsNavigatorPage(ReportsNavigatorPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
    }
}