using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class SupplierBillEntryWindow: UserControlBase
{
    public SupplierBillEntryViewModel ViewModel { get; }
    
    public SupplierBillEntryWindow(WindowEntryModel<RSimpleRequest> window)
    {
        ViewModel = new SupplierBillEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}