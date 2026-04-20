using Fly.Shared.Modules.Maintenance.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;

public partial class MaintenanceInvoicesPageViewModel : BasePageViewModel<DtoMaintenanceInvoice>
{
    protected override string Endpoint => "MaintenanceInvoice";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoMaintenanceInvoice>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];
    
    public MaintenanceInvoicesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoMaintenanceInvoice> args)
    {
        EventHelper.InvokeMaintenanceInvoice(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoMaintenanceInvoice> args)
    {
        var entry = new MaintenanceInvoiceEntryWindow(new WindowEntryModel<DtoMaintenanceInvoice>(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()
    {
        var entry = new MaintenanceInvoiceEntryWindow(new WindowEntryModel<DtoMaintenanceInvoice>(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}