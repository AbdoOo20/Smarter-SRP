using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace SmartiErpWin.Client.DtoModels.SalesModels;

public partial class DtoSelReturnlInvoiceModel: ObservableObject
{
    [ObservableProperty]
    private DtoSellReturnInvoice? _sellInvoiceReturn;
    
    public List<DtoSellReturnInvoiceDetailModel> SellReturnInvoiceDetailModels { get; set; } = new();
}
public partial class DtoPurchaseReturnlInvoiceModel : ObservableObject
{
    [ObservableProperty]
    private DtoPurchaseReturnInvoice? _purchaseReturnlInvoiceReturn;
    
    public List<DtoPurchaseReturnlInvoiceDetailModel> PurchaseReturnInvoiceDetailModels { get; set; } = new();
}