using Fly.Shared.Modules.Factoring.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Factoring.ManufacturingFacility;

public partial class ManufacturingFacilityEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoManufacturingFacility> ViewModel { get; }
    
    public ManufacturingFacilityEntryWindow(WindowEntryModel<DtoManufacturingFacility> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoManufacturingFacility>(window.Model,
            "ManufacturingFacility",
            window.EntryType,
            window.PageType);

        ViewModel.AddValidation(() => ViewModel.Model.Name, optional: false);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}