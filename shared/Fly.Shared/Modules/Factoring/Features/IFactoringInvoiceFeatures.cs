using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Factoring.Features;

public interface IFactoringInvoiceFeatures : IFeatureBase<DtoFactoringInvoice,
    FactoringInvoiceQueryRequest,
    FactoringInvoiceDeleteRequest,
    FactoringInvoiceCreateUpdateRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(StoreQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients(ClientQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes(SafeQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItemVariances(ItemVarianceQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCompositions(ItemCompositionQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupManufacturingFacilities(ManufacturingFacilityQueryRequest request);
}