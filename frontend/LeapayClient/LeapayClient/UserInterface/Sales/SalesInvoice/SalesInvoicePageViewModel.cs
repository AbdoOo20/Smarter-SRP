using Avalonia.Input;
using Avalonia.Platform.Storage;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Reports;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.ReceiptLookup;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.SalesInvoice;

public partial class SalesInvoicePageViewModel : BasePageViewModel<DtoSalesInvoice>
{
    protected override string Endpoint => "Sales";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoSalesInvoice>>>> ExtraButtons =>
    [
        new (MaterialIconKind.ContentCopy, OnCopy),
        new (MaterialIconKind.Printer, OnPrint),
        new (MaterialIconKind.PrinterPointOfSaleCheck, OnPrintReceipt),
        new (MaterialIconKind.Whatsapp, OnSendToWhatsapp)
    ];
    
    public SalesInvoicePageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        
        ExtraText = this["ReturnSalesInvoice"];
        ExtraIcon = MaterialIconKind.AssignmentReturn;
        ExtraButtonVisibility = true;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoSalesInvoice> args)
    {
        var entry = new SalesInvoiceEntryWindow(new WindowEntryModel<DtoSalesInvoice>(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }

    protected override async Task OnAddAsync()
    {
        var entry = new SalesInvoiceEntryWindow(new WindowEntryModel<DtoSalesInvoice>(null, EntryType.Add, GetType()));
        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
    
    private void OnPrint(object? sender, DataGridButtonEventArgs<DtoSalesInvoice> args)
    {
        EventHelper.InvokeSales(new(args.Model));
    }

    private void OnPrintReceipt(object? sender, DataGridButtonEventArgs<DtoSalesInvoice> args)
    {
        ContentHelper.GenerateReceipt(args.Model);
    }

    private async void OnSendToWhatsapp(object? sender, DataGridButtonEventArgs<DtoSalesInvoice> args)
    {
        try
        {
            if (args.Model.ClientId.IsNullOrEmpty() || string.IsNullOrEmpty(args.Model.ClientPhone))
            {
                await DialogueServices.ShowMessageAsync("Error", "CheckClientOrPhoneNumber");
                return;
            }
            
            var file = ReportRunnerHelper.GetSalesFile(new(args.Model));
            var clipboard = TopLevel.GetTopLevel(App.GetMainWindow())?.Clipboard;
            
            if (clipboard is null)
            {
                await DialogueServices.ShowMessageAsync("Error", "ContactSupport");
                return;
            }
            
            var tempFilePath = Path.Combine(Path.GetTempPath(), $"{args.Model.Number}-{args.Model.ClientPhone.Replace(" ", string.Empty).Replace("+", string.Empty).Replace("-", string.Empty)}.pdf");
            if (App.GetMainWindow().StorageProvider is not { } storageProvider)
                return;
            
            await File.WriteAllBytesAsync(tempFilePath, file);
            var fileSt = await storageProvider.TryGetFileFromPathAsync(tempFilePath);
            if (fileSt != null)
            {
                DataObject dataObject = new();
                dataObject.Set(DataFormats.Files, new List<IStorageFile> { fileSt });
                await clipboard.SetDataObjectAsync(dataObject);
            }

            await ExportServices.NavigateToWhatsAppAsync(args.Model.ClientPhone);
        }
        catch (Exception e)
        {
            await DialogueServices.ShowMessageAsync("Error", "ContactSupport");
            Error(e.ToString());
        }
    }
    
    private async void OnCopy(object? sender, DataGridButtonEventArgs<DtoSalesInvoice> args)
    {
        if (args.Model is null)
            return;
        
        var deepCopy = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(args.Model);
        deepCopy.Id = Identifier.Empty;
        
        var entry = new SalesInvoiceEntryWindow(new WindowEntryModel<DtoSalesInvoice>(deepCopy, EntryType.Add, GetType()));
        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
    
    protected async override void OnExtraButton()
    {
        var entry = new ReceiptLookupWindow(new ReceiptLookupViewModel(null, EntryType.Add));

        await ShowContentAsync(entry, options: new(this["ReturnSalesInvoice"], false, false, false, false));
    }
}