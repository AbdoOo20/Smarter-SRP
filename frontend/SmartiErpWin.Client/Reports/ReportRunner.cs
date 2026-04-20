using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using SmartErpWin.Core.Helpers;
using SmartiErpWin.Client.DtoModels.Reports;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;
using SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;
using SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;
using System.Collections;
using System.IO;
using SmartErpWin.Core;

namespace SmartiErpWin.Client.Reports;

/// <summary>
/// Not Working
/// </summary>
public class ReportRunner
{
    private readonly string _reportName;
    private readonly IEnumerable _data;

    public ReportRunner(string reportName, IEnumerable data)
    {
        _data = data;
        _reportName = reportName;
    }

    public static void PreoarwReportRunner()
    {
        //EventHelper.SalesInvoiceCreated += OnSalesInvoiceCreated;
        //EventHelper.PurchaseInvoiceCreated += OnPurchaseInvoiceCreated;
        EventHelper.PartialSalesInvoiceReturnCreated += EventHelper_PartialSalesInvoiceReturnCreated;
        EventHelper.CompleteSalesInvoiceReturnCreated += EventHelper_CompleteSalesInvoiceReturnCreated;
    }

    private static void EventHelper_PartialSalesInvoiceReturnCreated(object? sender, PrintEventArgs<DtoSellReturnInvoice> e)
    {
        var sellInvoice = e.Model.SellInvoice;
        ReportRunner.LoadSaleInvoiceReport(sellInvoice);
    }

    private static void EventHelper_CompleteSalesInvoiceReturnCreated(object? sender, PrintEventArgs<DtoSellReturnInvoice> e)
    {
        var sellInvoice = e.Model.SellInvoice;
        ReportRunner.LoadSaleInvoiceReport(sellInvoice);
    }

    private static void OnSalesInvoiceCreated(object? sender, PrintEventArgs<DtoSalesInvoice> e)
    {

    }

    private static void OnPurchaseInvoiceCreated(object? sender, PrintEventArgs<DtoPurchaseInvoice> e)
    {

    }


    /// <summary>
    /// "state", dataSource[0]  
    /// "date", dataSource[1]   
    /// "others", dataSource[2] 
    /// "amount", dataSource[3] 
    /// "number", dataSource[4] 
    /// "notes", dataSource[5] 
    /// "RemainText", dataSource[6] 
    /// "RemainAmount", dataSource[7] 
    /// "ppText", dataSource[7] 
    /// </summary>
    public static void LoadStatmentReport(string[] dataSource, string reportName = "MovementReceipt")
    {

        Stream reportDefinition = new StreamReader($"./Reports/{reportName}.rdlc").BaseStream;

        LocalReport report = new LocalReport();
        report.LoadReportDefinition(reportDefinition);
        //report.DataSources.Add(new ReportDataSource("source", dataSource));

        var parameters = new[]
        {
            new ReportParameter("state", dataSource[0]  ),
            new ReportParameter("date", dataSource[1]   ),
            new ReportParameter("others", dataSource[2] ),
            new ReportParameter("amount", dataSource[3] ),
            new ReportParameter("number", dataSource[4] ),
            new ReportParameter("notes", dataSource[5] ),
            new ReportParameter("remainText", dataSource[6] ),
            new ReportParameter("remainAmount", dataSource[7] ),
            new ReportParameter("ppText", dataSource[8] ),
            new ReportParameter("textAmount", dataSource[9] ),
        };

        report.SetParameters(parameters);
        byte[] pdf = report.Render("PDF");

        ReportRunnerHelper.OpenAndSavePdf(pdf);
    }

