using AutoMapper;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.Store.Dtos;
using SmartiErpWin.Client.DtoModels.AccountingModels;
using SmartiErpWin.Client.DtoModels.MarketingModels;
using SmartiErpWin.Client.DtoModels.PurchasesModels;
using SmartiErpWin.Client.DtoModels.QuantityCreditorModels;
using SmartiErpWin.Client.DtoModels.SalesModels;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

namespace SmartiErpWin.Client.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<DtoItemUnit, DtoItemUnitModel>();
        CreateMap<DtoItemUnitModel, DtoItemUnit>();

        CreateMap<DtoDisbursementPermitDetail, DtoDisbursementPermitDetailModel>();
        CreateMap<DtoDisbursementPermitDetailModel, DtoDisbursementPermitDetail>();

        CreateMap<DtoEntryDetail, DtoEntryDetailModel>();
        CreateMap<DtoEntryDetailModel, DtoEntryDetail>();

        CreateMap<DtoSupplyPermitDetail, DtoSupplyPermitDetailModel>();
        CreateMap<DtoSupplyPermitDetailModel, DtoSupplyPermitDetail>();
        
        CreateMap<DtoSalesInvoiceDetail, DtoSalesInvoiceDetailsModel>();
        CreateMap<DtoSalesInvoiceDetailsModel, DtoSalesInvoiceDetail>();
        
        CreateMap<DtoPurchaseInvoiceDetail, DtoPurchasesInvoiceDetailsModel>();
        CreateMap<DtoPurchasesInvoiceDetailsModel, DtoPurchaseInvoiceDetail>();
        
        CreateMap<DtoPurchaseInvoiceCost, DtoPurchasesInvoiceCostsModel>();
        CreateMap<DtoPurchasesInvoiceCostsModel, DtoPurchaseInvoiceCost>();
        
        CreateMap<DtoMarketerDisbursementPermitDetail, DtoMarketerDisbursementPermitDetailsModel>();
        CreateMap<DtoMarketerDisbursementPermitDetailsModel, DtoMarketerDisbursementPermitDetail>();
        
        CreateMap<DtoQuantityCreditingInvoiceDetail, DtoQuantityCreditingDetailsModel>();
        CreateMap<DtoQuantityCreditingDetailsModel, DtoQuantityCreditingInvoiceDetail>();

        CreateMap<DtoSellReturnInvoiceDetail, DtoSellReturnInvoiceDetailModel>();
        CreateMap<DtoSellReturnInvoiceDetailModel, DtoSellReturnInvoiceDetail>();

        CreateMap<DtoPurchaseReturnInvoiceDetail, DtoPurchaseReturnlInvoiceDetailModel>();
        CreateMap<DtoPurchaseReturnlInvoiceDetailModel, DtoPurchaseReturnInvoiceDetail>();

        //CreateMap<DtoSalesInvoice, DtoSellReturnInvoice>();
    }
}