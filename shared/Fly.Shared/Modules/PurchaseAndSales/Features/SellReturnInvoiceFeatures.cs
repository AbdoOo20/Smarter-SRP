using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface SellReturnInvoiceFeatures 
    : IFeatureBase<DtoSellReturnInvoice, SellReturnInvoiceQueryRequest
        , DeleteSellReturnInvoiceRequest, SellReturnInvoiceCreateRequest>
{
    ValueTask<HttpResponseWrapper<long>> GetNextNumber();
    ValueTask<HttpResponseWrapper<DtoSellReturnInvoice>> GetVerifyInvoice(SalesInvoiceQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes(SafeQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(StoreQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients(ClientQueryRequest request);
    ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> PostItemInfoQuery(GetItemInfoQueryRequest queryRequest);
    
}
