using Fly.Shared.Modules.Financial.Requests;
using Microsoft.Reporting.NETCore;
using System.Reflection;

namespace Fly.Core.Modules.F.Financial.Reports;

internal class FinancialReports
{
    const string extension = "pdf";
    const string renderFormat = "PDF";
    const string mimeType = "application/pdf";
    private readonly string reportFileName;

    public FinancialReports(string reportFileName)
    {
        this.reportFileName = reportFileName;
    }

    public async Task<FileResponse> Generate(Action<byte[]>? action = null, Func<Task<object>>? fillDatasource = null, Func<Task<List<Microsoft.Reporting.NETCore.ReportParameter>>>? fillParameters = null, string datasourceName = "DataSet1"
        , object[]? dataSources = null
        , string[]? dataSourcesNames = null)
    {
        using var report = new LocalReport();
        using var rs = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Fly.Core.Modules.F.Financial.Reports." + reportFileName + ".rdlc");
        report.LoadReportDefinition(rs);

        if (fillDatasource is not null)
        {
            var data = await fillDatasource.Invoke();
            if (data is not null)
            {
                //report.DataSources.Add(new ReportDataSource("ReportItemSchemas", data));
                report.DataSources.Add(new ReportDataSource(datasourceName, data));
            }
        }
        if (fillParameters is not null)
        {
            var parameters = await fillParameters.Invoke();
            if (parameters is not null)
                report.SetParameters(parameters);
        }
        if (dataSources is not null)
        {
            for (int i = 0; i < dataSources.Length; i++)
            {
                report.DataSources.Add(new ReportDataSource(dataSourcesNames[i], dataSources[i]));
            }
        }
        var pdf = report.Render(renderFormat);

        if (action is not null)
        {
            action.Invoke(pdf);
        }
        return new FileResponse(Convert.ToBase64String(pdf), extension);
    }

    public static void Load(LocalReport report, decimal widgetPrice, decimal gizmoPrice)
    {
        //var items = new[] { new ReportItem { Description = "Widget 6000", Price = widgetPrice, Qty = 1 }, new ReportItem { Description = "Gizmo MAX", Price = gizmoPrice, Qty = 25 } };

        //var parameters = new[] { new ReportParameter("Title", "Invoice 4/2020") };
        //using var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("ReportViewerCore.Sample.AspNetCore.Reports.Report.rdlc");
        //report.DataSources.Add(new ReportDataSource("Items", items));
        //report.SetParameters(parameters);
    }
}