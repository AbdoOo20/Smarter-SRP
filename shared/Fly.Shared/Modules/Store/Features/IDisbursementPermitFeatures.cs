using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Store.Features;

public interface IDisbursementPermitFeatures : IFeatureBase<DtoDisbursementPermit, DisbursementPermitQueryRequest, DeleteRequestBase, CreateDisbursementPermitRequest, UpdateDisbursementPermitRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStore(StoreQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCostcenter(SupplierQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
}