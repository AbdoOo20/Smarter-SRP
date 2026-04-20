using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class AccountEntryWindow: UserControlBase
{
    public AccountEntryViewModel ViewModel { get; }
    
    public AccountEntryWindow(WindowEntryModel<RSimpleRequest> window)
    {
        ViewModel = new AccountEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}