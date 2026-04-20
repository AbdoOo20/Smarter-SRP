using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.SupplyPermits;

public partial class SupplyPermitEntryWindow: UserControlBase
{
    public SupplyPermitEntryViewModel ViewModel { get; }
    
    public SupplyPermitEntryWindow(WindowEntryModel<DtoSupplyPermit> window)
    {
        ViewModel = new SupplyPermitEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}