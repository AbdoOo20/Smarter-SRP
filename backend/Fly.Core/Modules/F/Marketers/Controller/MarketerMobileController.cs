using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Modules.F.Marketers.Services.InternalServices;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Common.Controllers.Helpers;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Server.Security.Algorithms;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Framework.Shared.Modules.Identity.Dtos;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.F.Marketers.Controller;

[Authorize]
public class MarketerMobileController
        : CustomControllerBase, IMarketerMobileFeatures
{

    private ClaimsHelper hlper;
    private ICustomEncryptionAlgorithm? encryptionAlgorithm;
    private readonly IServiceBase<DtoMarketer, TMarketer> _service;
    private readonly IServiceBase<DtoMarketerSalesInvoice, TMarketerSalesInvoice> _marketerSalesInvoiceService;
    private readonly Identifier ExpenseAccountId = Identifier.Parse("0E54960C-8A5B-473D-8022-C4A47C65185F");

    public MarketerMobileController(
        ILocalizationService localizationService, ILogger<MarketerMobileController> logger,
        IMediator mediator,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster,
        IServiceBase<DtoMarketer, TMarketer> service,
        IServiceBase<DtoMarketerSalesInvoice, TMarketerSalesInvoice> marketerSalesInvoiceService) : base(localizationService, logger, mediator, mapper, mapster)
    {
        _service = service;
        _marketerSalesInvoiceService = marketerSalesInvoiceService;

    }

    private bool prepared = false;
    private ClaimsHelper? PrepareNow()
    {
        if (prepared) return hlper;

        encryptionAlgorithm = Prepare();
        if (encryptionAlgorithm is not null)
        {
            hlper = new ClaimsHelper(encryptionAlgorithm, User);
            prepared = true;
            return hlper;
        }
        return null;
    }

    private async Task<DtoMarketer?> GetConnectedMarketer()
    {
        if (PrepareNow() is null)
            return null;
        var userId = hlper.GetUserId();
        var phone = hlper.GetPhone(); // Map be bypassed
        var mrktr = await _service.GetAsync(x => x.Phone == phone);
        return mrktr;
    }


    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetSelecListAsync(textSelector, where)));

    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, Identifier> idSelector, Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetDefaultLookupAsync(idSelector, textSelector, where)));


    #region Get

    [HttpGet("ExcnceAccounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetExcnceAccounts([FromQuery] GetAccountsRequest request)
    =>
        TryMeny<DefaultLookup>(async opt =>
        {
            if (PrepareNow() is null)
                return opt.Problem("401");

            var mrktr = await GetConnectedMarketer();
            if (mrktr is null)
                return opt.Problem("401");

            return await GetSelectableList<DtoAccount, TAccount>(x => x.Id, x => x.Name,
                w =>
                    w.ParentId == ExpenseAccountId
            // && w.TenantId == mrktr.Id
            );
        });

    [HttpGet("Accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts([FromQuery] GetAccountsRequest request)
    =>
        GetSelectableList<DtoAccount, TAccount>(x => x.Id, x => x.Name);

    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetClients([FromQuery] ClientQueryRequest request)
    =>
        GetSelectableList<DtoClient, TClient>(x => x.AccountId, x => x.Name);

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetItems([FromQuery] ItemQueryRequest request)
    =>
        GetSelectableList<DtoItem, TItem>(x => x.Name, y => y.IsActive);

    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetUnits([FromQuery] UnitQueryRequest request)
    =>
        GetSelectableList<DtoItemUnit, TItemUnit>(k => k.UnitId, x => x.UnitName!, w => w.ItemId == request.ItemId);


    #endregion

    /*
     * 1- Get all invoices          | GET  /MarketerMobile/
     * 2- Make new saleINvoice      | POST /MarketerSalesInvoice/ {MarketerSalesInvoiceCreateRequest}
     */


    [HttpGet("TodyInvoices")]   // (1)
    public ValueTask<HttpResponseWrapperList<DtoMarketerSalesInvoice>> GetTodyInvoices([FromQuery] MarketerSalesInvoiceQueryRequest queryRequest)
    =>
        TryMeny<DtoMarketerSalesInvoice>(async opt =>
        {
            if (PrepareNow() is null)
                return opt.Problem("401");

            var mrktr = await GetConnectedMarketer();
            if (mrktr is null)
            {
                return opt.Problem("الرجاء إعادة تسجيل الدخول أو الاتصال بالدعم الفني"); // TODO: localize
            }

            queryRequest.FromDate ??= DateTime.Today;
            queryRequest.ToDate ??= DateTime.Today.AddDays(1);

            var invoicess = await _marketerSalesInvoiceService
            .GetAllAsync(
                w =>
                    w.MarketerId == mrktr.Id && (w.Date >= queryRequest.FromDate && w.Date <= queryRequest.ToDate),
                page: queryRequest.Page, limit: queryRequest.Liimit
            );
            return opt.Success(invoicess);
        });


    [HttpPost("Reciept")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostReciept([FromBody] ClientFinancialRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            if (PrepareNow() is null)
                return opt.Problem("401");

            var mrktr = await GetConnectedMarketer();
            if (mrktr is null)
                return opt.Problem("401");

            var entryService = (EntryService)opt.GetService<IEntryService>();
            var entryRequest = new RSimpleRequest()
            {
                Amount = request.Amount,
                FromAccount = request.FromAccount,
                Date = request.Date,
                Note = request.Note,
                ToAccount = mrktr.AccountId,    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                OprationTypeEnum = OprationTypeEnum.ClientFinancial,
            };

            var entry = await entryService.AddSimpleEntry(entryRequest, tagType: TagTypeEnum.MarketerOnly);
            if (entry is null) return opt.Problem("Cannot process payment");
            return opt.Success(new DtoStatment()
            {
                Debit = 0,
                Credit = 0,
                Balance = 0,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
            });
        });


    [HttpPost("Payment")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostPayment([FromBody] ClientFinancialRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            if (PrepareNow() is null)
                return opt.Problem("401");

            var mrktr = await GetConnectedMarketer();
            if (mrktr is null)
                return opt.Problem("401");

            var entryService = (EntryService)opt.GetService<IEntryService>();
            var entryRequest = new RSimpleRequest()
            {
                Amount = request.Amount,
                FromAccount = request.FromAccount,
                Date = request.Date,
                Note = request.Note,
                ToAccount = mrktr.AccountId,    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                OprationTypeEnum = OprationTypeEnum.ClientFinancial,
            };

            var entry = await entryService.AddSimpleEntry(entryRequest, tagType: TagTypeEnum.MarketerOnly);
            if (entry is null) return opt.Problem("Cannot process payment");
            return opt.Success(new DtoStatment()
            {
                Debit = 0,
                Credit = 0,
                Balance = 0,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
            });
        });


    [HttpPost("Excnce")]//
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostExcnce(ExpenseFinancialRequest request)
    => TryOne<DtoStatment>(async opt =>
    {
        if (PrepareNow() is null)
            return opt.Problem("401");

        var mrktr = await GetConnectedMarketer();
        if (mrktr is null)
            return opt.Problem("401");

        var entryService = (EntryService)opt.GetService<IEntryService>();
        var entryRequest = new RSimpleRequest()
        {
            Amount = request.Amount,
            FromAccount = mrktr.AccountId,
            Date = request.Date,
            Note = request.Note,
            //  "0E54960C-8A5B-473D-8022-C4A47C65185F"
            ToAccount = request.ToAccount,    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            OprationTypeEnum = OprationTypeEnum.ClientFinancial,
        };

        var entry = await entryService.AddSimpleEntry(entryRequest, tagType: TagTypeEnum.MarketerOnly);
        if (entry is null) return opt.Problem("Cannot process payment");
        return opt.Success(new DtoStatment()
        {
            Debit = 0,
            Credit = 0,
            Balance = 0,
            Notes = entry.Note,
            AccountName = entry.Details[0].AccountName,
        });
    });


    [HttpPost("HardFailing")]//
    public ValueTask<HttpResponseWrapper<MarketerFailingPermitCreateUpdateRequest>> PostHardFailing(MarketerFailingPermitCreateUpdateRequest request)
    => TryOne<MarketerFailingPermitCreateUpdateRequest>(async opt =>
    {
        //return opt.Success(request);
        return opt.Problem("Not Implemented");
    });


    [HttpPost("Lower")]//
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostLower(DiscountItemBasedRequest request)
   => TryOne<DtoStatment>(async opt =>
   {
       //return opt.Success(request.Adapt<DtoStatment>());
       return opt.Problem("Not Implemented");
   });


    [HttpPost("SampleFailing")]
    public ValueTask<HttpResponseWrapper<MarketerFailingPermitCreateUpdateRequest>> PostSampleFailing(MarketerFailingPermitDetail request)
     => TryOne<MarketerFailingPermitCreateUpdateRequest>(async opt =>
     {
         await Task.CompletedTask;
         //return opt.Success(new MarketerFailingPermitCreateUpdateRequest()
         //{
         //    Details = new List<MarketerFailingPermitDetail>()
         //    {
         //        request,
         //    }
         //});
         return opt.Problem("Not Implemented");
     });

    [HttpGet("StorageReport")]
    public ValueTask<HttpResponseWrapperList<DtoMarketerInventory>> GetStorageReport()
    => TryMeny<DtoMarketerInventory>(async opt =>
    {
        var helper = PrepareNow();
        var result = new List<DtoMarketerInventory>();

        var marketerInfo = User.HasClaim(x => x.Type == CustomClaims.MarketerIdentifier);
        if (!marketerInfo)
            opt.Problem(System.Net.HttpStatusCode.Unauthorized, "401");

        var marketerIdentifier = helper.GetId(CustomClaims.MarketerIdentifier);
        var storageModuleService = opt.GetService<IMarketerInventoryService>();
        var all = await storageModuleService.GetDetails(marketerIdentifier);

        var grpByItem = all.GroupBy(w => new { w.ItemId, w.ItemName });
        foreach (var item in grpByItem)
        {
            var name = item.Key.ItemName;
            var st = item.GroupBy(g => new { g.MarketerId, g.MarketerName });

            foreach (var byStore in st)
            {
                var storeName = byStore.Key.MarketerName;

                foreach (var byDate in byStore.GroupBy(g => new { g.ExpireDate }))
                {

                    //var expireDate = (byDate.Key.ExpireDate.HasValue) ? byDate.Key.ExpireDate.Value.ToDottedDate() : "بدون";
                    //var cost = byDate.Key.AvrageCost;


                    var currentInqomeQauntity = byStore.Where(W => W.Ingress).Sum(W => W.TotalQuantity);
                    var currentOutcomeQauntity = byStore.Where(W => !W.Ingress).Sum(W => W.TotalQuantity);

                    var currentQauntity = currentInqomeQauntity - currentOutcomeQauntity;


                    result.Add(new DtoMarketerInventory()
                    {
                        ItemName = name,
                        ExpireDate = byDate.Key.ExpireDate,
                        Quantity = currentQauntity
                    });
                }
            }
        }

        return opt.Success(result);
    });

    [HttpGet("DeptionReport")]
    public ValueTask<HttpResponseWrapperList<DtoStatmentInfo>> GetDeptionReport()
    => TryMeny<DtoStatmentInfo>(async opt =>
    {
        ; if (PrepareNow() is null)
            return opt.Problem("401");

        var mrktr = await GetConnectedMarketer();
        if (mrktr is null)
            return opt.Problem("401");

        var entryService = (EntryService)opt.GetService<IEntryService>();
        var list = await entryService.GetFastStatementListInfo(tagType: TagTypeEnum.MarketerOnly);
        list = list.Where(w => w.AccountId != mrktr.AccountId.Value).ToList();
        return opt.Success(list);
    });

    [HttpGet("ExcnceReport")]
    public ValueTask<HttpResponseWrapperList<DtoStatmentInfo>> GetExcnceReport()
     => TryMeny<DtoStatmentInfo>(async opt =>
     {
         //; if (PrepareNow() is null)
         //    return opt.Problem("401");

         //var mrktr = await GetConnectedMarketer();
         //if (mrktr is null)
         //    return opt.Problem("401");

         //var entryService = (EntryService)opt.GetService<IEntryService>();
         //var list = await entryService.GetFastStatementDeptionListInfo(accountId: mrktr.AccountId.Value, tagType: TagTypeEnum.MarketerOnly);
         ////var list = await entryService.GetNonPayedDebtiobsAccounts2(accountId: ExpenseAccountId, tagType: TagTypeEnum.MarketerOnly);
         ////list = list.Where(w => w.AccountId != mrktr.AccountId.Value).ToList();
         //return opt.Success(list);
         DtoStatmentInfo[] ar = [
             new DtoStatmentInfo(){
                 Debit = 500,
                 Credit = 0,
                 Name = "وقود",
             },
             new DtoStatmentInfo(){
                 Debit = 530,
                 Credit = 0,
                 Name = "كروت اتصالات",
             }];
         return opt.Success(ar);
     });
}
