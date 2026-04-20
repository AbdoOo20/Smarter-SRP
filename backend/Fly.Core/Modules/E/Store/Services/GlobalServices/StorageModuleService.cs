using System.Data;
using Fly.Core.Modules.E.Store.Services.InternalServices;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Dtos.Extra;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.E.Store.Services.ModuleService;

public class StorageModuleService : GlobalModuleService, IStorageModuleService
{

    //  Fields
    private readonly AutoMapper.IMapper _mapper;
    private readonly IServiceBase<DtoItemUnit, TItemUnit> itemUnitService;
    private readonly IServiceProvider _serviceProvider;
    private readonly MapsterMapper.IMapper _mapsterMapper;
    private readonly ILogger<StorageModuleService> _logger;
    private readonly IInventoryService _inventoryService;

    public StorageModuleService(ILogger<StorageModuleService> logger, AutoMapper.IMapper mapper,
        IServiceBase<DtoItemUnit, TItemUnit> itemUnitService, MapsterMapper.IMapper mapsterMapper, IServiceProvider serviceProvider)
        : base(logger, mapper, mapsterMapper, serviceProvider)
    {
        _mapper = mapper;
        this.itemUnitService = itemUnitService;
        _logger = logger;
        _mapsterMapper = mapsterMapper;
        _serviceProvider = serviceProvider;

        _inventoryService = serviceProvider.GetRequiredService<IInventoryService>();
    }

    public async ValueTask<IEnumerable<DtoInventory>> GetMovements(Identifier? itemId = null, Identifier? storeId = null, Identifier[]? relatedId = null, RelatedInventory? relatedIndex = null, StorageItemProperties? storageItemProperties = null)
    {
        if (itemId.HasValue && itemId.Value.IsNullOrEmpty()) itemId = null;
        if (storeId.HasValue && storeId.Value.IsNullOrEmpty()) storeId = null;
        if (relatedId?.Any() == false) relatedId = null;

        var w = _inventoryService.Build();
        if (itemId is not null) w = w.And(w => w.ItemId == itemId);
        if (storeId is not null) w = w.And(w => w.StoreId == storeId);
        if (relatedId is not null) w = w.And(w => relatedId.Contains(w.RelatedId.Value));
        if (relatedIndex is not null) w = w.And(w => w.RelatedIndex == relatedIndex);
        if (storageItemProperties?.ExpireDate is not null) w = w.And(w => w.ExpireDate == storageItemProperties.ExpireDate);

        var all = await _inventoryService.GetAllAsync(w);
        return all.Items;
    }

