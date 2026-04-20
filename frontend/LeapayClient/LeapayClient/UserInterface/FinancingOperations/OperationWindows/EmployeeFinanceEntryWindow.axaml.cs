using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class EmployeeFinanceEntryWindow: UserControlBase
{
    public EmployeeFinanceEntryViewModel ViewModel { get; }
    
    public EmployeeFinanceEntryWindow(WindowEntryModel<RSimpleRequest> window)
    {
        ViewModel = new EmployeeFinanceEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}