using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Maintenance.Dtos;

namespace Fly.Core.Modules.F.Maintenance.Services;


public interface IMaintenanceInvoiceService : IServiceBase<DtoMaintenanceInvoice, TMaintenanceInvoice> { }