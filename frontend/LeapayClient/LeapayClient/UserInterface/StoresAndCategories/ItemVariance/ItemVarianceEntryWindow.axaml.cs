using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemVariance;

public partial class ItemVarianceEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoItemVariance> ViewModel { get; }
    
    public ItemVarianceEntryWindow(WindowEntryModel<DtoItemVariance> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoItemVariance>(window.Model,
            "ItemVariance",
            window.EntryType,
            window.PageType);
        
        InitializeComponent();
        DataContext = this;
    }
}