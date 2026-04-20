using Fly.Framework.Server.Persistence.Implementations;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Shared.Common;
using LinqKit;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Core.Modules.F.Marketers.Services.InternalServices;

internal class MarketerInventoryService : ServiceBase<DtoMarketerInventory, TMarketerInventory>, IMarketerInventoryService
{
    public MarketerInventoryService(ILogger<MarketerInventoryService> logger
        , AutoMapper.IMapper mapper
        , MapsterMapper.IMapper mapsterMapper
        , IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {

    }

    public async ValueTask<IEnumerable<DtoMarketerInventory>> GetDetails2(Identifier? marketerId = null, Identifier? itemId = null, Identifier? unitId = null)
    {
        var w = Build();

        if (marketerId is not null)
        {
            if (!marketerId.Value.IsNullOrEmpty())
                w = w.And(w => w.MarketerId == marketerId);
        }
        if (itemId is not null)
            if (!itemId.Value.IsNullOrEmpty())
                w = w.And(w => w.ItemId == itemId);

        if (unitId is not null)
            if (!unitId.Value.IsNullOrEmpty())
                w = w.And(w => w.UnitId == unitId);

        var inventory = await GetAllAsync(w);
        return inventory.Items;
    }

    public async ValueTask<IEnumerable<DtoMarketerInventory>> GetDetails(Identifier? marketerId = null, Identifier? itemId = null, Identifier? unitId = null, DateTime? from = null, DateTime? to = null)
    {
        var w = Build();

        if (marketerId is not null)
        {
            if (!marketerId.Value.IsNullOrEmpty())
                w = w.And(w => w.MarketerId == marketerId);
        }
        if (itemId is not null)
            if (!itemId.Value.IsNullOrEmpty())
                w = w.And(w => w.ItemId == itemId);

        if (unitId is not null)
            if (!unitId.Value.IsNullOrEmpty())
                w = w.And(w => w.UnitId == unitId);

        if (from is not null && to is not null)
            w = w.And(w => w.CreateDate >= from && to >= w.CreateDate);

        var inventory = await GetAllAsync(w);
        return inventory.Items;
    }


    public async ValueTask<IEnumerable<DtoMarketerInventory>> GetMovements(Identifier? itemId = null, Identifier? marketerId = null, Identifier[]? relatedId = null, RelatedInventory? relatedIndex = null, StorageItemProperties? storageItemProperties = null)
    {
        if (itemId.HasValue && itemId.Value.IsNullOrEmpty()) itemId = null;
        if (marketerId.HasValue && marketerId.Value.IsNullOrEmpty()) marketerId = null;
        if (relatedId?.Any() == false) relatedId = null;

        var w = Build();
        if (itemId is not null) w = w.And(w => w.ItemId == itemId);
        if (marketerId is not null) w = w.And(w => w.MarketerId == marketerId);
        if (relatedId is not null) w = w.And(w => relatedId.Contains(w.RelatedId.Value));
        if (relatedIndex is not null) w = w.And(w => w.RelatedIndex == relatedIndex);
        if (storageItemProperties?.ExpireDate is not null) w = w.And(w => w.ExpireDate == storageItemProperties.ExpireDate);

        var all = await GetAllAsync(w);
        return all.Items;
    }
}
