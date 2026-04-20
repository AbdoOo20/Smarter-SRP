using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Store.Features;

public interface IItemCompositionFeatures : IFeatureBase<DtoItemComposition,
    ItemCompositionQueryRequest,
    DeleteItemCompositionRequest,
    CreateUpdateItemCompositionRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCategories(CategoryQueryRequest request);
    
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupVariances(ItemVarianceQueryRequest request);
}