using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Requests;

namespace Fly.Shared.Modules.Personnel.Features;

public interface IDiscountIncreaseFeatures : IFeatureBase<DtoDiscountIncrease, DiscountIncreaseQueryRequest, DeleteRequestBase, CreateUpdateDiscountIncreaseRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupEmployees(EmployeeQueryRequest request);
}