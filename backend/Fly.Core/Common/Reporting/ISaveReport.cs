namespace Fly.Core.Common.Reporting;

public interface ISaveReport
{
    Task<string> SaveBytes(byte[] date, string? fileName = null);
    Task<string> SaveBytes(string string64based);
    Task<string> GetReport(string reportName);
}