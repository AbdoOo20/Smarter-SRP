using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Requests;

namespace Fly.Shared.Modules.Personnel.Features;

public interface IDepartmentFeatures : IFeatureBase<DtoDepartment, DepartmentQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
{
}