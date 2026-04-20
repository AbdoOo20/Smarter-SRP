using System.Data;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using ClosedXML.Excel;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using iText.Layout.Element;
using Paragraph = iText.Layout.Element.Paragraph;
using Path = System.IO.Path;
using Table = iText.Layout.Element.Table;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using System.Xml.Linq;
using SmartiErpWin.Client.Reports;
using Microsoft.Reporting.NETCore;
using System.Diagnostics;

namespace SmartiErpWin.Client.Services;

public static class ExportServices
{
    public static IEnumerable<DtoAttendanceDeparture> ReadAttendanceFromExcel(string excelFilePath)
    {
        var workbook = new XLWorkbook(excelFilePath);
        var worksheet = workbook.Worksheet(1);

        var employeeCells = worksheet.GetColumnCells(EmployeeNumberColumn);
        var dateTimeCells = worksheet.GetColumnCells(DateTimeColumn);
        var attendanceModeCells = worksheet.GetColumnCells(AttendanceModeColumn);

        for (var i = 0; i < employeeCells.Count; i++)
        {
            var employeeName = string.Empty;
            var dateTime = default(DateTime);
            var mode = AttendanceDepartureMode.Attendance;

            try
            {
                employeeName = employeeCells[i].Value.GetText();
                mode = attendanceModeCells[i].Value.GetText() == AttendanceColumnCell
                    ? AttendanceDepartureMode.Attendance
                    : AttendanceDepartureMode.Departure;
                dateTime = DateTime.Parse(dateTimeCells[i].Value.GetText());
            }
            catch (Exception x)
            {
                Error(x, "Error while reading attendance cells");
            }

            yield return new DtoAttendanceDeparture
            {
                EmployeeName = employeeName,
                Date = dateTime,
                Mode = mode
            };
        }
    }

