using AutoMapper;
using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Maintenance.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.Maintenance.Services;

internal class MaintenanceInvoiceService(ILogger<ServiceBase<DtoMaintenanceInvoice,
        TMaintenanceInvoice>> logger, IMapper mapper, MapsterMapper.IMapper mapsterMapper,
    IServiceProvider serviceProvider)
    : ServiceBase<DtoMaintenanceInvoice, TMaintenanceInvoice>(logger, mapper, mapsterMapper, serviceProvider),
        IMaintenanceInvoiceService
{
}