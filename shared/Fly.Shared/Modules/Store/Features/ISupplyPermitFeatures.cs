using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Store.Features;

//  SupplyPermit

public interface ISupplyPermitFeatures : IFeatureBase<DtoSupplyPermit, SupplyPermitQueryRequest, DeleteRequestBase, CreateUpdateSupplyPermitRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStore(StoreQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSuppliers(SupplierQueryRequest request);
}