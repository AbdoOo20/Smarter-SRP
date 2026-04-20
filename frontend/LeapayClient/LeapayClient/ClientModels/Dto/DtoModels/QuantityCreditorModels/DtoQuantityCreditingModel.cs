using Fly.Shared.Modules.PurchaseAndSales.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.QuantityCreditorModels;

public partial class DtoQuantityCreditingModel: ObservableObject
{
    [ObservableProperty]
    private DtoQuantityCreditingInvoice? _dtoQuantityCrediting;
    
    public List<DtoQuantityCreditingDetailsModel> QuantityCreditingDetailModels { get; set; } = new();
}