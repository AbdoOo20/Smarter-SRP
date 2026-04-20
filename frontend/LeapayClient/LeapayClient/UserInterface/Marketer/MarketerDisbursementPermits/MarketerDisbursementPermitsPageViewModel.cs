using Fly.Shared.Modules.Marketers.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Marketer.MarketerDisbursementPermits;

public partial class MarketerDisbursementPermitsPageViewModel : BasePageViewModel<DtoMarketerDisbursementPermit>
{
    protected override string Endpoint => "MarketerDisbursementPermit";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoMarketerDisbursementPermit>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];
    
    public MarketerDisbursementPermitsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoMarketerDisbursementPermit> args)
    {
        EventHelper.InvokeMarketerDisbursement(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoMarketerDisbursementPermit> args)
    {
        var entry = new MarketerDisbursementPermitEntryWindow(new(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()    {
        var entry = new MarketerDisbursementPermitEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}