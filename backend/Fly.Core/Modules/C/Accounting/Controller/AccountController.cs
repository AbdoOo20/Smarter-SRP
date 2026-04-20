using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Features;
using Fly.Shared.Modules.Accounting.Requests;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.C.Accounting.Controller;

//public class MobileController
//    : 
//{
//    [HttpGet("SaleInvoice")]
//    public ValueTask<HttpResponseWrapperList<DtoSalesInvoice>> GetSaleInvoice([FromQuery] SalesInvoiceQueryRequest request)
//    =>
//        TryMany<DtoSalesInvoice>(async (opt) =>
//        {
//        });
//}


public class AccountController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoAccount, TAccount, AccountQueryRequest, DeleteRequestBase, CreateAccountRequest>,
        IAccountingFeatures
{

    private readonly AccountService _accountService;

    public AccountController(ILocalizationService localizationService, ILogger<AccountController> logger, IMediator mediator, AccountService service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _accountService = service;
    }
    /**/
    /**/
    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts([FromQuery] AccountQueryRequest request)
        =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            var all = await _accountService.GetAllAsAsync(acc => new DefaultLookup() { Display = acc.Name, Value = acc.Id });
            return opt.Success(all);
        });
    /**/
    /**/
    [HttpGet("currencies")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCurrencies([FromQuery] CurrencyQueryRequest request)
        =>
        GetSelectableList<DtoCurrency, TCurrency>(x => x.Name);
    /**/
    /**/
    [HttpGet("tree/{parentId}")]
    [AllowAnonymous]
    public ValueTask<HttpResponseWrapperList<DtoAccount>> GetAccountTreeAsync(Guid parentId)
        => TryMeny<DtoAccount>(async opt =>
        {
            var entryService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
            var all = await _accountService.GetAllAsync(e => e.ParentId == parentId);
            var acconuts = all.Items.Select(w => w.Id);
            //  todo: x
            var accountsMovement = await entryService.GetAllAsAsync(s => new { s.Debit, s.Credit, s.AccountId },w => acconuts.Contains(w.AccountId));
            if (accountsMovement.Items.Any())
            {
                foreach (var item in all.Items)
                {
                    item.TotalDebit = accountsMovement.Items.Where(w => w.AccountId == item.Id).Sum(s => s.Debit);
                    item.TotalCredit = accountsMovement.Items.Where(w => w.AccountId == item.Id).Sum(s => s.Credit);
                }
            }
            return opt.Success(all);
        });
    /**/
    /**/
    [HttpGet("accounts/balance")]
    public ValueTask<HttpResponseWrapper<GetAccountBalanceResponse>> GetAccountBalanc([FromBody] GetAccountBalance request)
        =>
        TryOne<GetAccountBalanceResponse>(async (opt) =>
        {
            var response = await Mediator.Send(request);
            return opt.Success(response);
        });


    [NonAction]
    public override Expression<Func<DtoAccount, bool>> GetWhere(Expression<Func<DtoAccount, bool>> expression, AccountQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            query.SearchText = query.SearchText.Trim();
            expression = expression.And(x =>
                x.Name.Contains(query.SearchText)
            );
        }
        return expression;
    }
}