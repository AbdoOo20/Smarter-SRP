using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Microsoft.Reporting.NETCore;
using System.Diagnostics;
using System.IO;
using SmartiErpWin.Client.Helpers;
using System.Data;
using SmartiErpWin.Client.DtoModels.Reports;
using SmartiErpWin.Client.UserInterface.Reports.GeneralReport;
using Mapster;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Controls;
using System.Xml;
using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.Reports.Fixed;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using SmartErpWin.Core;
using SmartErpWin.Core.Helpers;

namespace SmartiErpWin.Client.Reports;

/// <summary>
/// Working Reporting Service
/// </summary>
public static class ReportRunnerHelper
{

    public static void RegisterEvents()
    {
        // Working ...
        EventHelper.SalesInvoiceCreated += OnSalesInvoiceCreated;
        EventHelper.ReportPrintRequested += ReportPrintRequested;
        EventHelper.PurchaseInvoiceCreated += OnPurchaseInvoiceCreated;
        EventHelper.SellReturnInvoiceCreated += OnSellReturnInvoiceCreated;
        EventHelper.FinancialRequestCreated += EventHelper_FinancialRequestOpenCreated;

        // MessageBox.Show("ReportRunnerHelper event are registered");
    }

    private static void OnSellReturnInvoiceCreated(object? sender, PrintEventArgs<DtoSellReturnInvoice> e)
    {
        try
        {
            var ee = e.Model.Adapt<SalesInvoiceDto>();
            //var ddd = e.Details.Adapt<List<SalesInvoiceDetailDto>>();
            //ee.TotalAmount = e.TotalAmount;

            ee.Date = DateTime.Now.AddDays(10);
            var ddd = e.Model.Details.Select((s, indx) =>
            new SalesInvoiceDetailDto
            {
                ExpireDate = s.ExpireDate,
                ItemName = s.ItemName,
                //ItemNumber = s.ItemName,
                Price = s.Price,
                Quantity = s.Quantity,
                Total = (s.Price * s.Quantity) - s.Discount,
                Serial = (indx + 1),
                UnitName = s.UnitName,
                ItemNumber = s.ItemBarcode
            }).ToList();

            using var reportDefinition = new StreamReader($"./Reports/SellReturnInvoiceReport.rdlc").BaseStream;
            LocalReport report = new();
            
            report.DataSources.Add(new ReportDataSource("salesDet", ddd));
            report.DataSources.Add(new ReportDataSource("sales", new List<SalesInvoiceDto>() { ee }));
            report.DataSources.Add(new ReportDataSource("LuncherInfo", new List<ClientInfo> { Config.Get.ClientConfig }));
            report.LoadReportDefinition(reportDefinition);
            report.EnableExternalImages = true;

            report.SetParameters(new ReportParameter("InvoiceDate", ee.Date.ToString("yyyy‑MM‑dd")));

            byte[] pdf = report.Render("PDF");
            //ReportRunnerHelper.OpenPdf(ReportRunnerHelper.SavePdf(pdf));
            OpenAndSavePdf(pdf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    /// <summary>
    /// Print Sell Invoice Report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void OnSalesInvoiceCreated(object? sender, PrintEventArgs<DtoSalesInvoice> e)
    {

        // Legacy below:
        try
        {
            var pdf = GetSalesFiles(e);
            //ReportRunnerHelper.OpenPdf(ReportRunnerHelper.SavePdf(pdf));
            OpenAndSavePdf(pdf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static byte[] GetSalesFiles(PrintEventArgs<DtoSalesInvoice> e)
    {
        var ee = e.Model.Adapt<SalesInvoiceDto>();
        //var ddd = e.Details.Adapt<List<SalesInvoiceDetailDto>>();
        //ee.TotalAmount = e.TotalAmount;
        var ddd = e.Model.Details.Select((s, indx) =>
            new SalesInvoiceDetailDto
            {
                ExpireDate = s.ExpireDate,
                ItemName = s.ItemName,
                //ItemNumber = s.ItemName,
                Price = s.Price,
                Quantity = s.Quantity,
                Total = (s.Price * s.Quantity) - s.Discount,
                Serial = (indx + 1),
                UnitName = s.UnitName,
                ItemNumber = s.ItemBarcode
            }).ToList();

            
        Stream reportDefinition = new StreamReader($"./Reports/SaleInvoiceReport.rdlc").BaseStream;
        LocalReport report = new LocalReport();
        report.EnableExternalImages = true;
        report.LoadReportDefinition(reportDefinition);

            
        report.DataSources.Add(new ReportDataSource("returnDet", ee.ReturnInvoices));
        report.DataSources.Add(new ReportDataSource("salesDet", ddd));
        report.DataSources.Add(new ReportDataSource("sales", new List<SalesInvoiceDto>() { ee }));
        var config = Config.Get.ClientConfig;
        report.DataSources.Add(new ReportDataSource("LuncherInfo", new List<ClientInfo> { config }));

        byte[] pdf = report.Render("PDF");
        return pdf;
    }

    private static void OnPurchaseInvoiceCreated(object? sender, PrintEventArgs<DtoPurchaseInvoice> e)
    {
        try
        {
            var ee = e.Model.Adapt<PurchaseInvoiceDto>();
            //var ddd = e.Details.Adapt<List<PurchaseInvoiceDetailDto>>();
            //ee.TotalAmount = e.TotalAmount;
            var ddd = e.Model.Details.Select((s, indx) =>
                new PurchaseInvoiceDetailDto
                {
                    ExpireDate = s.ExpireDate,
                    ItemName = s.ItemName,
                    //ItemNumber = s.ItemName,
                    Price = s.Price,
                    Quantity = s.Quantity,
                    Total = (s.Price * s.Quantity) - s.Discount,
                    Serial = (indx + 1),
                    UnitName = s.UnitName,
                    ItemNumber = s.ItemBarcode
                }
            ).ToList();

            var saleInvoice = e; // viewModel.Model.DtoSalesInvoice;
            Stream reportDefinition = new StreamReader($"./Reports/PurchesReport.rdlc").BaseStream;
            LocalReport report = new LocalReport();
            
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);

            report.DataSources.Add(new ReportDataSource("purches", new List<PurchaseInvoiceDto>() { ee }));
            report.DataSources.Add(new ReportDataSource("purchesDet", ddd));
            report.DataSources.Add(new ReportDataSource("LuncherInfo", new List<ClientInfo> { Config.Get.ClientConfig }));

            byte[] pdf = report.Render("PDF");
            //ReportRunnerHelper.OpenPdf(ReportRunnerHelper.SavePdf(pdf));
            OpenAndSavePdf(pdf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }

    private static void EventHelper_FinancialRequestOpenCreated(object? sender, PrintEventArgs<RSimpleRequest> e)
    {
        //var dto = JsonConvert.DeserializeObject<DtoStatment>(e);
        //switch (sender)
        //{
        //    case ClientRepresentativeReceiptEntryViewModel clientRepresentativeReceipt:
        //        var pdf = Convert.FromBase64String(dto.Text64BaseString);
        //        ReportRunnerHelper.OpenAndSavePdf(pdf);
        //        break;
        //}
    }

    private static void ReportPrintRequested(object? sender, Helpers.ReportPrintEventArgs e)
    {

        var allowed = new string[]
        {
            "AccountMovements",
            "TheClientDebtion"
        };

        if (!string.IsNullOrEmpty(e.QueryRequest.ReportName) && allowed.Contains(e.QueryRequest.ReportName))
        {
            FixedStyleReport.StartReport(e);
            return;
        }

        if (e.DataTable == null || e.DataTable.Rows is null) return;
        var reportModel = sender as GeneralReportPageViewModel;
        //System.Windows.MessageBox.Show("ReportPrintRequested");

        var rows = e.DataTable.Rows;
        var ee = new List<RandomDto>();
        foreach (DataRow row in rows)
        {
            var clmns = row.Table.Columns;
            var a = new RandomDto();

            if (clmns.Count >= 1) a.Column1 = row[0].ToString();
            if (clmns.Count >= 2) a.Column2 = row[1].ToString();
            if (clmns.Count >= 3) a.Column3 = row[2].ToString();
            if (clmns.Count >= 4) a.Column4 = row[3].ToString();
            if (clmns.Count >= 5) a.Column5 = row[4].ToString();
            if (clmns.Count >= 6) a.Column6 = row[5].ToString();
            if (clmns.Count >= 7) a.Column7 = row[6].ToString();
            if (clmns.Count >= 8) a.Column8 = row[7].ToString();
            if (clmns.Count >= 9) a.Column9 = row[8].ToString();
            if (clmns.Count >= 10) a.Column10 = row[9].ToString();
            //if (row[8] is not null) a.Column8 = row[8].ToString();
            //if (row[9] is not null) a.Column9 = row[9].ToString();
            //if (row[10] is not null) a.Column10 = row[10].ToString();
            ee.Add(a);
        }
        
        try
        {

            Stream reportDefinition = new StreamReader($"./Reports/General/general.rdlc").BaseStream;
            LocalReport report = new LocalReport();
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);

            report.DataSources.Add(new ReportDataSource("DataSet1", ee));
            report.DataSources.Add(new ReportDataSource("LuncherInfo", new List<ClientInfo> { Config.Get.ClientConfig }));
            var parameters = new List<ReportParameter>();
            for (int i = 0; i < e.DataTable.Columns.Count; i++)
            {
                var col = e.DataTable.Columns[i];
                var colName = col.ColumnName;
                var parName = $"ReportParameter{(i + 1)}";
                var p = new ReportParameter(parName, colName);
                parameters.Add(p);
            }
            parameters.Add(new ReportParameter("ReportParameter_title", e.QueryRequest.Title));
            
            switch (e.QueryRequest.ReportName)
            {
                case "ItemsMovements":
                {
                    // Item amount is column 8
                    var sumOfAllItems = ee.Sum(i => double.Parse(i.Column8));
                    parameters.Add(new ReportParameter("OptionalInfoCol8", "مجموع الكميات: " + sumOfAllItems));
                    break;
                }
                case "RepresentativeItemsMovements":
                {
                    // Bill total is column 8
                    var billSum = ee.Sum(i => double.Parse(i.Column8));
                    parameters.Add(new ReportParameter("OptionalInfoCol8", "مجموع الفواتير: " + billSum));

                    break;
                }
            }
            
            parameters.Add(new ReportParameter("PrintTime", CommonTextHelpers.GetPrintTime()));
            report.SetParameters(parameters);
            //var a = report.GetDocumentMap();

            byte[] pdf = report.Render("PDF");
            //ReportRunnerHelper.OpenPdf(ReportRunnerHelper.SavePdf(pdf));
            OpenAndSavePdf(pdf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static void ModifyReport(string reportPath, string xmlDataPath)
    {
        try
        {
            // Load the RDLC file as XML
            XmlDocument reportDefinition = new XmlDocument();
            reportDefinition.Load(reportPath);

            // Load the XML data that specifies changes
            XmlDocument xmlData = new XmlDocument();
            xmlData.Load(xmlDataPath);


            //This is a highly simplified example.  You'll need robust error handling and more sophisticated XML navigation.
            XmlNode bodyNode = reportDefinition.SelectSingleNode("//Body");
            if (bodyNode != null)
            {
                foreach (XmlNode changeNode in xmlData.SelectNodes("//Change"))
                {
                    string columnToModify = changeNode.Attributes["column"].Value;
                    string action = changeNode.Attributes["action"].Value;

                    XmlNode columnNode = bodyNode.SelectSingleNode($".//Textbox[@Name='{columnToModify}']");


                    if (columnNode != null)
                    {
                        if (action == "hide")
                        {
                            //Hide the column by setting its visibility to false.  This requires knowing the specific attribute controlling visibility in your report's XML.
                            //This is highly report-specific and may not be the correct attribute.
                            XmlAttribute visibilityAttribute = columnNode.Attributes["Hidden"];
                            if (visibilityAttribute == null)
                            {
                                visibilityAttribute = reportDefinition.CreateAttribute("Hidden");
                                columnNode.Attributes.Append(visibilityAttribute);
                            }
                            visibilityAttribute.Value = "true";
                        }
                        else if (action == "show")
                        {
                            //Show the column
                            XmlAttribute visibilityAttribute = columnNode.Attributes["Hidden"];
                            if (visibilityAttribute != null)
                            {
                                columnNode.Attributes.Remove(visibilityAttribute);
                            }
                        }
                        // Add more actions (e.g., rename, add columns) as needed.  This is extremely complex and would require deep understanding of the RDLC XML structure.
                    }
                    else
                    {
                        Console.WriteLine($"Column '{columnToModify}' not found in the report.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Report body not found.");
            }

            // Save the modified RDLC file.  Always back up your original file before doing this!
            reportDefinition.Save(reportPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error modifying report: {ex.Message}");
        }
    }

    /// <summary>
    /// "state", dataSource[0]  
    /// "date", dataSource[1]   
    /// "others", dataSource[2] 
    /// "amount", dataSource[3] 
    /// </summary>
    public static void LoadStatementReport(string[] dataSource)
    {
        var reportDefinition = new StreamReader("./Reports/MovementReceipt.rdlc").BaseStream;

        LocalReport report = new LocalReport();
        report.LoadReportDefinition(reportDefinition);

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
        };

        report.SetParameters(parameters);
        var pdf = report.Render("PDF");
        var file = SavePdf(pdf);
        OpenPdf(file);
    }

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

    public static void OpenPdf(FileInfo fileInfo)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileInfo.FullName,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }

    public static void OpenAndSavePdf(byte[] file)
    {
        var fileInfo = SavePdf(file);
        OpenPdf(fileInfo);
    }

    public static async void AskPrintOrOpen(byte[] file)
    {

        var fileInfo = SavePdf(file);
        void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Specify the file path
            string filePath = fileInfo.FullName;
            // Read the content of the file
            string line = System.IO.File.ReadAllText(filePath);
            // Create a Font object (adjust as needed)
            Font font = new Font("Arial", 12);
            // Create a RectangleF to define the printing area
            RectangleF area = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height);
            // Draw the content to the printing area
            e.Graphics.DrawString(line, font, Brushes.Black, area);
            // Set HasMorePages to false to indicate that there are no more pages to print
            e.HasMorePages = false;
        }

        var s = new Wpf.Ui.Controls.MessageBox()
        {
            IsPrimaryButtonEnabled = true,
            PrimaryButtonText = "طباعة مباشرة",

            IsSecondaryButtonEnabled = true,
            SecondaryButtonText = "فتح",

            CloseButtonText = "رجوع",

            Title = "هل تريد طباعة التقرير من الطابعة",
        };
        var res = await s.ShowDialogAsync();
        if (res == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            //PrintDocument pd = new PrintDocument();
            //pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            //// Optionally, display a print dialog for user configuration
            //PrintDialog printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == DialogResult.OK)
            //{
            //    pd.PrinterSettings = printDialog.PrinterSettings;
            //    pd.Print();
            //}

            string pdfPath = fileInfo.FullName;


            PrintDocument pd = new PrintDocument();
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() ?? false)
            {
                pd.PrinterSettings.PrintFileName = fileInfo.FullName;
                pd.Print();
            }

        }
        else if (res == Wpf.Ui.Controls.MessageBoxResult.Secondary)
        {
            OpenPdf(fileInfo);
        }
    }

}