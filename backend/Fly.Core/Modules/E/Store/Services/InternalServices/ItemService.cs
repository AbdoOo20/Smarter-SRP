using AutoMapper;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Store.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.E.Store.Services.InternalServices;

internal class ItemService(ILogger<ServiceBase<DtoItem, TItem>> logger, IMapper mapper, MapsterMapper.IMapper mapsterMapper, IServiceProvider serviceProvider) : ServiceBase<DtoItem, TItem>(logger, mapper, mapsterMapper, serviceProvider), IItemService
{
}