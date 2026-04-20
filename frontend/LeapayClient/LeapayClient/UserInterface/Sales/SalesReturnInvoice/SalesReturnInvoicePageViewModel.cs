using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Common.ReceiptLookup;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.SalesReturnInvoice;

public partial class SellReturnInvoicesPageViewModel : BasePageViewModel<DtoSellReturnInvoice>
{
    protected override string Endpoint => "SellReturnInvoice";

    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoSellReturnInvoice>>>> ExtraButtons =>
    [
        new(MaterialIconKind.Printer, OnPrint)
    ];

    public SellReturnInvoicesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        
        ExtraText = this["ReturnSalesInvoice"];
        ExtraIcon = MaterialIconKind.AssignmentReturn;
        ExtraButtonVisibility = true;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoSellReturnInvoice> args)
    {
        EventHelper.InvokeSellReturnInvoice(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoSellReturnInvoice> args)
    {
        var entry = new SellReturnInvoiceEntryWindow(new WindowEntryModel<DtoSellReturnInvoice>(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }

    protected override async Task OnAddAsync()
    {
        var entry = new SellReturnInvoiceEntryWindow(new WindowEntryModel<DtoSellReturnInvoice>(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }

    protected async override void OnExtraButton()
    {
        var entry = new ReceiptLookupWindow(new ReceiptLookupViewModel(null, EntryType.Add));

        await ShowContentAsync(entry, options: new(this["ReturnSalesInvoice"], false, false, false, false));
    }
}