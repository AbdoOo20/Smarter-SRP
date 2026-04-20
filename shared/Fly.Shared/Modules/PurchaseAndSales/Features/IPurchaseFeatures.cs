using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface IPurchaseFeatures : IFeatureBase<DtoPurchaseInvoice, PurchaseInvoiceQueryRequest, DeleteRequestBase, PurchaseInvoiceCreateRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(StoreQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSuppliers(SupplierQueryRequest request); 
    ValueTask<HttpResponseWrapper<long>> GetNextNumber();

}

