using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Common.ReceiptLookupPurchase;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Purchases.PurchasesInvoice;

public partial class PurchaseInvoicesPageViewModel : BasePageViewModel<DtoPurchaseInvoice>
{
    protected override string Endpoint => "Purchase";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoPurchaseInvoice>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];
    
    public PurchaseInvoicesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        
        ExtraText = this["ReturnPurchaseInvoice"];
        ExtraIcon = MaterialIconKind.AssignmentReturn;
        ExtraButtonVisibility = true;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoPurchaseInvoice> args)
    {
        EventHelper.InvokePurchases(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoPurchaseInvoice> args)
    {
        var entry = new PurchaseInvoiceEntryWindow(new WindowEntryModel<DtoPurchaseInvoice>(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()
    {
        var entry = new PurchaseInvoiceEntryWindow(new WindowEntryModel<DtoPurchaseInvoice>(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
    
    protected async override void OnExtraButton()
    {
        var entry = new PurchaseReceiptLookupWindow(new PurchaseReceiptLookupViewModel(null, EntryType.Add));

        await ShowContentAsync(entry, options: new(this["ReturnPurchaseInvoice"], false, false, false, false));
    }
}