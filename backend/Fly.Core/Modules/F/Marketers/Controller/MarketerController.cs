using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Framework.Shared.Common;
using Mapster;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.Marketers.Features;
using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Service;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using LinqKit;
using System.Linq.Expressions;
using Fly.Framework.Server.Common.Localization;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.F.Marketers.Controller;


public class MarketerController
        :
        CustomControllerFeatrueBased<DtoMarketer, TMarketer, MarketerQueryRequest, MarketerDeleteRequest, MarketerCreateUpdateRequest>,
        IMarketersFeatures
{
    public MarketerController(ILocalizationService localizationService, ILogger<MarketerController> logger, IMediator mediator, IServiceBase<DtoMarketer, TMarketer> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += ClientController_OnCreateUpdate;
    }

    [NonAction]
    public override Expression<Func<DtoMarketer, bool>> GetWhere(Expression<Func<DtoMarketer, bool>> expression, MarketerQueryRequest query)
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
    private async ValueTask<DtoMarketer> ClientController_OnCreateUpdate(MarketerCreateUpdateRequest q)
    {
        var tokenService = _service.MakeChildService<ITokenService>();
        var userService = _service.MakeChildService<DtoUser, TUser>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var marketer = q.Adapt<DtoMarketer>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.ClientsAccount);
            if (suplAccountId is null)
                throw new Exception("marketer account not ready"); // TODO: Localization

            var supplyerBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (supplyerBaseAccount is null)
                throw new Exception("marketer account not ready"); // TODO: Localization

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
                Name = marketer.Name,
                Natural = supplyerBaseAccount.Natural,
                Number = nextNumber,
                AccountCat = supplyerBaseAccount.AccountCat,
                CurrencyId = supplyerBaseAccount.CurrencyId,
                ParentId = supplyerBaseAccount.Id,
                OpeningCreditor = q.AccountOpeningCreditor,
                OpeningDebtor = q.AccountOpeningDebtor,
                StatementAccountType = StatementAccountType.Marketers
            });
            var passInfo = await tokenService.GeneratePassword(q.Phone);
            var user = await userService.Add(new DtoUser()
            {
                Accesser = "M",
                IsRedy = false,
                IsActive = true,
                Phone = q.Phone,
                Name = marketer.Name,
                Sult = passInfo.Salt,
                Password = passInfo.Password,
                //  TODO: create new action
                //UserRoles = [
                //    new DtoUserRole(){
                //        RoleId = Identifier.Parse("08b4dae1-2eef-4509-b0e0-1ffffffff101")
                //    }
                //]
            });
            marketer.AccountId = account.Id;
            if (marketer.MarketerClients.Any())
            {
                marketer.MarketerClients.ForEach(f => f.MarketerId = marketer.Id);
            }
            await _service.Add(marketer);
        }
        else
        {
            //  TODO: Contenue
            var db = await _service.GetById(marketer.Id);
            q.Adapt(db);
            marketer = await _service.Update(db);
            await accountService.UpdateOpeningBalance(marketer.AccountId, q.AccountOpeningDebtor, q.AccountOpeningCreditor);

            var marketerClientService = _service.MakeChildService<DtoMarketerClient, TMarketerClient>();
            await marketerClientService.Remove(w => w.MarketerId == marketer.Id);

            var newMarketerCLients = q.MarketerClients.Select(f => new DtoMarketerClient { ClientId = f.ClientId, MarketerId = db.Id }).ToArray();
            await marketerClientService.AddRange(newMarketerCLients);
        }

        _service.CommitTransaction();
        return marketer;
    }

    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts([FromQuery] AccountQueryRequest request)
    =>
        GetSelectableList<DtoAccount, TAccount>(x => x.Name);

    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DtoClient>> GetLookupClients([FromQuery] ClientQueryRequest request)
    =>
        // GetSelectableList<DtoClient, TClient>(x => x.Name, w =>);
        TryMeny<DtoClient>(async opt =>
        {
            var q = "SELECT";
            var where = _service.Build();
            if (request.MarketerId is not null && !request.MarketerId.Value.IsNullOrEmpty()) where = where.And(W => W.Id != request.MarketerId);
            var allUSedClientsInfo = await _service.GetAllAsAsync(s => s.MarketerClients.Select(ss => ss.ClientId), where);
            var allUSedClients = allUSedClientsInfo.Items.SelectMany(s => s.Select(ss => ss));

            var clientService = opt.MakeChildService<DtoClient, TClient>();
            var avalibleClients = await clientService.GetAllAsync(w => !allUSedClients.Contains(w.Id));
            return opt.Success(avalibleClients);
        });


    //
    //  MOBILE NEEDS

}
