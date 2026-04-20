using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Common.Reporting;

public class RTSellInvoiceReportModel
{
    public long SerialNumber { get; set; }
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public string? ClientName { get; set; } = string.Empty;
    public string? ClientPhone { get; set; } = string.Empty;
    public string? RepresentativeName { get; set; } = string.Empty;
    public decimal RepresentativePercentage { get; set; }
    public decimal RepresentativePhone { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string? SafeName { get; set; }
    public decimal Discount { get; set; }
    public PeymentType PeymentType { get; set; }
    public string PeymentTypeString
    {
        set { }
        get => PeymentType switch
        {
            PeymentType.Cash => "نقدا",
            PeymentType.Deferred => "آجل",
            _ => ""
        };
    }

    public RTSellInvoiceReportDetailModel[] Details { get; set; } = [];
}