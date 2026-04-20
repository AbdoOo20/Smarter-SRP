using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.SupplyPermits;

public partial class SupplyPermitsPageViewModel : BasePageViewModel<DtoSupplyPermit>
{
    protected override string Endpoint => "SupplyPermit";
    
    public SupplyPermitsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoSupplyPermit> args)
    {
        var entry = new SupplyPermitEntryWindow(new(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()    {
        var entry = new SupplyPermitEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}