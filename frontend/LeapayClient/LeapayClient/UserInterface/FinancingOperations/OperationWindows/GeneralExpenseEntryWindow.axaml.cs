using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class GeneralExpenseEntryWindow: UserControlBase
{
    public GeneralExpenseEntryViewModel ViewModel { get; }
    
    public GeneralExpenseEntryWindow(WindowEntryModel<RSimpleRequest> window)
    {
        ViewModel = new GeneralExpenseEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}