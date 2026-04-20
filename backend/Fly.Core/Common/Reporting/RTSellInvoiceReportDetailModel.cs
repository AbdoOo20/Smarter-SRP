namespace Fly.Core.Common.Reporting;

public class RTSellInvoiceReportDetailModel
{
    public string UnitName { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string ItemBarcode { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}