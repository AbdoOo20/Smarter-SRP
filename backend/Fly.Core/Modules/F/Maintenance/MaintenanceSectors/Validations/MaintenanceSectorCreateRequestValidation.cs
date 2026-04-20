using FluentValidation;
using Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Requests;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Validations;

public class MaintenanceSectorCreateRequestValidation : AbstractValidator<MaintenanceSectorCreateRequest>
{
    public MaintenanceSectorCreateRequestValidation(IServiceBase<DtoMaintenanceSector, TMaintenanceSector> serviceBase)
    {
            
    }
}