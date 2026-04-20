using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Stores;

public partial class StoreEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoStore> ViewModel { get; }
    
    public StoreEntryWindow(WindowEntryModel<DtoStore> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoStore>(window.Model,
            "Store",
            window.EntryType,
            window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}