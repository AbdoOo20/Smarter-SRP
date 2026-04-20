using Fly.Shared.Modules.Financial.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationWindows;

public partial class MarketerFinanceEntryWindow: UserControlBase
{
    public MarketerFinanceEntryViewModel ViewModel { get; }
    
    public MarketerFinanceEntryWindow(WindowEntryModel<MarketerPercentRequest> window)
    {
        ViewModel = new MarketerFinanceEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
    
    private void MarketerSelected(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetMarketerInfo();
    }
}