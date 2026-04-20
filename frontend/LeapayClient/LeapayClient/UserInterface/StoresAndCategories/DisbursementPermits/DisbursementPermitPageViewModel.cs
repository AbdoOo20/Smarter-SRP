using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.DisbursementPermits;

public partial class DisbursementPermitsPageViewModel : BasePageViewModel<DtoDisbursementPermit>
{
    protected override string Endpoint => "DisbursementPermit";
    
    public DisbursementPermitsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoDisbursementPermit> args)
    {
        var entry = new DisbursementPermitEntryWindow(new(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()    {
        var entry = new DisbursementPermitEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}