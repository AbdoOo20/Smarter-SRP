using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Global.Requests;
using Fly.Framework.Shared.Common.Features;

namespace Fly.Shared.Modules.Global.Features;

public interface IRoleGroupFeatures : IFeatureBase<DtoRoleGroup, RoleGroupQuery, RoleGroupDelete, RoleGroupCreateUpdate>
{
}