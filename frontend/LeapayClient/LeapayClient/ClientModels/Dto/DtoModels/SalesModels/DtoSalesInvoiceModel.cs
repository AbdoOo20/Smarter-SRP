using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace Leapay.Client.ClientModels.Dto.DtoModels.SalesModels;

public partial class DtoSalesInvoiceModel: ObservableObject
{
    [ObservableProperty]
    public partial DtoSalesInvoice? DtoSalesInvoice { get; set; }
    
    public List<DtoSalesInvoiceDetailsModel> SalesInvoiceDetailModels { get; set; } = new();
}
