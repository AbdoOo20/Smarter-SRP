using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mapster;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using System.Linq.Expressions;
using LinqKit;
using Fly.Framework.Server.Common.Localization;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class ClientController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoClient, TClient, ClientQueryRequest, DeleteRequestBase, ClientCreateUpdateRequest>,
        IClientFeatures
{
    public ClientController(ILocalizationService localizationService, ILogger<ClientController> logger, IMediator mediator, IServiceBase<DtoClient, TClient> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += ClientController_OnCreateUpdate;
    }

    [NonAction]
    public override Expression<Func<DtoClient, bool>> GetWhere(Expression<Func<DtoClient, bool>> expression, ClientQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Name.Contains(query.SearchText) |
                x.Phone.Contains(query.SearchText) |
                x.Email.Contains(query.SearchText) |
                x.Identifier.Contains(query.SearchText)
            );
        return base.GetWhere(expression, query);
    }

    [NonAction]
    private async ValueTask<DtoClient> ClientController_OnCreateUpdate(ClientCreateUpdateRequest q)
    {
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var supplyer = q.Adapt<DtoClient>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.ClientsAccount);
            if (suplAccountId is null)
                throw new Exception("Client account not ready"); // TODO: Localization

            var supplyerBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (supplyerBaseAccount is null)
                throw new Exception("Client account not ready"); // TODO: Localization

            var nextNumber = await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString().StartsWith(supplyerBaseAccount.Number.ToString()));

            if (nextNumber == supplyerBaseAccount.Number)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(supplyerBaseAccount.Number.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{supplyerBaseAccount.Number}{a}");
            }

            var account = await accountService.Add(new DtoAccount()
            {
                Name = supplyer.Name,
                Natural = supplyerBaseAccount.Natural,
                Number = nextNumber,
                AccountCat = supplyerBaseAccount.AccountCat,
                CurrencyId = supplyerBaseAccount.CurrencyId,
                ParentId = supplyerBaseAccount.Id,
                OpeningCreditor = q.AccountOpeningCreditor,
                OpeningDebtor = q.AccountOpeningDebtor,
                StatementAccountType = StatementAccountType.Clients
            });
            supplyer.AccountId = account.Id;
            await _service.Add(supplyer);
        }
        else
        {
            var db = await _service.GetById(supplyer.Id);
            q.Adapt(db);
            supplyer = await _service.Update(db);
            await accountService.UpdateOpeningBalance(db.AccountId, q.AccountOpeningDebtor, q.AccountOpeningCreditor);
        }

        _service.CommitTransaction();
        return supplyer;
    }

    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts([FromQuery] AccountQueryRequest request)
        =>
        GetSelectableList<DtoAccount, TAccount>(x => x.Name);

}