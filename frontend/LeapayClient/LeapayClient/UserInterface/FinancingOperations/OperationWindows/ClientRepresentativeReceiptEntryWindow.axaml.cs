using Fly.Shared.Modules.Financial.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class ClientRepresentativeReceiptEntryWindow: UserControlBase
{
    public ClientRepresentativeReceiptEntryViewModel ViewModel { get; }
    
    public ClientRepresentativeReceiptEntryWindow(WindowEntryModel<RepresentativeFinancialRequest> window)
    {
        ViewModel = new ClientRepresentativeReceiptEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}