using System.Data;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;
using LinqKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.Marketers.Services.GlobalServices;

public class MarketerModuleService : GlobalModuleService, IMarketerModuleService
{

    //  Fields
    private readonly ILogger _logger;
    private readonly AutoMapper.IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly MapsterMapper.IMapper _mapsterMapper;
    private readonly IServiceBase<DtoMarketerInventory, TMarketerInventory> _inventoryService;

    public MarketerModuleService(ILogger<MarketerModuleService> logger,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapsterMapper,
        IServiceProvider serviceProvider)
        : base(logger, mapper, mapsterMapper, serviceProvider)
    {
        _mapper = mapper;
        _logger = logger;
        _mapsterMapper = mapsterMapper;
        _serviceProvider = serviceProvider;

        _inventoryService = serviceProvider.GetRequiredService<IServiceBase<DtoMarketerInventory, TMarketerInventory>>();
    }

    public async ValueTask<IEnumerable<LookupOf<string, DateTime>>> GetItemExpireDates(Identifier itemId, Identifier marketerId)
    {
        var inventory = await _inventoryService.GetAllAsAsync(s =>
                new { s.ExpireDate.Value, Display = s.ExpireDate.Value.ToDottedDate(), s.TotalQuantity, s.Ingress }
            , w => w.ItemId == itemId && w.MarketerId == marketerId && w.ExpireDate != null);
        return inventory.Items
            .GroupBy(ss => ss.Value)
            .Select(ss => new { ss.Key, Q = ss.Sum(sm => sm.Ingress ? sm.TotalQuantity : sm.TotalQuantity * -1) })
            .Where(W => W.Q > 0)
            .Select(s => new LookupOf<string, DateTime>() { Display = s.Key.ToDottedDate() + " | " + s.Q, Value = s.Key });
    }

