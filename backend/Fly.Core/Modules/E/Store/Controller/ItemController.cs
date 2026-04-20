using Fly.Core.Modules.E.Store.Services.InternalServices;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Fly.Core.Modules.E.Store.Controller;

public class ItemController
    : CustomControllerFeatrueBased<DtoItem, TItem, ItemQueryRequest, DeleteItemRequest, CreateUpdateItemRequest>
        , IItemFeatures, IDisposable

{
    public ItemController(ILocalizationService localizationService, ILogger<ItemController> logger, IMediator mediator, IItemService service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += ItemControllerOnCreateUpdate;
    }

    public override Expression<Func<DtoItem, bool>> GetWhere(Expression<Func<DtoItem, bool>> expression, ItemQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Name.Contains(query.SearchText) |
                x.ItemUnits.Any(u => u.ItemName.Contains(query.SearchText)) |
                x.Price.ToString().Contains(query.SearchText) |
                x.Barcode.Contains(query.SearchText) |
                x.ItemBarcodes.Any(c => c.Barcode.Contains(query.SearchText))
            );
        return base.GetWhere(expression, query);
    }

    [HttpGet("NextNumber")]
    public string GetNextString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "a"; // Start from 'a' if input is empty
        }

        char[] chars = input.ToCharArray();
        for (int i = chars.Length - 1; i >= 0; i--)
        {
            if (chars[i] == 'z')
            {
                chars[i] = 'a'; // Reset 'z' to 'a'
            }
            else
            {
                chars[i]++;
                return new string(chars);
            }
        }

        // If all characters were 'z', add 'a' at the beginning
        return "a" + new string(chars);
    }

    private async ValueTask<DtoItem> ItemControllerOnCreateUpdate(CreateUpdateItemRequest q)
    {
        //  TODO: PHOTO for item

        // get needed services
        //var unitService = _service.MakeChildService<DtoUnit, TUnit>();
        var itemUnitService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var itemBarcodeService = _service.MakeChildService<DtoItemBarcode, TItemBarcode>();
        //var storageModuleService = _service.MakeChildService<IStorageModuleService>();

        //var itemunits = q.ItemUnits.Adapt<List<DtoItemUnit>>();
        //q.ItemUnits = null;

        var item = q.Adapt<DtoItem>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (item.IsNew())
        {
            if (string.IsNullOrEmpty(item.Barcode))
            {
                var max = await _service.GetMaxAsync(w => w.Barcode);
                var nextMax = GetNextString(max);
                //var num = Convert.ToInt32(max);
                item.Barcode = nextMax;
            }

            var dbEntity = item.Adapt<TItem>();
            dbEntity.Image = q.Image;
            
            await _service.AddT(dbEntity, item);
            foreach (var u in q.ItemUnits)
            {
                var itmId = item.Id;
                var uniId = u.UnitId;
                var qun = u.CurrentQuantity;
                //  TODO: Storage = ?
                //await storageModuleService.LoseItem(itmId, uniId, strId, qun, permit.Id, RelatedInventory.Disbursement);
            }
            //itemunits.ForEach(f => f.ItemId = item.Id);
            //await itemUnitService.AddRange(itemunits);
        }
        else
        {
            var db = await _service.GetById(item.Id);
            //db.ItemUnits = null;
            await itemUnitService.Remove(w => w.ItemId == db.Id);
            var all = item.ItemUnits.ToList();
            await itemBarcodeService.Remove(w => w.ItemId == db.Id);
            var allBarcodes = item.ItemBarcodes.ToList();
            
            item.Cost = db.Cost;
            item.Adapt(db);
            item.UpdateCounter += 1;
            
            all.ForEach(f => f.ItemId = db.Id);
            allBarcodes.ForEach(f => f.ItemId = db.Id);
            await itemUnitService.AddRange(all);
            await itemBarcodeService.AddRange(allBarcodes);

            var dbEntity = item.Adapt<TItem>();
            dbEntity.Image = q.Image;
            
            await _service.UpdateT(dbEntity, db);
        }

        _service.CommitTransaction();
        return item;
    }

    [HttpGet("gategories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCategories([FromQuery] CategoryQueryRequest request)
        =>
            GetSelectableList<DtoCategory, TCategory>(x => x.Name);

    [HttpGet("units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoUnit, TUnit>(x => x.Name);

    [HttpPost("ChangePrice")]
    public ValueTask<HttpResponseWrapperList<ChangePriceRequest>> PostChangePrice(List<ChangePriceRequest> request)
        =>
        TryMeny<ChangePriceRequest>(async _ =>
        {
            await ValueTask.CompletedTask;

            var ids = request.Select(S => S.ItemId).ToList();
            var q = await _service.GetAllAsync(w => ids.Contains(w.Id));
            //var q = await _service.GetAllAsync(w => request.Any(ww => w.Id == ww.ItemId && ww.Price != w.Price));
            var tiBeChanged = q.Items.Where(w => request.Any(ww => w.Id == ww.ItemId && ww.Price != w.Price)).ToList();
            tiBeChanged.ForEach(f =>
            {
                var reqItem = request.First(w => w.ItemId == f.Id);
                f.Price = reqItem.Price;
            });
            await _service.UpdateRange(tiBeChanged);
            return _.Success(request);
        });

    [HttpGet("Image"), AllowAnonymous]
    public async ValueTask<IActionResult> PostItemsImage([FromQuery] Identifier id)
    {
        var imageBytes = await GetImageData(id);

        return File(imageBytes, "image/png");
    }

    private async ValueTask<byte[]> GetImageData(Identifier id)
    {
        byte[]? imageBytes;

        if (id.IsNullOrEmpty())
        {
            imageBytes = GetDefaultProductImage();
        }
        else
        {
            var entity = await _service.GetTById(id);
            imageBytes = entity.Image;
        }
        
        if (imageBytes == null || imageBytes.Length == 0)
        {
            imageBytes = GetDefaultProductImage();
        }

        return imageBytes;
    }

    [HttpGet("ImageQuery")]
    public ValueTask<HttpResponseWrapper<ItemQueryResponse>> PostItemsImageQuery([FromQuery] ItemQueryRequest query)
    {
        return TryOne<ItemQueryResponse>(async a =>
        {

            var data = await GetImageData(query.ItemId.GetValueOrDefault());

            return a.Success(new ItemQueryResponse
            {
                ItemImage = data
            });
        });

    }
    
    private static byte[] GetDefaultProductImage()
    {
        try
        {
            var data = System.IO.File.ReadAllText("Resources/def.data");

            if (string.IsNullOrEmpty(data))
                return [];
            
            return Convert.FromBase64String(data);
        }
        catch
        {
            return [];
        }
    }
    
    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores([FromQuery] StoreQueryRequest request)
        => GetSelectableList<DtoStore, TStore>(x => x.Name);
    
    [HttpGet("ItemsList")]
    public ValueTask<HttpResponseWrapper<ItemsSaleQueryResponse>> PostItemsList([FromBody] ItemsSaleQueryRequest request) =>
        TryOne<ItemsSaleQueryResponse>(async _ =>
        {
            var storage = _service.MakeChildService<IPurchaseAndSalesModuleService>();
            var items = await _service.GetAllAsAsync(s => s);

            var inventoryService = _service.MakeChildService<IInventoryService>();
            var list = new List<ItemsSaleDetailsQueryResponse>();
            var where = inventoryService.Build().And(w => w.RelatedIndex != RelatedInventory.Damaged
            && w.StoreId == request.StorageId.GetValueOrDefault());
            var movements = await inventoryService.GetAllAsync(where);

            foreach (var item in items.Items)
            {
                var currMovements = movements.Items.ToList().Where(s => s.ItemId == item.Id);
                var units = request.StorageId is null
                    ? item.ItemUnits.Select(s => new UnitSaleDetailsQueryResponse
                    {
                        QuantityInStorage = 0,
                        IsPrimary = s.IsPrimary,
                        UnitId = s.UnitId,
                        UnitName = s.UnitName
                    })
                    : await storage.GetItemUnitsQuantity(currMovements, item.ItemUnits.ToList(), request.StorageId.GetValueOrDefault());
                
                list.Add(new ItemsSaleDetailsQueryResponse
                {
                    ItemName = item.Name,
                    ItemId = item.Id,
                    Units = units.ToList(),
                    ItemRegionalPrice = item.RegionalPrice,
                    ItemBulkPrice = item.RegionalPrice,
                    ItemCustomPrice = item.Price,
                    ItemBarcode = item.Barcode,
                    ItemUpdateCounter = item.UpdateCounter
                });
            }
            
            /*var units = await itemUnits.GetAllAsAsync(s => s);
            var grouping = units.Items.GroupBy(s => s.ItemId)
                .Select(x => new ItemsSaleDetailsQueryResponse
                {
                    ItemId = x.Key,
                    ItemName = items.Items.FirstOrDefault(s => s.Id == x.Key)?.Name,
                    Units = x.Select(async s =>
                    {
                        decimal? quantity = 0m;

                        if (!request.StorageId.IsNullOrEmpty())
                            quantity = (await storage.GetItemQuntity(x.Key,
                                s.UnitId,
                                request.StorageId.GetValueOrDefault()))?.Inventory?.Quantity;

                        return new UnitSaleDetailsQueryResponse
                        {
                            UnitId = s.UnitId,
                            UnitName = s.UnitName,
                            QuantityInStorage = (int)(quantity ?? 0),
                            IsPrimary = s.IsPrimary
                        };
                    }).Select(a => a.Result).ToList()
                }).ToList();*/
            
            return HttpResponseWrapper<ItemsSaleQueryResponse>.Success(new()
            {
                Items = list
            });
        });

    public virtual void Dispose()
    {
        OnCreateUpdate -= ItemControllerOnCreateUpdate;
    }

}