using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace SmartiErpWin.Client.DtoModels.SalesModels;

public partial class DtoSalesInvoiceModel: ObservableObject
{
    [ObservableProperty]
    private DtoSalesInvoice? _dtoSalesInvoice;
    
    public List<DtoSalesInvoiceDetailsModel> SalesInvoiceDetailModels { get; set; } = new();
}
