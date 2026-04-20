using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Maintenance.Features;

public interface IMaintenanceInvoiceFeatures : IFeatureBase<DtoMaintenanceInvoice,
    MaintenanceInvoiceQueryRequest,
    MaintenanceInvoiceDeleteRequest,
    MaintenanceInvoiceCreateUpdateRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(StoreQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients(ClientQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes(SafeQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItemVariances(ItemVarianceQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupMaintenanceSectors(MaintenanceSectorQueryRequest request);
}