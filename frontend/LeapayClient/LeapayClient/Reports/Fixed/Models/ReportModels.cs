namespace Leapay.Client.Reports.Fixed.Models;

public class AccountMovementsReportModel
{
    public int SerialNumber { get; set; }

    public string Date { get; set; }

    public string AccountName { get; set; } = string.Empty;

    public string Movement { get; set; } = string.Empty;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public decimal Balance { get; set; }
}

public class TheClientDebtionReportModel
{
    public string AccountName { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public int LastPaymentNumber { get; set; }

    public string LastPaymentType { get; set; } = string.Empty;

    public decimal LastPaymentAmount { get; set; }

    public string LastPaymentDate { get; set; }

    public int LastInvoiceNumber { get; set; }

    public string LastInvoiceType { get; set; } = string.Empty;

    public decimal LastInvoiceAmount { get; set; }

    public string LastInvoiceDate { get; set; }

}