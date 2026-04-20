using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Units;

public partial class UnitEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoUnit> ViewModel { get; }
    
    public UnitEntryWindow(WindowEntryModel<DtoUnit> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoUnit>(window.Model,
            "Unit",
            window.EntryType,
            window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}