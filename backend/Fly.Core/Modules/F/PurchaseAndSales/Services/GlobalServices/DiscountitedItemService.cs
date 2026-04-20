using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Shared.Modules.Financial.Dtos;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;

public class DiscountitedItemService : ServiceBase<DtoDiscountitedItem, TDiscountitedItem>, IDiscountitedItemService
{
    public DiscountitedItemService(ILogger<ServiceBase<DtoDiscountitedItem, TDiscountitedItem>> logger, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapsterMapper, IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {
    }
}