using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Store.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.E.Store.Services.InternalServices;

internal class InventoryService : ServiceBase<DtoInventory, TInventory>, IInventoryService
{
    public InventoryService(ILogger<InventoryService> logger
        , AutoMapper.IMapper mapper
        , MapsterMapper.IMapper mapsterMapper
        , IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {
    }

}