    public ValueTask<DtoMarketerInventory?> RegainOrLoseItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool isLose = false, bool softDelete = false, StorageItemProperties? storageItemProperties = null)
    {
        return isLose
                ? softDelete
                    ? LoseItem(itemId, unitId, marketerId, quantity, totalQuantity, relatedId, relatedInventory, storageItemProperties)
                    : LossEver(itemId, unitId, marketerId, quantity, quantity, relatedId, relatedInventory, storageItemProperties: storageItemProperties)
                : RegainItem(itemId, unitId, marketerId, quantity, totalQuantity, relatedId, relatedInventory, storageItemProperties)
            ;
    }

    public async ValueTask<DtoMarketerInventory?> RegainItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null)
    {

        //_dtoInventoryService.BeginTransaction(IsolationLevel.ReadCommitted);

        var inventory = await _inventoryService.Add(new DtoMarketerInventory
        {
            ItemId = itemId,
            Ingress = true,
            UnitId = unitId,
            ExpireDate = storageItemProperties?.ExpireDate ?? null, // TODO: Fix
            MarketerId = marketerId,
            Quantity = quantity,
            RelatedId = relatedId,
            CreateDate = DateTime.UtcNow,
            TotalQuantity = totalQuantity,
            OprationDate = DateTime.UtcNow,
            RelatedIndex = relatedInventory ?? RelatedInventory.None,
        });
        return inventory;
    }

    public async ValueTask<DtoMarketerInventory?> LoseItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _inventoryService.Add(new DtoMarketerInventory
        {
            ItemId = itemId,
            Ingress = false,
            UnitId = unitId,
            ExpireDate = storageItemProperties?.ExpireDate ?? null, // TODO: Fix
            MarketerId = marketerId,
            Quantity = quantity,
            RelatedId = relatedId,
            CreateDate = DateTime.UtcNow,
            OprationDate = DateTime.UtcNow,
            TotalQuantity = totalQuantity,
            RelatedIndex = relatedInventory ?? RelatedInventory.None,
        });
        
        return inventory;
    }

    public async ValueTask<bool> CanLoseItemQuntity(Identifier itemId, Identifier unitId, Identifier marketerId, decimal qun)
    {
        var res = await GetItemQuntity(itemId, unitId, marketerId);
        if (res == null) return false;
        return res.Quantity >= qun;
    }
    
    public async ValueTask<(bool CanLoost, DtoMarketerInventory? Info)> CanLoseItemQuntityInfo(Identifier itemId, Identifier unitId, Identifier marketerId, decimal qun, Identifier? ignoredRelated = null)
    {
        var res = await GetItemQuntity(itemId, unitId, marketerId, ignoredRelated);
        if (res == null) return (false, null);
        return (res.Quantity >= qun, res);
    }
    
    public async ValueTask<DtoMarketerInventory?> GetItemQuntity(Identifier itemId, Identifier unitId, Identifier marketerId, Identifier? ignoredRealted = null)
    {
        var unitItemService = _serviceProvider.GetRequiredService<IServiceBase<DtoItemUnit, TItemUnit>>();
        var itemUnitsInfo = (await unitItemService.GetAllAsAsync(s => new { s.IsPrimary, s.TransferRate, s.UnitId, s.ItemName }, w => w.ItemId == itemId)).Items;

        var primaryUnit = itemUnitsInfo.FirstOrDefault(w => w.IsPrimary);
        var selectedUnit = itemUnitsInfo.FirstOrDefault(w => w.UnitId == unitId);
        if (primaryUnit is null || selectedUnit is null)
        {
            throw new Exception($"No primary unit for this item: {itemId.Value}");
        }
        if (selectedUnit is null)
        {
            throw new Exception($"Unit is not part of this item: {itemId.Value}");
        }

        var where = _inventoryService.Build();
        where = where.And(w => w.ItemId == itemId /*&& w.UnitId == unitId*/ && w.MarketerId == marketerId);
        if(ignoredRealted is not null)
        {
            where = where.And(w => w.RelatedId != ignoredRealted.Value);
        }

        var allMovement = await _inventoryService.GetAllAsync(where);
        //  TODO: may be some unit has been changed but still in the inventory !
        var increment = allMovement.Items
            .Where(w => w.Ingress)
            .Select(s => new { s, Plain = itemUnitsInfo.First(w => w.UnitId == s.UnitId).TransferRate * s.Quantity })
            .ToArray();
        var decrement = allMovement.Items
            .Where(w => !w.Ingress)
            .Select(s => new { s, Plain = itemUnitsInfo.First(w => w.UnitId == s.UnitId).TransferRate * s.Quantity })
            .ToArray();

        var incrementSum = increment.Sum(w => w.Plain);
        var decrementSum = decrement.Sum(w => w.Plain);
        var currentSum = incrementSum - decrementSum;
        var unitQuntity = Math.Floor(currentSum / selectedUnit.TransferRate);
        var addQuntity = currentSum % selectedUnit.TransferRate;
        return new DtoMarketerInventory
        {
            ItemId = itemId,
            UnitId = unitId,
            MarketerId = marketerId,
            Quantity = unitQuntity,
            AdditionalQuantity = addQuntity,
        };
    }

    public async ValueTask<DtoMarketerInventory?> LossEver(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool softDelete = false, StorageItemProperties? storageItemProperties = null)
    {
        var where = _inventoryService.Build();

        where = where
            .And(w => w.ItemId == itemId && w.UnitId == unitId && w.MarketerId == marketerId && w.RelatedId == relatedId && w.RelatedIndex == relatedInventory);
        if (storageItemProperties?.ExpireDate != null) where = where.And(w => w.ExpireDate == storageItemProperties.ExpireDate);

        var re = await _inventoryService.GetAsync(where);
        if (re == null) return null;
        await _inventoryService.Remove(re.Id, softDelete);
        return re;
    }

    public async Task LossAllEver(Identifier relatedId, RelatedInventory relatedInventory)
    {
        await _inventoryService.Remove(
            w =>
                w.RelatedId == relatedId && w.RelatedIndex == relatedInventory
        );
    }

    public void SetTransaction(IDbTransaction dbTransaction)
    {
        _inventoryService.BeginTransaction(dbTransaction);
    }
}