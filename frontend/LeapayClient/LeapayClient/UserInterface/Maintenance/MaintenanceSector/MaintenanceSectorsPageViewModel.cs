using Fly.Shared.Modules.Maintenance.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceSector;

public partial class MaintenanceSectorsPageViewModel : BasePageViewModel<DtoMaintenanceSector>
{
    protected override string Endpoint => "MaintenanceSector";
    
    public MaintenanceSectorsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoMaintenanceSector> args)
    {
        var entry = new MaintenanceSectorEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()    {
        var entry = new MaintenanceSectorEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}