using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.BankBranches;

public partial class BankBranchEntryWindow: UserControlBase
{
    public BankBranchEntryViewModel ViewModel { get; }
    
    public BankBranchEntryWindow(WindowEntryModel<DtoBankBranch> window)
    {
        ViewModel = new BankBranchEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}