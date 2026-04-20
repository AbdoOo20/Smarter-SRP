using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Framework.Shared.Common;
using Mapster;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.Marketers.Features;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.PurchaseAndSales;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Shared.Modules.Store.Enums;
using System.Linq.Expressions;
using Fly.Core.Modules.Global.Models.DbModels;
using LinqKit;
using Fly.Framework.Server.Common.Localization;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Microsoft.Extensions.Configuration;
using Fly.Shared.Modules.Accounting.Enums;

namespace Fly.Core.Modules.F.Marketers.Controller;

public class MarketerSalesInvoiceController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoMarketerSalesInvoice, TMarketerSalesInvoice, MarketerSalesInvoiceQueryRequest, MarketerDeleteSalesInvoiceRequest, MarketerSalesInvoiceCreateRequest>,
        IMarketerSalesInvoiceFeatures
{
    private readonly IConfiguration configuration;

    public MarketerSalesInvoiceController(ILocalizationService localizationService, ILogger<MarketerSalesInvoiceController> logger,
                                          IMediator mediator,
                                          IServiceBase<DtoMarketerSalesInvoice, TMarketerSalesInvoice> service,
                                          AutoMapper.IMapper mapper,
                                          MapsterMapper.IMapper mapster, IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += SalesInvoiceController_OnCreateUpdate;
        //OnCreateUpdateEx += MarketerSalesInvoiceController_OnCreateUpdateEx;
        this.configuration = configuration;
    }

    [NonAction]
    public override Expression<Func<DtoMarketerSalesInvoice, bool>> GetWhere(Expression<Func<DtoMarketerSalesInvoice, bool>> expression, MarketerSalesInvoiceQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Details.Any(d => d.UnitName.Contains(query.SearchText)) |
                x.Details.Any(d => d.ItemName.Contains(query.SearchText)) |
                x.Number.ToString().Contains(query.SearchText) |
                x.MarketerName.Contains(query.SearchText) |
                x.TotalAmount.ToString() == query.SearchText |
                x.ClientName.Contains(query.SearchText)
            );
        return base.GetWhere(expression, query);
    }


    [NonAction]
    private async ValueTask<DtoMarketerSalesInvoice> SalesInvoiceController_OnCreateUpdate(MarketerSalesInvoiceCreateRequest q)
    {
        //  TODO:
        // Validate
        //      1. SupplierId
        //      2. SafeId && PeymentType == PeymentType.Cash
        // Prepare object
        // Save
        // Prepare Finacial Transaction
        // Prepare recipt
        // items 
        // Inventory items

        var safeService = _service.MakeChildService<DtoSafe, TSafe>();
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var unitItemService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var clientService = _service.MakeChildService<DtoClient, TClient>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var marketerSalesInvoiceDetailService = _service.MakeChildService<DtoMarketerSalesInvoiceDetail, TMarketerSalesInvoiceDetail>();
        var marketerService = _service.MakeChildService<DtoMarketer, TMarketer>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var entryService = _service.MakeChildService<IEntryService>();

        var ___x = PrepareNow();
        var userId = Helper.GetUserId();
        var phone = Helper.GetPhone();
        var senderMarketer = await marketerService.GetAsync(x => x.Phone == phone);
        if (senderMarketer is null)
            throw new Exception("marketer account not exists"); // TODO: Localization

        var items = q.Details.Select(w => w.ItemId).Distinct().ToArray();
        var itemsCache = await itemService.GetAllAsAsync(s => new { s.Price, s.Id, s.Name }, w => items.Contains(w.Id));
        var unitItemsCache = await unitItemService.GetAllAsAsync(s => new { s.IsPrimary, s.UnitId, s.TransferRate, s.ItemId }, w => items.Contains(w.ItemId));

        var sellInvoice = q.Adapt<DtoMarketerSalesInvoice>();
        var total = 0m;
        sellInvoice.Details.ForEach(f =>
        {
            var itemInfo = itemsCache.Items.FirstOrDefault(w => w.Id == f.ItemId);
            f.Price = itemInfo.Price;
            total += f.Quantity * (f.Price - f.Discount);
        });
        sellInvoice.MarketerId = senderMarketer.Id;
        sellInvoice.TotalAmount = sellInvoice.Details.Sum(s => s.Quantity * (s.Price));

        //  
        //  CHECKS
        if (sellInvoice.Details?.Any() == false)
        {
            throw new Exception("Invoice cannot be empty"); // TODO: Localization
        }
        var clientInfo = await clientService.GetOneAsAsync(x => new { x.AccountId, x.Id }, w => w.AccountId == sellInvoice.ClientId);
        if (clientInfo is null)
            throw new Exception("client account not exists"); // TODO: Localization
        sellInvoice.ClientId = clientInfo.Id;

        var marckterInfo = await marketerService.GetOneAsAsync(x => new { x.AccountId }, w => w.Id == sellInvoice.MarketerId);
        if (marckterInfo is null)
            throw new Exception("marckter account not exists"); // TODO: Localization

        var saleAccountId = await configService.GetOneAsAsync(x => x.MarketerInvoicesAccountId);
        if (saleAccountId is null)
            throw new Exception("Marketer Sales Invoice account not ready"); // TODO: Localization

        var generalClientId = await configService.GetOneAsAsync(x => x.GeneralClientAccountAccount);
        if (generalClientId is null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization

        var saleInvoiceBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(saleAccountId));
        if (saleInvoiceBaseAccount is null)
            throw new Exception("SalesInvoicer account not ready"); // TODO: Localization

        var generalClientAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(generalClientId));
        if (generalClientAccount is null)
            throw new Exception("GeneralClientAccountAccount account not ready"); // TODO: Localization

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        var invoiceNumber = 0L;
        if (q.IsNew())
        {
            sellInvoice.ClientId = sellInvoice.ClientId?.Value == Identifier.Empty ? null : sellInvoice.ClientId.Value;

            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            // todo: discount
            sellInvoice.Date = DateTime.Now;
            sellInvoice.Number = invoiceNumber;
            sellInvoice.TotalAmount = total;

            await _service.Add(sellInvoice);
            await InventiryProcess(sellInvoice.Details);
            await FinacialProcess(total);


        }
        else
        {
            var old = await _service.GetById(sellInvoice.Id) ?? throw new Exception("Cannot update this invoice");

            sellInvoice.Number = old.Number;
            invoiceNumber = old.Number;
            Logger.LogInformation("updated {0} with id {1}", nameof(DtoMarketerSalesInvoice), sellInvoice.Id);
            await _service.Update(sellInvoice);

            Logger.LogInformation("removing details {0} for id {1}", nameof(DtoMarketerSalesInvoiceDetail), sellInvoice.Id);

            await marketerSalesInvoiceDetailService.Remove(w => w.MarketerSalesInvoiceId == sellInvoice.Id);
            sellInvoice.Details.ForEach(f => f.MarketerSalesInvoiceId = sellInvoice.Id);
            await marketerSalesInvoiceDetailService.AddRange(sellInvoice.Details);

            await marketerModuleService.LossAllEver(q.Id, RelatedInventory.MarketerMobile_SellInvoice);
            await entryService.RemoveAllRelated(sellInvoice.Id, OprationTypeEnum.Marketer, PurchaseAndSalesConstants.TransactionOprationType__MarkterSalesRelated);

            await InventiryProcess(sellInvoice.Details);
            await FinacialProcess(total);

        }

        var status = _service.CommitTransaction();
        if (!status)
            throw new InvalidInputException("حدث خطأ عند محاولة أكمال عملية البيع."); // TODO localization
        else
            return sellInvoice;

        async Task InventiryProcess(IEnumerable<DtoMarketerSalesInvoiceDetail> details)
        {
            foreach (var item in details)
            {
                var itmId = item.ItemId;
                var untId = item.UnitId;
                var qun = item.Quantity;
                var itemInfo = itemsCache.Items.First(w => w.Id == itmId);
                var unitInfo = unitItemsCache.Items.Where(w => w.ItemId == itmId).ToArray();
                var selectedUnit = unitInfo.FirstOrDefault(w => w.UnitId == untId);
                if (selectedUnit is null) throw new Exception();
                var totalQuanitiy = selectedUnit.TransferRate * qun;

                var minusSellConfig = configuration.GetSection("MinusSellCheck");
                if (minusSellConfig is null || minusSellConfig.Get<bool>())
                {
                    var canLose = await marketerModuleService.CanLoseItemQuntity(itmId, untId, sellInvoice.MarketerId, qun);
                    if (!canLose) throw new InvalidInputException($"Can not sale the item {itemInfo.Name}.");
                }

                await marketerModuleService.LoseItem(itmId, untId, sellInvoice.MarketerId, qun, totalQuanitiy, sellInvoice.Id, RelatedInventory.MarketerMobile_SellInvoice);
            }
        }
        async Task FinacialProcess(decimal total)
        {
            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now                    
                    OprationTypeEnum.Marketer,
                    PurchaseAndSalesConstants.TransactionOprationType__MarkterSalesRelated,
                    sellInvoice.Id,
                    invoiceNumber,
                   "",
                   TagTypeEnum.MarketerOnly);
            var there = clientInfo?.AccountId ?? generalClientAccount.Id;
            finacialTransaction.AppendEntry(
                debit: there,
                credit: marckterInfo.AccountId,
                ammount: total);

            switch (sellInvoice.PeymentType)
            {
                case Shared.Modules.PurchaseAndSales.Enums.PeymentType.Cash:
                    finacialTransaction.AppendEntry(marckterInfo.AccountId, there, total);
                    break;
                case Shared.Modules.PurchaseAndSales.Enums.PeymentType.Deferred:
                    break;
            }

            accountingFinacialService = await finacialTransaction.ComplateTransaction();
        }
    }

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        =>
        GetSelectableList<DtoItem, TItem>(x => x.Name, y => y.IsActive);

    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty));

    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores([FromQuery] StoreQueryRequest request)
        =>
        GetSelectableList<DtoStore, TStore>(x => x.Name);

    [HttpGet("Clients")]
    public async ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupClients([FromQuery] ClientQueryRequest request)
    {
        var marketer = await GetCurrentMarketerAsync();
        if(marketer is null)
            return HttpResponseWrapperList<DefaultLookup>.Problem("User not a marketer.");
        
        return await GetSelectableList<DtoClient, TClient>(x => x.Name,
            y => marketer.MarketerClients.Any(c => c.ClientId == y.Id));
    }

    private async Task<DtoMarketer?> GetCurrentMarketerAsync()
    {
        var phone = Helper.GetPhone();
        var marketerService = _service.MakeChildService<DtoMarketer, TMarketer>();
        return await marketerService.GetAsync(x => x.Phone == phone);
    }
    
    private async Task<DtoUser?> GetCurrentMarketerUserAsync()
    {
        var marketer = await GetCurrentMarketerAsync();

        if (marketer is null)
            return null;
        
        var user = _service.MakeChildService<DtoUser, TUser>();
        return await user.GetAsync(x => x.Phone == marketer.Phone);
    }
    
    [HttpGet("Safes")]
    public async ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupSafes([FromQuery] SafeQueryRequest request)
    {
        var user = await GetCurrentMarketerUserAsync();
        if(user is null)
            return HttpResponseWrapperList<DefaultLookup>.Problem("User not a marketer.");

        return await GetSelectableList<DtoSafe, TSafe>(x => x.Name,
            y => user.VisibleSafes.Contains(y.Id.ToString()));
    }

    [HttpPost("ItemInfo")]
    public ValueTask<HttpResponseWrapper<ItemInfoQueryResponse>> GetItemInfoQuery(
        [FromBody] GetItemInfoQueryRequest queryRequest)
    {
        return TryOne<ItemInfoQueryResponse>(async opt =>
        {
            var storageService = opt.GetService<IStorageModuleService>();

            var inventoryInfo = await storageService.GetItemQuntity(queryRequest.ItemId, queryRequest.UnitId, queryRequest.StoreId);

            return opt.Success(new ItemInfoQueryResponse
            {
                Price = 0,
                Quntity = inventoryInfo.Quantity,
                AdditionalQuntity = inventoryInfo.AdditionalQuantity,
            });
        });
    }
    
    [NonAction]
    public virtual void Dispose()
    {
        OnCreateUpdate -= SalesInvoiceController_OnCreateUpdate;
    }
}
