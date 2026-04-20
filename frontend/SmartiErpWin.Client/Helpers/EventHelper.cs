using System.Data;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using SmartiErpWin.Client.UserInterface.Reports.GeneralReport;

namespace SmartiErpWin.Client.Helpers;

public static class EventHelper
{
    public static event EventHandler<PrintEventArgs<DtoPurchaseInvoice>>? PurchaseInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoSalesInvoice>>? SalesInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoSellReturnInvoice>>? SellReturnInvoiceCreated;

    public static event EventHandler<PrintEventArgs<DtoSellReturnInvoice>>? PartialSalesInvoiceReturnCreated;
    
    public static event EventHandler<PrintEventArgs<DtoSellReturnInvoice>>? CompleteSalesInvoiceReturnCreated;

    public static event EventHandler<PrintEventArgs<DtoMarketerDisbursementPermit>>? MarketerDisbursementInvoiceCreated;

    public static event EventHandler<PrintEventArgs<RSimpleRequest>>? FinancialRequestCreated;
    //public static event EventHandler<string>? FinancialRequestOpenCreated;
    //public static event EventHandler<RSimpleRequest>? FinancialRequestCreated;

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

    internal static void InvokePurchaseReturnInvoice(PrintEventArgs<DtoPurchaseReturnInvoice> value)
    {
        throw new NotImplementedException();
    }

    internal static void InvokeCompleteSalesPurchesReturn(PrintEventArgs<DtoPurchaseReturnInvoice> printEventArgs)
    {
        throw new NotImplementedException();
    }
}

public record struct ReportPrintEventArgs(DataTable DataTable, StatementQueryRequest QueryRequest, GeneralReportPageViewModel ViewModel);

public record struct PrintEventArgs<T>(T Model, string Endpoint = "");