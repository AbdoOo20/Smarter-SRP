using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Suppliers;

public partial class SupplierEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoSupplier> ViewModel { get; }
    
    public SupplierEntryWindow(WindowEntryModel<DtoSupplier> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoSupplier>(window.Model,
            "Supplier",
            window.EntryType,
            window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}