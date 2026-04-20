using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Common.Localization;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Invoices;
using Fly.Core.Modules.F.Marketers.Services.GlobalServices;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Enums;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Mapster;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.Extensions.Configuration;
using LinqKit;
using Microsoft.AspNetCore.Mvc;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class DamagedInvoiceController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoDamagedInvoice, TDamagedInvoice, DamagedInvoiceQueryRequest, DamagedInvoiceDelete, DamagedInvoiceCreateUpdate>,
        IDamagedInvoiceFeatures
{

    private readonly IConfiguration _configuration;


    public DamagedInvoiceController(ILocalizationService localizationService, ILogger<DamagedInvoiceController> logger, IMediator mediator, IServiceBase<DtoDamagedInvoice, TDamagedInvoice> service, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapster, IConfiguration configuration) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _configuration = configuration;

        OnCreateUpdateEx += DamagedInvoiceController_OnCreateUpdateEx;
    }

    private ValueTask<HttpResponseWrapper<DtoDamagedInvoice?>> DamagedInvoiceController_OnCreateUpdateEx(DamagedInvoiceCreateUpdate q, Framework.Server.Common.ResultOrFailer<DtoDamagedInvoice?> opt)
    => TryOne<DtoDamagedInvoice?>(async (opt) =>
    {
        //
        //  PREPARE SERVICES
        var itemService = _service.MakeChildService<DtoItem, TItem>();
        var marketerService = _service.MakeChildService<DtoMarketer, TMarketer>();
        var unitItemService = _service.MakeChildService<DtoItemUnit, TItemUnit>();
        var marketerModuleService = _service.MakeChildService<IMarketerModuleService>();
        var detailsService = _service.MakeChildService<DtoDamagedInvoiceDetail, TDamagedInvoiceDetail>();

        //
        //  PREPARE REQUEST
        var ___x = PrepareNow();
        var userId = Helper.GetUserId();
        var phone = Helper.GetPhone();
        var senderMarketer = await marketerService.GetAsync(x => x.Phone == phone);
        if (senderMarketer is null)
            return opt.Problem(statusCodes: System.Net.HttpStatusCode.BadRequest, "marketer account not exists"); // TODO: Localization
        var items = q.Details.Select(w => w.ItemId).Distinct().ToArray();
        var itemsCache = await itemService.GetAllAsAsync(s => new { s.Price, s.Id, s.Name }, w => items.Contains(w.Id));
        var unitItemsCache = await unitItemService.GetAllAsAsync(s => new { s.IsPrimary, s.UnitId, s.TransferRate, s.ItemId }, w => items.Contains(w.ItemId));

        var invoice = q.Adapt<DtoDamagedInvoice>();
        invoice.StoreId = null;
        invoice.MarketerStoreId = senderMarketer.Id;

        //
        //  CHECKS
        if (invoice.Details?.Any() == false) return opt.Problem("Invoice cannot be empty"); // TODO: Localization
        var invoiceNumber = 0L;
        //
        //
        //  PREPARE TRANSACTION
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        if (q.IsNew())
        {
            var lastNumber = await _service.GetMaxAsync(c => c.Number);
            invoiceNumber = lastNumber + 1;
            invoice.Date = DateTime.Now;
            invoice.Number = invoiceNumber;

            await _service.Add(invoice);
            await InventiryProcess(invoice.Details);
        }
        else
        {
            var old = await _service.GetById(invoice.Id);
            if (old is null) return opt.Problem("Cannot update this invoice");

            invoice.Number = old.Number;
            invoiceNumber = old.Number;
            await _service.Update(invoice);

            await detailsService.Remove(w => w.DamagedInvoiceId == invoice.Id);
            invoice.Details.ForEach(f => f.DamagedInvoiceId = invoice.Id);
            await detailsService.AddRange(invoice.Details);

            await marketerModuleService.LossAllEver(q.Id, RelatedInventory.MarketerMobile_DamageInvoice);
            await InventiryProcess(invoice.Details);
        }

        //  
        //  CLOSE TRANSACTION
        var status = _service.CommitTransaction();
        if (!status)
            throw new Exception("cannot proccesed the sale opration");
        else
            return opt.Success(invoice);

        //
        //  OTHER FUNCTIONS
        async Task InventiryProcess(IEnumerable<DtoDamagedInvoiceDetail> details)
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

                var minusSellConfig = _configuration.GetSection("MinusSellCheck");
                if (minusSellConfig is null || minusSellConfig.Get<bool>())
                {
                    var canLose = await marketerModuleService.CanLoseItemQuntity(itmId, untId, invoice.MarketerStoreId.Value, qun);
                    if (!canLose) throw new InvalidInputException($"Can not sale the item {itemInfo.Name}.");
                }

                await marketerModuleService.LoseItem(itmId, untId, invoice.MarketerStoreId.Value, qun, totalQuanitiy, invoice.Id, RelatedInventory.MarketerMobile_DamageInvoice);
            }
        }
    });


    [HttpPost("SimbleDamge")]
    public ValueTask<HttpResponseWrapper<DtoDamagedInvoice?>> PostSimbleDamge(DamagedInvoiceDetailCreateUpdate q)
    => TryOne<DtoDamagedInvoice?>((opt) =>
    {
        var invoice = new DamagedInvoiceCreateUpdate()
        {
            Date = DateTime.Now,
            Details = [q],
        };
        return DamagedInvoiceController_OnCreateUpdateEx(invoice, opt);
    });

}

