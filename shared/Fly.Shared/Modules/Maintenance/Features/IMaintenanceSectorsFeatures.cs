using Fly.Framework.Shared.Common.Features;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Requests;

namespace Fly.Shared.Modules.Maintenance.Features;

public interface IMaintenanceSectorsFeatures
    : IFeatureBase<DtoMaintenanceSector, MaintenanceSectorCreateRequest, MaintenanceSectorDeleteRequest, MaintenanceSectorCreateUpdateRequest>
{
        
}