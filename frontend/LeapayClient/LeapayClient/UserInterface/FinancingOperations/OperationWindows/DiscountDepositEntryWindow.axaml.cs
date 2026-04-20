using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class DiscountDepositEntryWindow: UserControlBase
{
    public DiscountDepositFinanceEntryViewModel ViewModel { get; }
    
    public DiscountDepositEntryWindow(WindowEntryModel<RSimpleRequest> window)
    {
        ViewModel = new DiscountDepositFinanceEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}