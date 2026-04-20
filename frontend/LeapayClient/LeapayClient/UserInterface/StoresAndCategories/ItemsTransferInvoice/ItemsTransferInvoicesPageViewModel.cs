using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemsTransferInvoice;

public partial class ItemsTransferInvoicesPageViewModel : BasePageViewModel<DtoItemsTransferInvoice>
{
    protected override string Endpoint => "ItemsTransferInvoice";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoItemsTransferInvoice>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];
    
    public ItemsTransferInvoicesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoItemsTransferInvoice> args)
    {
        EventHelper.InvokeItemsTransferInvoice(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoItemsTransferInvoice> args)
    {
        var entry = new ItemsTransferInvoiceEntryWindow(new(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()
    {
        var entry = new ItemsTransferInvoiceEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}