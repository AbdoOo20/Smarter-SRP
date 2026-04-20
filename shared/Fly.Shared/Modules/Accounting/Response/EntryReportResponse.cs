namespace Fly.Shared.Modules.Accounting.Response;

public class EntryReportResponse
{
    public DateTime Date { get; set; }
    public string Note { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public string? AccountName { get; set; } = string.Empty;
    public long? EntryNumber { get; set; } 
}