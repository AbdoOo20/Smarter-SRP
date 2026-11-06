using System.Linq.Expressions;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.ItemsTransferInvoice.Controllers;

public class ItemsTransferInvoiceController
    : CustomControllerFeatrueBased<DtoItemsTransferInvoice,
            TItemsTransferInvoice, ItemsTransferInvoiceQueryRequest,
            ItemsTransferInvoiceDeleteRequest,
            ItemsTransferInvoiceCreateUpdateRequest>,
        IItemsTransferInvoiceFeatures, IDisposable
{
    private readonly IConfiguration _configuration;

    public ItemsTransferInvoiceController(ILocalizationService localizationService,
        ILogger<ItemsTransferInvoiceController> logger,
        IMediator mediator, IServiceBase<DtoItemsTransferInvoice, TItemsTransferInvoice> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster,
        IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _configuration = configuration;
        OnCreateUpdate += ItemsTransferInvoiceControllerOnCreateUpdate;
    }

    public override Expression<Func<DtoItemsTransferInvoice, bool>> GetWhere(
        Expression<Func<DtoItemsTransferInvoice, bool>> expression,
        ItemsTransferInvoiceQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.InvoiceNumber.ToString().Contains(query.SearchText) |
                x.TransferStatus.ToString().Contains(query.SearchText) |
                x.ToStorageName.Contains(query.SearchText) |
                x.FromStorageName.Contains(query.SearchText) |
                x.Details.Any(u => u.ItemName.Contains(query.SearchText))
            );
        
        return base.GetWhere(expression, query);
    }

    [HttpGet("Number")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber()
        =>
            TryOne<long>(async (opt) =>
            {
                var next = await _service.NextNumber(x => x.InvoiceNumber);
                return opt.Success(next);
            });
    
    private async ValueTask<DtoItemsTransferInvoice> ItemsTransferInvoiceControllerOnCreateUpdate(ItemsTransferInvoiceCreateUpdateRequest q)
    {
        var maintenanceDetailService = _service.MakeChildService<DtoItemsTransferInvoiceDetail, TItemsTransferInvoiceDetail>();
        var mainInvoice = q.Adapt<DtoItemsTransferInvoice>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var itemUnitsService = _service.MakeChildService<DtoItemUnit, TItemUnit>();

        if (mainInvoice.IsNew())
        {
            await _service.Add(mainInvoice);
            switch (mainInvoice.TransferStatus)
            {
                case ItemsTransferRequestStatus.AwaitingConfirmation:
                    break;
                case ItemsTransferRequestStatus.OnTheWay:
                    await RemoveFromFirstStorageAsync();
                    break;
                case ItemsTransferRequestStatus.Completed:
                    await InventoryProcess(mainInvoice.Details, mainInvoice.FromStorageId, true);
                    await InventoryProcess(mainInvoice.Details, mainInvoice.ToStorageId);
                    break;
                case ItemsTransferRequestStatus.Cancelled:
                    throw new InvalidInputException("لا يمكن أنشاء فاتورة جديدة ملغاة.");
            }
        }
        else
        {
            var db = await _service.GetById(mainInvoice.Id);
            var oldDb = db.DeepCopyShared();
            await maintenanceDetailService.Remove(w => w.ItemsTransferInvoiceId == db.Id, false);
            var all = mainInvoice.Details.ToList();
            mainInvoice.Adapt(db);
            all.ForEach(f => f.ItemsTransferInvoiceId = db.Id);
            await maintenanceDetailService.AddRange(all);
            await _service.Update(mainInvoice);

            var statusInDb = oldDb.TransferStatus;

            if (oldDb.FromStorageId != mainInvoice.FromStorageId)
            {
                throw new InvalidInputException(
                    "لا يمكنك تغيير المخزن الذي تنقل منه ألأصناف بعد أنشاء الفاتورة. يرجى أرجاع المخزن الى: " + oldDb.FromStorageName);
            }
            
            if (oldDb.ToStorageId != mainInvoice.ToStorageId)
            {
                throw new InvalidInputException(
                    "لا يمكنك تغيير المخزن الذي تنقل أليه ألأصناف بعد أنشاء الفاتورة. يرجى أرجاع المخزن الى: " + oldDb.ToStorageName);
            }
            
            switch (statusInDb)
            {
                case ItemsTransferRequestStatus.AwaitingConfirmation:
                    if (mainInvoice.TransferStatus == ItemsTransferRequestStatus.OnTheWay)
                    {
                        // Remove items from storage 1
                        await RemoveFromFirstStorageAsync();
                    }
                    else if (mainInvoice.TransferStatus == ItemsTransferRequestStatus.Cancelled)
                    {
                        // Do nothing
                    }
                    else if(mainInvoice.TransferStatus == ItemsTransferRequestStatus.Completed)
                    {
                        // Move the items: take them out of storage 1 and add them to storage 2.
                        // (Before, only storage 2 was increased, so the items were duplicated.)
                        await RemoveFromFirstStorageAsync();
                        await InventoryProcess(mainInvoice.Details, mainInvoice.ToStorageId);
                    }
                    break;

                case ItemsTransferRequestStatus.OnTheWay:
                    if (mainInvoice.TransferStatus == ItemsTransferRequestStatus.Completed)
                    {
                        await InventoryProcess(mainInvoice.Details, mainInvoice.ToStorageId);
                    }
                    else if (mainInvoice.TransferStatus == ItemsTransferRequestStatus.Cancelled)
                    {
                        // Add items back to storage 1
                        await InventoryProcess(mainInvoice.Details, mainInvoice.FromStorageId);
                    }
                    else if(mainInvoice.TransferStatus == ItemsTransferRequestStatus.AwaitingConfirmation)
                    {
                        throw new InvalidInputException("هذه الفاتورة تمت الموافقة عليها مسبقاً ولا يمكن الرجوع الى هذه الحالة.");
                    }
                    break;

                case ItemsTransferRequestStatus.Completed:
                    if (mainInvoice.TransferStatus == ItemsTransferRequestStatus.Cancelled)
                    {
                        // Roll back items from storage 2 to storage 1
                        await InventoryProcess(mainInvoice.Details, mainInvoice.ToStorageId, true);
                        await InventoryProcess(mainInvoice.Details, mainInvoice.FromStorageId);
                    }
                    else
                    {
                        throw new InvalidInputException("هذه الفاتورة مكتملة. يمكنك ألغاء العملية لأرجاع الأصناف الى المخزن الأولي, فقط.");
                    }
                    break;

                case ItemsTransferRequestStatus.Cancelled:
                    throw new InvalidInputException("هذه الفاتورة تم ألغائها مسبقا. لا يمكن التعديل عليها, يمكنك عمل فاتورة جديدة لعمل أي تغييرات اخرى.");

                default:
                    throw new InvalidInputException("Unknown status.");
            }

            // (An early `return` here skipped CommitTransaction, so edits to a transfer
            //  that was still awaiting confirmation were never saved.)
        }

        _service.CommitTransaction();

        return mainInvoice;

        async Task InventoryProcess(IEnumerable<DtoItemsTransferInvoiceDetail> invoiceDetails, Identifier storageId, bool isLose = false, bool softDelete = true)
        {
            Logger.LogTrace("Items updating : {0}, IsLose: {1}", invoiceDetails.Count(), isLose);
            foreach (var item in invoiceDetails)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var strId = storageId;
                var qun = item.Quantity;

                var selectedOne = await itemUnitsService.GetOneAsAsync(w => new { w.ItemId, w.UnitId, w.TransferRate }, w => w.ItemId == itmId & w.UnitId == untId);
                if (selectedOne is null)
                {
                    Logger.LogTrace("Cannot get item unit info ==> unitid:{0}, unitId:{1}", untId, itmId);
                    throw new InvalidInputException("Cannot get item unit info");
                }

                var itemInfo = await itemService.GetOneAsAsync(s => s, w => w.Id == itmId);

                // Only check available quantity when taking items OUT of a store. Checking it
                // when adding to the destination store made every completed transfer fail.
                if (isLose)
                {
                    var minusSellConfig = _configuration.GetSection("MinusSellCheck");
                    if (minusSellConfig is null || minusSellConfig.Get<bool>())
                    {
                        var quntityInfo = await storageModuleService.CanLoseItemQuntityInfo(itmId, untId, strId.Value, qun, new());
                        var errQaun = (quntityInfo.Info?.Inventory?.Quantity is not null) ? "أقل من " +
                            quntityInfo.Info?.Inventory?.Quantity.ToString() : " غير متوفرة";
                        if (!quntityInfo.CanLoost)
                            throw new InvalidInputException($"لا يمكن أزالة الصنف " +
                                                            $" {itemInfo?.Name ?? "بدون اسم"}. " +
                                                            $"لان الكمية المطلوبة غير كافية في المخزن:" +
                                                            $"{qun} " +
                                                            $" {errQaun}");
                    }
                }

                var allOnes = selectedOne.TransferRate * qun;

                await storageModuleService.RegainOrLoseItem(itmId, untId, strId.Value, qun, allOnes,
                    cost: itemInfo.Cost,
                    avragePiceCost: itemInfo.Price / selectedOne.TransferRate,
                    price: itemInfo.Price,
                    relatedId: mainInvoice.Id,
                    relatedInventory: RelatedInventory.Transfer,
                    isLose: isLose,
                    softDelete: softDelete,
                    storageItemProperties: new()
                );
            }
        }

        async ValueTask RemoveFromFirstStorageAsync()
        {
            await InventoryProcess(mainInvoice.Details, mainInvoice.FromStorageId, true);
        }
    }

    /// <summary>
    /// Deleting a transfer must undo its stock movements in both stores.
    /// </summary>
    protected override async ValueTask AfterDelete(DtoItemsTransferInvoice dto, ItemsTransferInvoiceDeleteRequest command, Fly.Framework.Server.Common.ResultOrFailer<DtoItemsTransferInvoice?> opt)
    {
        var storageModuleService = _service.MakeChildService<IPurchaseAndSalesModuleService>();
        var detailService = _service.MakeChildService<DtoItemsTransferInvoiceDetail, TItemsTransferInvoiceDetail>();
        await storageModuleService.LossAllEver(dto.Id, [RelatedInventory.Transfer]);
        await detailService.Remove(w => w.ItemsTransferInvoiceId == dto.Id);
    }

    public virtual void Dispose()
    {
        OnCreateUpdate -= ItemsTransferInvoiceControllerOnCreateUpdate;
    }

    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(
        [FromQuery] StoreQueryRequest request)
        => GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(
        [FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name} • {x.Barcode}",
            request.ItemId != null
                ? x => x.CategoryId == request.ItemId.Value && x.IsActive
                : y => y.IsActive);

    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(
        [FromQuery] UnitQueryRequest request)
        => GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));
}