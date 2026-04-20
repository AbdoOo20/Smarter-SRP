using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Store.Features;

public interface IUnitFeatures : IFeatureBase<DtoUnit, UnitQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
{
}