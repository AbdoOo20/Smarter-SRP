using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Items;

public partial class ItemEntryWindow: UserControlBase
{
    public ItemEntryViewModel ViewModel { get; }
    
    public ItemEntryWindow(WindowEntryModel<CreateUpdateItemRequest> window)
    {
        ViewModel = new ItemEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
        ViewModel.ShowData(DataGrid);
        ViewModel.Barcodes.ShowData(BarcodesDataGrid);
    }
}