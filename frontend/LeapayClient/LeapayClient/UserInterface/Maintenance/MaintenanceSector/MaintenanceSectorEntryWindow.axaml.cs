using Fly.Shared.Modules.Maintenance.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceSector;

public partial class MaintenanceSectorEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoMaintenanceSector> ViewModel { get; }
    
    public MaintenanceSectorEntryWindow(WindowEntryModel<DtoMaintenanceSector> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoMaintenanceSector>(window.Model,
            "MaintenanceSector",
            window.EntryType,
            window.PageType);

        ViewModel.AddValidation(() => ViewModel.Model.Name, optional: false);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}