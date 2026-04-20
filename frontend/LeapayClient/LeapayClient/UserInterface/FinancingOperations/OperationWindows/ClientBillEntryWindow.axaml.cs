using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class ClientBillEntryWindow: UserControlBase
{
    public ClientBillEntryViewModel ViewModel { get; }
    
    public ClientBillEntryWindow(WindowEntryModel<RSimpleRequest> window)
    {
        ViewModel = new ClientBillEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}