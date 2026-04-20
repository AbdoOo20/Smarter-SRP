using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.CostCentres;

public partial class CostCentresPageViewModel : BasePageViewModel<DtoCostCenter>
{
    protected override string Endpoint => "CostCenter";
    
    public CostCentresPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoCostCenter> args)
    {
        var entry = new CostCentreEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new CostCentreEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}