using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Common.ReceiptLookupPurchase;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoicesPageViewModel : BasePageViewModel<DtoPurchaseReturnInvoice>
{
    protected override string Endpoint => "PurchaseReturnInvoice";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoPurchaseReturnInvoice>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];
    
    public PurchaseReturnInvoicesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        
        ExtraText = this["ReturnPurchaseInvoice"];
        ExtraIcon = MaterialIconKind.AssignmentReturn;
        ExtraButtonVisibility = true;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoPurchaseReturnInvoice> args)
    {
        EventHelper.InvokePurchaseReturnInvoice(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoPurchaseReturnInvoice> args)
    {
        var entry = new PurchaseReturnInvoiceEntryWindow(new WindowEntryModel<DtoPurchaseReturnInvoice>(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()
    {
        var entry = new PurchaseReturnInvoiceEntryWindow(new WindowEntryModel<DtoPurchaseReturnInvoice>(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
    
    protected async override void OnExtraButton()
    {
        var entry = new PurchaseReceiptLookupWindow(new PurchaseReceiptLookupViewModel(null, EntryType.Add));

        await ShowContentAsync(entry, options: new(this["ReturnPurchaseInvoice"], false, false, false, false));
    }
}