    public async ValueTask<DtoInventory?> RegainItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null)
    {

        //_dtoInventoryService.BeginTransaction(IsolationLevel.ReadCommitted);

        var inventory = await _inventoryService.Add(new DtoInventory
        {
            ItemId = itemId,
            Ingress = true,
            UnitId = unitId,
            ExpireDate = storageItemProperties?.ExpireDate ?? null, // TODO: Fix
            StoreId = storeId,
            Quantity = quantity,
            RelatedId = relatedId,
            CreateDate = DateTime.UtcNow,
            TotalQuantity = totalQuantity,
            OprationDate = DateTime.UtcNow,
            RelatedIndex = relatedInventory ?? RelatedInventory.None,
        });
        return inventory;
    }

    public async ValueTask<DtoInventory?> LoseItem(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, StorageItemProperties? storageItemProperties = null)
    {
        var inventory = await _inventoryService.Add(new DtoInventory
        {
            ItemId = itemId,
            Ingress = false,
            UnitId = unitId,
            ExpireDate = storageItemProperties?.ExpireDate ?? null, // TODO: Fix
            StoreId = storeId,
            Quantity = quantity,
            RelatedId = relatedId,
            CreateDate = DateTime.UtcNow,
            OprationDate = DateTime.UtcNow,
            TotalQuantity = totalQuantity,
            RelatedIndex = relatedInventory ?? RelatedInventory.None,
        });
        return inventory;
    }

    public async ValueTask<MoveItemsRequest> TransferItemsBetweenStoragesAsync(MoveItemsRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (request.Details == null || !request.Details.Any())
            throw new InvalidInputException("يرجى أختيار الأصناف.");

        foreach (var itemUnit in request.Details)
        {
            // Assumption: DtoItemUnit contains ItemId, UnitId, and Quantity
            var itemId = itemUnit.ItemId;
            var unitId = itemUnit.UnitId;
            var quantity = itemUnit.Quantity;
            var storageItemProperties = new StorageItemProperties(); // If present
            var selectedOne = await itemUnitService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itemId & w.UnitId == unitId);
            if (selectedOne is null)
            {
                throw new InvalidInputException("Cannot get item unit info");
            }
            bool canLose = await CanLoseItemQuntity(itemId, unitId, request.SourceLocationId, quantity,
                storageItemProperties);
            if (!canLose)
            {
                throw new InvalidInputException("لا توجد كمية كافية في هذا المخزن من الأصناف التي أخترتها." + "\n" +
                    $"Not enough quantity for ItemId {itemUnit.ItemId } (UnitId {itemUnit.UnitId}) in source storage {request.SourceLocationId}."
                );
            }

            var sourceInventory = await GetItemQuntity(itemId, unitId, request.SourceLocationId, storageItemProperties);
            decimal sourceTotalQuantity = sourceInventory?.TotalQuantity ?? 0;

            await LoseItem(
                itemId,
                unitId,
                request.SourceLocationId,
                quantity,
                quantity * selectedOne.TransferRate,
                relatedId: request.DestinationLocationId,
                relatedInventory: RelatedInventory.Transfer,
                storageItemProperties: storageItemProperties
            );

            var destInventory =
                await GetItemQuntity(itemId, unitId, request.DestinationLocationId, storageItemProperties);
            decimal destTotalQuantity = (destInventory?.TotalQuantity ?? 0) + quantity;

            await RegainItem(
                itemId,
                unitId,
                request.DestinationLocationId,
                quantity,
                quantity * selectedOne.TransferRate,
                relatedId: request.SourceLocationId, // Optionally link to source
                relatedInventory: RelatedInventory.Transfer, // Or another enum value if appropriate
                storageItemProperties: storageItemProperties
            );
        }

        return request;
    }
    
    public async ValueTask<DamageItemsRequest> MarkItemsAsDamagedAsync(DamageItemsRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (request.Details == null || !request.Details.Any())
            throw new InvalidInputException("يرجى أختيار الأصناف.");

        foreach (var itemUnit in request.Details)
        {
            // Assumption: DtoItemUnit contains ItemId, UnitId, and Quantity
            var itemId = itemUnit.ItemId;
            var unitId = itemUnit.UnitId;
            var quantity = itemUnit.Quantity;
            var storageItemProperties = new StorageItemProperties();
            var selectedOne = await itemUnitService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itemId & w.UnitId == unitId);
            if (selectedOne is null)
            {
                throw new InvalidInputException("Cannot get item unit info");
            }
            
            // 1. Check if source has enough quantity
            bool canLose = await CanLoseItemQuntity(itemId, unitId, request.SourceLocationId, quantity,
                storageItemProperties);
            if (!canLose)
            {
                throw new InvalidInputException("لا توجد كمية كافية في هذا المخزن من الأصناف التي أخترتها." + "\n" +
                    $"Not enough quantity for ItemId {itemUnit.ItemId} (UnitId {itemUnit.UnitId}) in source storage {request.SourceLocationId}."
                );
            }
            
            await LoseItem(
                itemId,
                unitId,
                request.SourceLocationId,
                quantity,
                quantity * selectedOne.TransferRate,
                relatedId: request.SourceLocationId,
                relatedInventory: RelatedInventory.Damaged,
                storageItemProperties: storageItemProperties
            );
        }

        return request;
    }
    
    public async ValueTask<bool> CanLoseItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null)
    {
        var res = await GetItemQuntity(itemId, unitId, storeId, storageItemProperties);
        if (res == null) return false;
        return res.Quantity >= qun;
    }
    public async ValueTask<(bool CanLoost, DtoInventory? Info)> CanLoseItemQuntityInfo(Identifier itemId, Identifier unitId, Identifier storeId, decimal qun, StorageItemProperties? storageItemProperties = null)
    {
        var res = await GetItemQuntity(itemId, unitId, storeId, storageItemProperties);
        if (res == null) return (false, null);
        var canLose = res.Quantity >= qun;
        return (canLose, res);
    }
    public async ValueTask<IEnumerable<LookupOf<string, DateTime>>> GetItemExpireDates(Identifier itemId, Identifier storeId)
    {
        var inventory = await _inventoryService.GetAllAsAsync(s =>
                new { s.ExpireDate.Value, s.TotalQuantity, s.Ingress }
            , w => w.ItemId == itemId && w.StoreId == storeId && w.ExpireDate != null);
        return inventory.Items
            .GroupBy(ss => ss.Value)
            .Select(ss => new { ss.Key, Q = ss.Sum(sm => sm.Ingress ? sm.TotalQuantity : sm.TotalQuantity * -1) })
            .Where(W => W.Q > 0)
            .Select(s => new LookupOf<string, DateTime>() { Display = s.Key.ToDottedDate() + " | " + s.Q, Value = s.Key });
    }
    public async ValueTask<DtoInventory?> GetItemQuntity(Identifier itemId, Identifier unitId, Identifier storeId, StorageItemProperties? storageItemProperties = null)
    {
        var unitItemService = _serviceProvider.GetRequiredService<IServiceBase<DtoItemUnit, TItemUnit>>();
        var itemUnitsInfo = (await unitItemService.GetAllAsAsync(s => new { s.IsPrimary, s.TransferRate, s.UnitId, s.ItemName }, w => w.ItemId == itemId)).Items;

        var primaryUnit = itemUnitsInfo.FirstOrDefault(w => w.IsPrimary);
        var selectedUnit = itemUnitsInfo.FirstOrDefault(w => w.UnitId == unitId);
        if (primaryUnit is null || selectedUnit is null)
        {
            throw new InvalidInputException($"No primary unit for this item {itemId.Value}");
        }
        if (selectedUnit is null)
        {
            throw new InvalidInputException($"the unit not one ot item units for this item {itemId.Value}");
        }


        var where = _inventoryService.Build();
        where = where.And(w => w.ItemId == itemId && w.RelatedIndex != RelatedInventory.Damaged /*&& w.UnitId == unitId*/ && w.StoreId == storeId);
        if (storageItemProperties?.ExpireDate != null) where = where.And(w => w.ExpireDate == storageItemProperties.ExpireDate);

        var allMovement = await _inventoryService.GetAllAsync(where);
        //  TODO: may be some unit has been changed but still in the inventory !!
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
        return new DtoInventory
        {
            ItemId = itemId,
            UnitId = unitId,
            StoreId = storeId,
            Quantity = unitQuntity,
            AdditionalQuantity = addQuntity,
            TotalQuantity = currentSum,
            ExpireDate = storageItemProperties?.ExpireDate ?? null,
            // RelatedId = primaryUnit.UnitId
        };
    }

    public async ValueTask<List<UnitSaleDetailsQueryResponse>> GetItemUnitsQuantity(IEnumerable<DtoInventory> inventory, List<DtoItemUnit> itemUnits, Identifier storeId, StorageItemProperties? storageItemProperties = null)
    {
        var itemId = itemUnits.FirstOrDefault()?.ItemId;

        if (itemId is null)
            return [];
        
        var where = _inventoryService.Build();
        where = where.And(w => w.ItemId == itemId && w.RelatedIndex != RelatedInventory.Damaged /*&& w.UnitId == unitId*/ && w.StoreId == storeId);
        if (storageItemProperties?.ExpireDate != null) where = where.And(w => w.ExpireDate == storageItemProperties.ExpireDate);
        
        return (from itemUnit in itemUnits
            let increment = inventory.Where(w => w.Ingress)
                .Select(s => new { s, Plain = itemUnits.First(w 
                    => w.UnitId == s.UnitId).TransferRate * s.Quantity })
                .ToArray()
            let decrement = inventory.Where(w => !w.Ingress)
                .Select(s => new { s, Plain = itemUnits.First(w 
                    => w.UnitId == s.UnitId).TransferRate * s.Quantity })
                .ToArray()
            let incrementSum = increment.Sum(w => w.Plain)
            let decrementSum = decrement.Sum(w => w.Plain)
            let currentSum = incrementSum - decrementSum
            let unitQuntity = Math.Floor(currentSum / itemUnit.TransferRate)
            select new UnitSaleDetailsQueryResponse
            {
                QuantityInStorage = (int)unitQuntity,
                UnitId = itemUnit.UnitId,
                IsPrimary = itemUnit.IsPrimary,
                UnitName = itemUnit.UnitName
            }).ToList();
    }
        
    public async ValueTask<DtoInventory?> LossEver(Identifier itemId, Identifier unitId, Identifier storeId, decimal quantity, decimal totalQuantity, Identifier? relatedId = null, RelatedInventory? relatedInventory = null, bool softDelete = false, StorageItemProperties? storageItemProperties = null)
    {
        var where = _inventoryService.Build();

        where = where
            .And(w => w.ItemId == itemId && w.UnitId == unitId && w.StoreId == storeId && w.RelatedId == relatedId && w.RelatedIndex == relatedInventory);

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

    public ValueTask<DtoInventory?> RegainOrLoseItem(Identifier itemId, Identifier unitId, Identifier marketerId, decimal quantity, decimal totalQuantity, Identifier relatedId, RelatedInventory relatedInventory, bool isLose, bool softDelete, StorageItemProperties? storageItemProperties = null)
    {
        return isLose
                ? softDelete
                    ? LoseItem(itemId, unitId, marketerId, quantity, totalQuantity, relatedId, relatedInventory, storageItemProperties)
                    : LossEver(itemId, unitId, marketerId, quantity, quantity, relatedId, relatedInventory, storageItemProperties: storageItemProperties)
                : RegainItem(itemId, unitId, marketerId, quantity, totalQuantity, relatedId, relatedInventory, storageItemProperties)
            ;
    }
}