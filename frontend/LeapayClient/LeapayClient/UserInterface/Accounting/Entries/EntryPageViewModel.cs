using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.Entries;

public partial class EntryPageViewModel : BasePageViewModel<DtoEntry>
{
    protected override string Endpoint => "Entry";
    
    /*protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoEntry>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];*/
    
    public EntryPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoEntry> args)
    {
        EventHelper.InvokeEntry(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoEntry> args)
    {
        var entry = new EntriesEntryWindow(new(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, EntryType.Edit);
    }
    
    protected override async Task OnAddAsync() {
        var entry = new EntriesEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry);
    }
}