using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.Safes;

public partial class SafeEntryWindow: UserControlBase
{
    public SafeEntryViewModel ViewModel { get; }
    
    public SafeEntryWindow(WindowEntryModel<DtoSafe> window)
    {
        ViewModel = new SafeEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}