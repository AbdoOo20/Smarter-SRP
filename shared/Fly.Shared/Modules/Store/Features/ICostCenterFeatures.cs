using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Store.Features;

public interface ICostCenterFeatures : IFeatureBase<DtoCostCenter, CostCenterQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
{
}