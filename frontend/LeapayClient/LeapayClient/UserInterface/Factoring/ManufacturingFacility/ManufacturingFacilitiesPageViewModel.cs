using Fly.Shared.Modules.Factoring.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Factoring.ManufacturingFacility;

public partial class ManufacturingFacilitiesPageViewModel : BasePageViewModel<DtoManufacturingFacility>
{
    protected override string Endpoint => "ManufacturingFacility";
    
    public ManufacturingFacilitiesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoManufacturingFacility> args)
    {
        var entry = new ManufacturingFacilityEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new ManufacturingFacilityEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}