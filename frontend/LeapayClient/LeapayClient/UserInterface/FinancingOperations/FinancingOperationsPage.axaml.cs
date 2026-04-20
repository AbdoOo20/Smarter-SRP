using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.FinancingOperations;

public partial class FinancingOperationsPage : UserControlBase
{
    public FinancingOperationsPageViewModel ViewModel { get; set; }
    
    public FinancingOperationsPage(FinancingOperationsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
    }
}