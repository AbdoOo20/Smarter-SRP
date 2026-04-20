using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemCompositions;

public partial class ItemCompositionEntryWindow: UserControlBase
{
    public ItemCompositionEntryViewModel ViewModel { get; }
    
    public ItemCompositionEntryWindow(WindowEntryModel<DtoItemComposition> window)
    {
        ViewModel = new ItemCompositionEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}