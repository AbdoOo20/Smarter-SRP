using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Stores.DamagedPartial;

public partial class DamageItemsEntryWindow: UserControlBase
{
    public DamageItemsEntryViewModel ViewModel { get; }
    
    public DamageItemsEntryWindow(WindowEntryModel<DamageItemsRequest> window)
    {
        ViewModel = new DamageItemsEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}