using System.Collections;
using System.Data;
using System.Diagnostics;
using Fly.Framework.Shared.Extentions;
using Leapay.Client.Helpers;
using Leapay.Client.Reports.Fixed.Models;
using Microsoft.Reporting.NETCore;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.Reports.Fixed;

internal static class FixedStyleReport
{

    #region Tools


    public static FileInfo SavePdf(byte[] file)
    {
        var fileName = Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Reports", fileName);
        var fileInfo = new FileInfo(path);
        fileInfo.Directory.Create();
        using (var dtr = fileInfo.OpenWrite())
        {
            dtr.Write(file, 0, file.Length);
        }
        return fileInfo;
    }


    public static void OpenAndSavePdf(byte[] file)
    {
        var fileInfo = SavePdf(file);
        OpenPdf(fileInfo);
    }


    public static void OpenPdf(FileInfo fileInfo)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileInfo.FullName,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }


    #endregion

    public static decimal Sanitize(string input)
    {
        return decimal.TryParse(input.Replace("_", string.Empty).Replace("+", string.Empty),
            out var dec) ? dec : 0;
    }
    
    #region Reports

    public static void AccountMovementsReport(Leapay.Client.Helpers.ReportPrintEventArgs e)
    {
        var reportName = e.DataTable.Columns.Count == 8 ? "AccountMovements" : "AccountMovements2";
        string? accName = null;
        var data = new List<AccountMovementsReportModel>();
        foreach (DataRow item in e.DataTable.Rows)
        {
            if (e.DataTable.Columns.Count == 9)
            {
                data.Add(new AccountMovementsReportModel()
                {
                    AccountName = item[0]?.ToString(),
                    SerialNumber = item[1] as int? ?? 0,
                    Date = item[3]?.ToString() ?? "",
                    Movement = item[4]?.ToString() ?? "",
                    Debit = Sanitize(item[5].ToString() ?? "0"),
                    Credit = Sanitize(item[6].ToString() ?? "0"),
                    Balance = Sanitize(item[7].ToString() ?? "0"),
                });
                accName = e.ViewModel?.ComboModelBase?.Api1?.Display == "..." ? null : e.ViewModel?.ComboModelBase?.Api1?.Display;
            }
                
            else
            {
                data.Add(new AccountMovementsReportModel()
                {
                    SerialNumber = item[1] as int? ?? 0,
                    Date = item[2]?.ToString() ?? "",
                    Movement = item[3]?.ToString() ?? "",
                    Debit = Sanitize(item[4].ToString() ?? "0"),
                    Credit = Sanitize(item[5].ToString() ?? "0"),
                    Balance = Sanitize(item[6].ToString() ?? "0"),
                });
                accName = e.ViewModel.Data?.Uppers?.FirstOrDefault().Value ?? "";
            }
        }
        data.RemoveAt(data.Count - 1);
        StartReport(reportName: reportName, values: data, reportParameters:
        [
            new("ReportParameter_fromDate", e.QueryRequest.StartDate.ToDottedDate()),
            new("ReportParameter_title", "التقرير الشامل"),
            new("ReportParameter_toDate", e.QueryRequest.EndDate.ToDottedDate()),
            new("ReportParameter_account", accName)
        ]);
    }

    public static void TheClientDebtionReport(Leapay.Client.Helpers.ReportPrintEventArgs e)
    {
        var reportName = "TheClientDebtion";
        string? accName = "";
        string? totamamount = "";
        var data = new List<TheClientDebtionReportModel>();
        if (e.DataTable.Columns.Count == 10)
        {
            foreach (DataRow item in e.DataTable.Rows)
            {
                data.Add(new TheClientDebtionReportModel()
                {
                    AccountName = item[0]?.ToString(),
                    Balance = Sanitize(item[1].ToString() ?? "0"),
                    LastPaymentAmount = Sanitize(item[2].ToString() ?? "0"),
                    LastPaymentDate = item[3]?.ToString(),
                    LastPaymentType = item[4]?.ToString(),
                    LastInvoiceAmount = Sanitize(item[6].ToString() ?? "0"),
                    LastInvoiceDate = item[7]?.ToString(),
                    LastInvoiceType = item[8]?.ToString(),
                });
            }
        }
        else
        {
            foreach (DataRow item in e.DataTable.Rows)
            {
                data.Add(new TheClientDebtionReportModel()
                {
                    Balance = Sanitize(item[0].ToString() ?? "0"),
                    LastPaymentAmount = Sanitize(item[1].ToString() ?? "0"),
                    LastPaymentDate = item[2]?.ToString(),
                    LastPaymentType = item[3]?.ToString(),
                    LastInvoiceAmount = Sanitize(item[5].ToString() ?? "0"),
                    LastInvoiceDate = item[6]?.ToString(),
                    LastInvoiceType = item[7]?.ToString(),
                });
            }
        }
        try
        {
            accName = e.ViewModel.DynamicCombos[0].SelectedCombo.Display;
            //accName = e.ViewModel.DynamicCombos[1].SelectedCombo.Display;
        }
        catch { }
        try
        {
            totamamount = e.ViewModel.DynamicFields[0].Value.ToString();
        }
        catch { }
        StartReport(reportName: reportName, values: data, reportParameters: new List<ReportParameter>()
        {
            new ReportParameter("ReportParameter_ammount", totamamount),
            new ReportParameter("ReportParameter_account",  accName),
            new("ReportParameter_title", "ديون العملاء"),
        });
    }

    #endregion

    public static void StartReport(Leapay.Client.Helpers.ReportPrintEventArgs e)
    {
        try
        {
            if (e.QueryRequest.ReportName == "AccountMovements")
            {
                AccountMovementsReport(e);
                return;
            }
            else if (e.QueryRequest.ReportName == "TheClientDebtion")
            {
                TheClientDebtionReport(e);
                return;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static void StartReport(string reportName, IEnumerable values, List<ReportParameter> reportParameters)
    {
        try
        {
            Stream reportDefinition = new StreamReader($"./Reports/Fixed/" + reportName + ".rdlc").BaseStream;
            LocalReport report = new LocalReport();
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);
            reportParameters.Add(new ReportParameter("PrintTime", CommonTextHelpers.GetPrintTime()));

            report.DataSources.Add(new ReportDataSource("DataSet1", values));
            report.DataSources.Add(new ReportDataSource("LuncherInfo", DataHelper.GetCompanyInfo()));
            report.SetParameters(reportParameters);

            byte[] pdf = report.Render("PDF");
            OpenAndSavePdf(pdf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

}