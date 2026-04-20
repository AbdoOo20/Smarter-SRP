using FluentValidation;
using Fly.Core.Modules.Factoring.ManufacturingFacilities.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Requests;

namespace Fly.Core.Modules.Factoring.ManufacturingFacilities.Validations;

public class ManufacturingFacilityCreateRequestValidation : AbstractValidator<ManufacturingFacilityCreateRequest>
{
    public ManufacturingFacilityCreateRequestValidation(IServiceBase<DtoManufacturingFacility, TManufacturingFacility> serviceBase)
    {
            
    }
}