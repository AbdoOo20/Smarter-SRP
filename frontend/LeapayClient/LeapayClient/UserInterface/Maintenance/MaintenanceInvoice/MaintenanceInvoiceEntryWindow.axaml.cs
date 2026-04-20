using Avalonia.Interactivity;
using Fly.Shared.Modules.Maintenance.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;

public partial class MaintenanceInvoiceEntryWindow: UserControlBase
{
    public MaintenanceInvoiceEntryViewModel ViewModel { get; }
    
    public MaintenanceInvoiceEntryWindow(WindowEntryModel<DtoMaintenanceInvoice> window)
    {
        ViewModel = new MaintenanceInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        Init();
    }
    
    public MaintenanceInvoiceEntryWindow(MaintenanceInvoiceEntryViewModel vm)
    {
        ViewModel = vm;
        Init();
    }
    
    private void Init()
    {
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.Costs.ShowData(SecondaryDataGrid);
        SynchronisationModel = ViewModel;

        ViewModel.ItemInfoBar = ItemInfoBar;
    }

    private void InternalMaintenanceCheckedChanged(object? sender, RoutedEventArgs e)
    {
        ViewModel.OnInternalMaintenanceCheckChanged((sender as CheckBox)?.IsChecked ?? false);
    }
}