using Fly.Shared.Modules.Financial.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class DiscountItemFinanceEntryWindow: UserControlBase
{
    public DiscountItemFinanceEntryViewModel ViewModel { get; }
    
    public DiscountItemFinanceEntryWindow(WindowEntryModel<DiscountItemBasedRequest> window)
    {
        ViewModel = new DiscountItemFinanceEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}