    [Obsolete]
    public static void OnSalesComplate(SalesInvoiceEntryViewModel viewModel, DtoSalesInvoice? data)
    {
        try
        {
            var ddd = data.Details.Select((s, indx) => new SalesInvoiceDetailDto
            {
                ExpireDate = s.ExpireDate,
                ItemName = s.ItemName,
                ItemNumber = s.ItemBarcode,
                Price = s.Price,
                Quantity = s.Quantity,
                Total = (s.Price * s.Quantity) - s.Discount,
                Serial = (indx + 1),
                UnitName = s.UnitName,
            }).ToList();


            var saleInvoice = data; // viewModel.Model.DtoSalesInvoice;
            Stream reportDefinition = new StreamReader($"./Reports/SaleInvoiceReport.rdlc").BaseStream;
            LocalReport report = new LocalReport();
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);

            report.DataSources.Add(new ReportDataSource("DataSet1", ddd));
            report.DataSources.Add(new ReportDataSource("DataSet2", new List<DtoSalesInvoice>() { data }));
            
            report.DataSources.Add(new ReportDataSource("LuncherInfo", new List<ClientInfo> { Config.Get.ClientConfig }));

            byte[] pdf = report.Render("PDF");
            ReportRunnerHelper.OpenPdf(ReportRunnerHelper.SavePdf(pdf));
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "OnSalesComplate");
            Console.WriteLine(ex.ToString());
        }
    }

    [Obsolete]
    public static void OnPurchesComplate(PurchasesInvoiceEntryViewModel viewModel)
    {

        MessageBox.Show("OnPurchesComplate");

    }

    public static void LoadSaleInvoiceReport(DtoSalesInvoice dataSource)
    {

        Stream reportDefinition = new StreamReader("./Reports/SaleInvoiceReport.rdlc").BaseStream;
        LocalReport report = new LocalReport();
        report.LoadReportDefinition(reportDefinition);
        var data = dataSource.Details.Select(s =>
        new
        { 
            ItemName = s.ItemName, UnitName = s.UnitName, Quntity = s.Quantity, Price = s.Price, Total = s.Price * s.Quantity }
        ).ToList();
        report.DataSources.Add(new ReportDataSource("dataset1", dataSource.Details));

        var parameters = new[]
        {
            new ReportParameter("total", dataSource.TotalAmount.ToString()),
        };

        report.SetParameters(parameters);
        byte[] pdf = report.Render("PDF");

        ReportRunnerHelper.OpenAndSavePdf(pdf);
    }

    internal static void LoadStatementReport(object value)
    {
    }

    // **********************************

    private static bool PrintNewStyle<T>(FinanceOperationViewModelBase<T> viewModel)
        where T : RSimpleRequest, new()
    {
        var response = viewModel.Model.ApiResponse as DtoStatment;
        if (!string.IsNullOrEmpty(response.Text64BaseString))
        {
            var bytes = Convert.FromBase64String(response.Text64BaseString);
            ReportRunnerHelper.OpenAndSavePdf(bytes);
            return true;
        }
        return false;
    }

    // **********************************

    public static void GeneralBill(string receiptType, DateTime dateTime, string others, decimal amount, string number, string notes, string remainText, string remainAmount, string statusText)
    {
        LoadStatmentReport(new string[]
        {
                receiptType,
                dateTime.ToDashedDate(),
                others,
                amount.ToDottedCommaedNumber(),
                number,
                notes,
                remainText,
                remainAmount,
                statusText,
                CommonTextHelpers.GetText(amount),
        });
    }

    //
    //  F1
    public static void PrintAccountEntry(bool isDeposit, AccountEntryViewModel model)
    {
        if (PrintNewStyle(model))
            return;

        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        // var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");

    }

    //
    //  F2
    internal static void PrintClientBill(bool isDeposit, ClientBillEntryViewModel model, string? data)
    {
        if (PrintNewStyle(model))
            return;

        var obj = JsonConvert.DeserializeObject<DtoStatment>(data);
        if (!string.IsNullOrEmpty(obj.Text64BaseString))
        {
            var bytes = Convert.FromBase64String(obj.Text64BaseString);
            ReportRunnerHelper.OpenAndSavePdf(bytes);
            return;
        }
        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        // var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");
    }

    //
    //  F3
    internal static void PrintupplierBill(bool isDeposit, SupplierBillEntryViewModel model, string? data)
    {
        if (PrintNewStyle(model))
            return;


        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        // var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");
    }

    //
    //  F4
    internal static void PrintEmployee(bool isDeposit, EmployeeFinanceEntryViewModel model)
    {
        if (PrintNewStyle(model))
            return;

        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        //var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");
    }

    //
    //  F5
    internal static void PrintGeneralExpense(GeneralExpenseEntryViewModel model)
    {
        if (PrintNewStyle(model))
            return;

        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var number = model.Model.Number.ToString("D8");
        var notes = " ";
        var remainText = " ";
        var RemainAmount = " ";
        LoadStatmentReport(new string[]
        {
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now.ToString("yyyy.MM.dd - HH:mm"),
                others,
                model.Model?.Amount.ToString("###,##0.00"),
                number,
                notes,
                remainText,
                RemainAmount,
                isDeposit ? "استلمت من" : "استلم"
        });
    }

    //
    //  F6
    internal static void PrintDiscountDeposit(bool isDeposit, DiscountDepositFinanceEntryViewModel model)
    {
        if (PrintNewStyle(model))
            return;

        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        //var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");
    }

    //
    //  F7
    internal static void PrintDiscountItem(bool isDeposit, DiscountItemFinanceEntryViewModel model)
    {
        if (PrintNewStyle(model))
            return;

        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        //var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");
    }

    //
    //  F8
    internal static void PrintClientRepresentativeReceipt(bool isDeposit, ClientRepresentativeReceiptEntryViewModel model)
    {
        if (PrintNewStyle(model))
            return;

        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        // var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var remainAmount = model.Model.RemainAmount;
        GeneralBill(
                !isDeposit ? "ايصال قبض" : "سند صرف",
                model.Model.Date,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                remainAmount.ToDottedCommaedNumber(),
                !isDeposit ? "استلمت من" : "استلم");
    }

    internal static void PrintMarketer(bool isDeposit, MarketerFinanceEntryViewModel model)
    {
        var from = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 0 : 1]?.Display;
        var to = model.ComboModelBase.SelectedApiComboItems[model.ComboModelBase.AssertThatStaticIsAt() ? 1 : 0]?.Display;
        // var isDeposit = model.ComboModelBase.AssertThatStaticIsAt();
        var others = isDeposit ? from : to;
        var amount = model.Model.Amount;
        var notes = model.Model.Notes;
        var remainText = model.Model.RemainText;
        var RemainAmount = model.Model.RemainAmount;
        var response = model.Model.ApiResponse as DtoStatment;
        GeneralBill(
                isDeposit ? "ايصال قبض" : "سند صرف",
                DateTime.Now,
                others,
                amount,
                model.Model.Number.ToString(),
                notes,
                remainText,
                RemainAmount.ToDottedCommaedNumber(),
                isDeposit ? "استلمت من" : "استلم");
    }

}