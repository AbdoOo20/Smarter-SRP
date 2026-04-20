using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.Banks;

public partial class BankEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoBank> ViewModel { get; }
    
    public BankEntryWindow(WindowEntryModel<DtoBank> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoBank>(window.Model,
            "Bank",
            window.EntryType,
            window.PageType);
        SynchronisationModel = ViewModel;

        ViewModel.AddValidation(() => ViewModel.Model.Name, optional: false);
        
        InitializeComponent();
        DataContext = this;
    }
}