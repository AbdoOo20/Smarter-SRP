using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Mapster;

namespace Leapay.Client.ClientModels.Dto.DtoModels.Reports;

public class SalesInvoiceDetailDto
{
    public int Serial { get; set; }
    public string ItemNumber { get; set; }
    public string ItemName { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string ExpireDateString
    {
        get => ExpireDate?.ToString("yyyy.MM.dd") ?? string.Empty;
        set => throw new NotImplementedException();
    }
    public string UnitName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public SalesInvoiceDetailDto() { }
    public SalesInvoiceDetailDto(DtoSalesInvoiceDetail dto, string storageName)
    {
        dto.Adapt(this);
    }
}