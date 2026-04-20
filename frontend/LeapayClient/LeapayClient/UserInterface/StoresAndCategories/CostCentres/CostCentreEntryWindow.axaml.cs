using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.CostCentres;

public partial class CostCentreEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoCostCenter> ViewModel { get; }
    
    public CostCentreEntryWindow(WindowEntryModel<DtoCostCenter> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoCostCenter>(window.Model,
            "CostCenter",
            window.EntryType,
            window.PageType);

        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}