using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace SmartiErpWin.Client.DtoModels.PurchasesModels;

public partial class DtoPurchasesInvoiceModel: ObservableObject
{
    [ObservableProperty]
    private DtoPurchaseInvoice? _dtoPurchasesInvoice;
    
    public List<DtoPurchasesInvoiceDetailsModel> PurchasesInvoiceDetailModels { get; set; } = new();
    
    public List<DtoPurchasesInvoiceCostsModel> PurchasesInvoiceDetailCosts { get; set; } = new();
}