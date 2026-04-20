using Fly.Framework.Shared.Common.Features;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Requests;

namespace Fly.Shared.Modules.Factoring.Features;

public interface IManufacturingFacilityFeatures
    : IFeatureBase<DtoManufacturingFacility,
        ManufacturingFacilityCreateRequest,
        ManufacturingFacilityDeleteRequest,
        ManufacturingFacilityCreateUpdateRequest>
{
        
}