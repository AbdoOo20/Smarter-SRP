using Fly.Core.Common.Reporting;

namespace Fly.WebApi.Tools.SaveFiles;

public class SaveReport : ISaveReport
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    private DirectoryInfo ReportsPath => new DirectoryInfo(Path.Combine(_webHostEnvironment.ContentRootPath, "Resources", "Reports"));

    public SaveReport(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        Directory.CreateDirectory(ReportsPath.FullName);
    }

    public async Task<string> SaveBytes(byte[] date, string? fileName = null)
    {
        //  todo:save `byte[]` to file
        var randomFileName = fileName ?? Guid.NewGuid().ToString() + ".pdf";
        ReportsPath.Create();
        var repFile = new FileInfo(Path.Combine(ReportsPath.FullName, randomFileName));
        await File.WriteAllBytesAsync(repFile.FullName, date);
        return randomFileName;
    }



    public Task<string> SaveBytes(string string64based)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetReport(string reportName)
    {
        await Task.CompletedTask;
        var repFile = new FileInfo(Path.Combine(ReportsPath.FullName, reportName));
        return repFile.Exists ? repFile.FullName : null;
    }
}