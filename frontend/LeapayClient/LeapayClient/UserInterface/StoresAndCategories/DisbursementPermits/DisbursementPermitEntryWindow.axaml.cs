using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.DisbursementPermits;

public partial class DisbursementPermitEntryWindow: UserControlBase
{
    public DisbursementPermitEntryViewModel ViewModel { get; }
    
    public DisbursementPermitEntryWindow(WindowEntryModel<DtoDisbursementPermit> window)
    {
        ViewModel = new DisbursementPermitEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}