using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemsTransferInvoice;

public partial class ItemsTransferInvoiceEntryWindow: UserControlBase
{
    public ItemsTransferInvoiceEntryViewModel ViewModel { get; }
    
    public ItemsTransferInvoiceEntryWindow(WindowEntryModel<DtoItemsTransferInvoice> window)
    {
        ViewModel = new ItemsTransferInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        
        InitializeComponent();
        DataContext = this;
        SynchronisationModel = ViewModel;

        ViewModel.ShowData(DataGrid);
        
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}