namespace Fly.Shared.Modules.Reporting.Response;

public class SalesInvoiceDetailResponse
{
    public DateTime InvoiceDate { get; set; }
    public long InvoiceNumber { get; set; }
    public string? ClientName { get; set; }
    public string? MarketerName { get; set; }
    public string? ClientNumber { get; set; }
    public string? SafeName { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? ItemNumber { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SalesValue { get; set; }
    public decimal SalesPercentage { get; set; }
    public decimal? CollectionValue { get; set; }
    //public decimal CollectionPercentage { get; set; }
    //public decimal InvoiceBalance { get; set; }
}
public class ClientsOverdueDebtsResponse
{
    public string ClientName { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal RemainingDebt { get; set; }
    public DateTime? Date { get; set; }
    public int OverdueDays { get; set; }
}
public class RepresentativeAmountsResponse
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public decimal Debit { get; set; }
    public decimal TotalAmount { get; set; }
    public long Number { get; set; }
    public DateTime? Date { get; set; }
}
public class RepresentativeAmountsResponseV2
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal TotalAmount { get; set; }
    public long Number { get; set; }
    public DateTime? Date { get; set; }
}