    public static string SelectExcelFile()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            DefaultExt = "xls",
            Filter = "Excel Files (*.xls)|*.xls"
        };

        var result = dlg.ShowDialog();

        var fileName = result ?? false ? dlg.FileName : string.Empty;
        Verbose("Selected Excel file: {0}", fileName);

        return fileName;
    }

    public static string SelectSaveFile(string extension, string name)
    {
        var dlg = new Microsoft.Win32.SaveFileDialog
        {
            DefaultExt = "xls",
            Filter = $"{name} (*.{extension})|*.{extension}"
        };

        var result = dlg.ShowDialog();

        var fileName = result ?? false ? dlg.FileName : string.Empty;
        Verbose("Selected save location for {name}: {path}", name, fileName);

        return fileName;
    }

    private static List<IXLCell> GetColumnCells(
        this IXLWorksheet worksheet,
        string name)
    {
        return worksheet.Column(name).Cells().ToList();
    }

    public static void ExportDataGridToPdf(DataGrid dataGrid)
    {
        var filePath = SelectSaveFile("PDF files", "pdf");

        if (string.IsNullOrEmpty(filePath))
            return;

        var path = Path.Combine(Directory.GetCurrentDirectory(), DefaultFontFile);

        Verbose("Using font file ({0}) to save PDF...", DefaultFontFile);

        var font = PdfFontFactory.CreateFont(
            path,
            PdfEncodings.IDENTITY_H,
            PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

        var dataTable = GetDataTable(dataGrid);

        using var writer = new PdfWriter(filePath);
        using var pdfDoc = new PdfDocument(writer);
        using var document = new iText.Layout.Document(pdfDoc);

        var table = new Table(UnitValue.CreatePercentArray(dataTable.Columns.Count))
            .UseAllAvailableWidth();

        foreach (DataColumn column in dataTable.Columns)
        {
            table.AddHeaderCell(new Cell().Add(new Paragraph(column.ColumnName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(font))
                .SetBackgroundColor(new DeviceRgb(224, 230, 237)));
        }

        foreach (DataRow row in dataTable.Rows)
        {
            foreach (var cell in row.ItemArray)
            {
                var textData = cell?.ToString() ?? string.Empty;

                table.AddCell(new Cell().Add(new Paragraph(textData)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(font)));
            }
        }

        document.Add(table);
        Verbose("PDF export completed and saved at: {0}", filePath);
    }

    public static Task ExportToExcelAsync(DataGrid dataGrid)
    {
        var filePath = SelectSaveFile("Excel Files", "xls");

        if (string.IsNullOrEmpty(filePath))
            return Task.CompletedTask;

        var dataTable = GetDataTable(dataGrid);

        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Table");

        worksheet.Cell(1, 1).InsertTable(dataTable);

        worksheet.Columns().AdjustToContents();

        workbook.SaveAs(filePath);

        Debug("DataGrid exported to Excel file: {0}", filePath);

        return Task.CompletedTask;
    }

    private static DataTable GetDataTable(DataGrid dataGrid)
    {
        var dataTable = new DataTable();

        foreach (var column in dataGrid.Columns.OfType<DataGridTextColumn>())
        {
            if (column.Header is TextBlock txtBlock)
            {
                dataTable.Columns.Add(txtBlock.Text);
            }
        }

        foreach (var item in dataGrid.Items)
        {
            var row = dataTable.NewRow();
            foreach (var column in dataGrid.Columns.OfType<DataGridTextColumn>())
            {
                var columnHeader = (column.Header as TextBlock)?.Text ?? string.Empty;
                
                if (column.GetCellContent(item) is TextBlock textBlock
                    && !string.IsNullOrEmpty(textBlock.Text))
                {
                    row[columnHeader] = textBlock.Text;
                }
            }

            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

    public static void PrintToDialogue(DataGrid printedGrid)
    {
        var printDialog = new PrintDialog();

        if (printDialog.ShowDialog() != true)
            return;

        var document = new FixedDocument();

        var fixedPage = new FixedPage();

        var grid = new Grid();
        grid.Children.Add(printedGrid);

        fixedPage.Width = printDialog.PrintableAreaWidth;
        fixedPage.Height = printDialog.PrintableAreaHeight;

        var visualBrush = new VisualBrush(grid);

        var rectangle = new Rectangle
        {
            Width = fixedPage.Width,
            Height = fixedPage.Height,
            Fill = visualBrush
        };

        fixedPage.Children.Add(rectangle);

        var pageContent = new PageContent
        {
            Child = fixedPage
        };

        document.Pages.Add(pageContent);

        printDialog.PrintDocument(document.DocumentPaginator, "Print");
    }



    public static void PrintToDdlc(DataGrid printedGrid)
    {

        var dir = Path.Combine(Environment.CurrentDirectory, "Reports", "General");
        var reportFilePath = Directory.GetFiles(dir, "general.rdlc").FirstOrDefault();

        var reportEditor = new RdlcReportEditor(reportFilePath);

        var d = GetDataTable(printedGrid);
        // Example DataTable
        DataTable dataTable = new DataTable("NewDataSet");
        dataTable.Columns.Add("Column1", typeof(string));
        dataTable.Columns.Add("Column2", typeof(int));

        // Add DataTable to the report
        reportEditor.AddDataTable(dataTable);

        // Add a new column to the Tablix
        reportEditor.AddColumn("NewColumn", "String");

        // Save the modified report
        var f = Guid.NewGuid().ToString();
        var fileStram = new FileStream(Path.Combine(dir, f), FileMode.OpenOrCreate);
        //var newFile = new FileInfo();
        //reportEditor.Save(newFile.FullName);

        LocalReport report = new LocalReport();
        report.LoadReportDefinition(fileStram);
        report.DataSources.Add(new ReportDataSource("NewDataSet", dataTable));
        report.SetParameters(new[] { new ReportParameter("Parameter1", "Parameter value") });
        byte[] pdf = report.Render("PDF");

        var pdfFile = SavePdf(pdf);
        OpenPdf(pdfFile);
    }

    private static FileInfo SavePdf(byte[] file)
    {
        var fileName = Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Reports", fileName);
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Directory.Create();
        using (var dtr = fileInfo.OpenWrite())
        {
            dtr.Write(file, 0, file.Length);
        }
        return fileInfo;
    }

    private static void OpenPdf(FileInfo fileInfo)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = fileInfo.FullName,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }


    public static void PrintToDdlc2(DataGrid printedGrid)
    {
        // Find the RDLC report file
        var reportFilePath = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Reports", "General"), "general.rdlc").FirstOrDefault();
        if (reportFilePath == null)
        {
            throw new FileNotFoundException("RDLC report file not found");
        }

        // Load the report file
        XDocument reportDocument = XDocument.Load(reportFilePath);

        // Find the DataSet element
        XElement dataSetElement = reportDocument.Descendants("DataSet").FirstOrDefault();
        if (dataSetElement == null)
        {
            throw new InvalidOperationException("DataSet element not found in RDLC report file");
        }

        // Create a new DataSet
        DataSet dataSet = new DataSet();

        // Add a new DataTable to the DataSet
        DataTable dataTable = new DataTable();
        dataSet.Tables.Add(dataTable);

        // Add columns to the DataTable
        foreach (DataGridColumn column in printedGrid.Columns)
        {
            dataTable.Columns.Add(column.Header.ToString());
        }

        // Add rows to the DataTable
        foreach (var item in printedGrid.Items)
        {
            DataRow row = dataTable.NewRow();
            for (int i = 0; i < printedGrid.Columns.Count; i++)
            {
                var cell = printedGrid.Columns[i].GetCellContent(item);
                TextBlock textBlock = cell as TextBlock;
                if (textBlock != null)
                {
                    row[i] = textBlock.Text;
                }
            }
            dataTable.Rows.Add(row);
        }

        // Replace the DataSet element in the report file with the new DataSet
        dataSetElement.ReplaceWith(new XElement("DataSet", new XElement("diffgr", new XAttribute("xmlns", "urn:schemas-microsoft-com:xml-diffgram-v1"), dataSet.GetXml())));

        // Save the modified report file
        reportDocument.Save(reportFilePath);

        // Create a ReportViewer control
        //var reportViewer = new LocalReport();
        ////report.LoadReportDefinition(reportDefinition);
        //reportViewer.ReportPath = reportFilePath;

        //// Set the DataSource of the report
        //reportViewer.LocalReport.DataSources.Clear();
        //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dataSet.Tables[0]));

        //// Print the report
        //printDialog.PrintDocument(reportViewer.LocalReport, "Print");
    }

}