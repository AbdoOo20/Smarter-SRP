using System.Data;
using System.Diagnostics;
using ClosedXML.Excel;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;
using Path = System.IO.Path;

namespace Leapay.Client.Services;

public static class ExportServices
{
    // Most of the methods in this class are either outdated or kept as backup
    
    private static DataTable GetDataTable(DataGrid dataGrid)
    {
        // This method is probably not compatible with Avalonia's DataGrid.
        
        var dataTable = new DataTable();

        foreach (var column in dataGrid.Columns.OfType<DataGridTextColumn>())
        {
            if (column.Header is TextBlock txtBlock)
            {
                dataTable.Columns.Add(txtBlock.Text);
            }
        }

        foreach (var item in dataGrid.ItemsSource)
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

    private static List<IXLCell> GetColumnCells(
        this IXLWorksheet worksheet,
        string name)
    {
        return worksheet.Column(name).Cells().ToList();
    }
    
    /*public static string SelectExcelFile()
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
    }*/

    /*
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
    */
    
    /*public static void ExportDataGridToPdf(DataGrid dataGrid)
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
    }*/

    /*public static Task ExportToExcelAsync(DataGrid dataGrid)
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
    }*/

    /*public static void PrintToDialogue(DataGrid printedGrid)
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
    }*/

    /*public static void PrintToDdlc(DataGrid printedGrid)
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
    }*/

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
    
    public static async Task NavigateToWhatsAppAsync(string? phoneNumber, string source = "wa")
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return;
            
        if (phoneNumber.StartsWith('0'))
            phoneNumber = phoneNumber.Remove(0, 1);

        if(!phoneNumber.StartsWith("+218"))
            phoneNumber = "+218" + phoneNumber;

        phoneNumber = phoneNumber.Replace(" ", string.Empty).Replace("-", string.Empty);
        var url = $"https://web.whatsapp.com/send?phone={phoneNumber}";

        try
        {
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
                
            await Task.Delay(10000);

            var browserProcesses = Process.GetProcessesByName("chrome")
                .Concat(Process.GetProcessesByName("brave"))
                .Concat(Process.GetProcessesByName("msedge"))
                .Concat(Process.GetProcessesByName("iexplore"));

            foreach (var browserProcess in browserProcesses
                         .Where(p => p.StartTime.AddSeconds(15) > DateTime.Now))
            {
                try
                {
                    browserProcess.Kill();
                    await browserProcess.WaitForExitAsync();
                }
                catch
                {
                    
                }
            }
        }
        catch (Exception ex)
        {
            Information($"Failed to open or destroy WhatsApp: {ex.Message}");
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
}