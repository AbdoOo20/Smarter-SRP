using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Stores.Partials;

public partial class MoveItemsEntryWindow: UserControlBase
{
    public MoveItemsEntryViewModel ViewModel { get; }
    
    public MoveItemsEntryWindow(WindowEntryModel<MoveItemsRequest> window)
    {
        ViewModel = new MoveItemsEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}