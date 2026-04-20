using Fly.Framework.Shared.Common.Features;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface ISafeFeatures : IFeatureBase<DtoSafe, SafeQueryRequest, SafeDeleteRequest, SafeCreateUpdateRequest>
{
}
