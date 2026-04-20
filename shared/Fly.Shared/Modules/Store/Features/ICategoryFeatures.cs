using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Store.Features;

public interface ICategoryFeatures : IFeatureBase<DtoCategory, CategoryQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCateguries(CategoryQueryRequest request);

}
