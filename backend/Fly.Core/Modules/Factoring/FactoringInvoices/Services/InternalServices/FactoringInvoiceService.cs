using AutoMapper;
using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Factoring.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Services.InternalServices;

internal class FactoringInvoiceService(ILogger<ServiceBase<DtoFactoringInvoice,
        TFactoringInvoice>> logger, IMapper mapper, MapsterMapper.IMapper mapsterMapper,
    IServiceProvider serviceProvider)
    : ServiceBase<DtoFactoringInvoice, TFactoringInvoice>(logger, mapper, mapsterMapper, serviceProvider),
        IFactoringInvoiceService
{
}