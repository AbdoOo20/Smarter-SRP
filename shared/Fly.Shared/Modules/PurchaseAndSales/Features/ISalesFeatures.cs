using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.PurchaseAndQuantityCrediting.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface IFastSalesFeatures : IFeatureBase<DtoSalesInvoice, SalesInvoiceQueryRequest, DeleteSalesInvoiceRequest, FastSaleInvoiceCreateRequest>
{

}
public interface ISalesFeatures : IFeatureBase<DtoSalesInvoice, SalesInvoiceQueryRequest, DeleteSalesInvoiceRequest, SalesInvoiceCreateRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(StoreQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients(ClientQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes(SafeQueryRequest request);
    ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> PostItemInfoQuery(GetItemInfoQueryRequest queryRequest);
    ValueTask<HttpResponseWrapper<long>> GetNextNumber();
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupRepresentatives(MarketerQueryRequest request);
}
public interface IQuantityCreditingFeatures : IFeatureBase<DtoQuantityCreditingInvoice, QuantityCreditingInvoiceQueryRequest, DeleteQuantityCreditingInvoiceRequest, QuantityCreditingInvoiceCreateRequest>
{
    ValueTask<HttpResponseWrapper<long>> GetNextNumber();
}
