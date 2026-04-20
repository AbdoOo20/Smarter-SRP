using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Core.Modules.E.Store.Models.DbModels;

namespace Fly.Core.Modules.E.Store.Services.InternalServices;

internal interface IInventoryService : IServiceBase<DtoInventory, TInventory>
{

}