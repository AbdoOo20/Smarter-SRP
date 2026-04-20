using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface PurchaseReturnInvoiceFeatures
    : IFeatureBase<DtoPurchaseReturnInvoice, PurchaseReturnInvoiceQueryRequest
        , DeletePurchaseReturnInvoiceRequest, PurchaseReturnInvoiceCreateRequest>
{
    ValueTask<HttpResponseWrapper<long>> GetNextNumber();
    ValueTask<HttpResponseWrapper<DtoPurchaseReturnInvoice>> GetVerifyInvoice(PurchaseInvoiceQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes(SafeQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(StoreQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients(ClientQueryRequest request);
    ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> PostItemInfoQuery(GetItemInfoQueryRequest queryRequest);
    
}
