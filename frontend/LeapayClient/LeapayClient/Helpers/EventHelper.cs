using System.Data;
using Fly.Framework.Desktop.LicenseValidation;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.UserInterface.Reports.GeneralReport;

namespace Leapay.Client.Helpers;

public static class EventHelper
{
    public static event EventHandler<PrintEventArgs<DtoPurchaseInvoice>>? PurchaseInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoSalesInvoice>>? SalesInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoSellReturnInvoice>>? SellReturnInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoSellReturnInvoice>>? PartialSalesInvoiceReturnCreated;

    public static event EventHandler<PrintEventArgs<DtoSellReturnInvoice>>? CompleteSalesInvoiceReturnCreated;

    public static event EventHandler<PrintEventArgs<DtoMaintenanceInvoice>>? MaintenanceInvoiceCreated;
    
    public static event EventHandler<PrintEventArgs<DtoFactoringInvoice>>? FactoringInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoMarketerDisbursementPermit>>? MarketerDisbursementInvoiceCreated;
    
    public static event EventHandler<PrintEventArgs<DtoItemsTransferInvoice>>? ItemsTransferInvoiceCreated;

    public static event EventHandler<PrintEventArgs<RSimpleRequest>>? FinancialRequestCreated;
    //public static event EventHandler<string>? FinancialRequestOpenCreated;
    //public static event EventHandler<RSimpleRequest>? FinancialRequestCreated;

    // TODO: General reports need implementation
    public static event EventHandler<ReportPrintEventArgs>? ReportPrintRequested;

    public static event EventHandler<PrintEventArgs<DtoEntry>>? EntryPrintRequested;

    static EventHelper()
    {
    }

    public static void InvokePurchases(PrintEventArgs<DtoPurchaseInvoice> invoice)
    {
        PurchaseInvoiceCreated?.Invoke(null, invoice);
    }

    public static void InvokeEntry(PrintEventArgs<DtoEntry> entry)
    {
        EntryPrintRequested?.Invoke(null, entry);
    }

    public static void InvokeSales(PrintEventArgs<DtoSalesInvoice> invoice)
    {
        SalesInvoiceCreated?.Invoke(null, invoice);
    }
    public static void InvokeSellReturnInvoice(PrintEventArgs<DtoSellReturnInvoice> invoice)
    {
        SellReturnInvoiceCreated?.Invoke(null, invoice);
    }

    public static void InvokePartialSalesReturn(PrintEventArgs<DtoSellReturnInvoice> invoice)
    {
        PartialSalesInvoiceReturnCreated?.Invoke(null, invoice);
    }

    public static void InvokeCompleteSalesReturn(PrintEventArgs<DtoSellReturnInvoice> invoice)
    {
        CompleteSalesInvoiceReturnCreated?.Invoke(null, invoice);
    }

    public static void InvokeMarketerDisbursement(PrintEventArgs<DtoMarketerDisbursementPermit> permit)
    {
        MarketerDisbursementInvoiceCreated?.Invoke(null, permit);
    }

    public static void InvokeFinancial(PrintEventArgs<RSimpleRequest> request)
    {
        FinancialRequestCreated?.Invoke(null, request);
    }

    public static void InvokeFinancial2(object sender, string request)
    {
        //FinancialRequestOpenCreated?.Invoke(sender, request);
    }

    public static void InvokeReport(ReportPrintEventArgs e)
    {
        ReportPrintRequested?.Invoke(null, e);
    }
    
    public static void InvokeItemsTransferInvoice(PrintEventArgs<DtoItemsTransferInvoice> invoice)
    {
        ItemsTransferInvoiceCreated?.Invoke(null, invoice);
    }
    
    public static void InvokeMaintenanceInvoice(PrintEventArgs<DtoMaintenanceInvoice> invoice)
    {
        MaintenanceInvoiceCreated?.Invoke(null, invoice);
    }
    
    public static void InvokeFactoringInvoice(PrintEventArgs<DtoFactoringInvoice> invoice)
    {
        FactoringInvoiceCreated?.Invoke(null, invoice);
    }

    internal static void InvokePurchaseReturnInvoice(PrintEventArgs<DtoPurchaseReturnInvoice> value)
    {
        //throw new NotImplementedException();
    }

    internal static void InvokeCompleteSalesPurchesReturn(PrintEventArgs<DtoPurchaseReturnInvoice> printEventArgs)
    {
        //throw new NotImplementedException();
    }

    public static void SyncSubscribers()
    {
        var subscription = App.GetService<ISubscriberModule>();
        if (!subscription.SyncSubscription())
        {
            App.TryGetDesktopLifetime(out var lifetime);
            lifetime?.Shutdown();
        }
    }
}

public record struct ReportPrintEventArgs(DataTable DataTable, StatementQueryRequest QueryRequest, GeneralReportPageViewModel ViewModel);

public record struct PrintEventArgs<T>(T Model, string Endpoint = "");