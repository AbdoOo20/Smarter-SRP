using System.Linq.Expressions;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class BankController
    // : CustomControllerBase
    : CustomControllerFeatrueBased<DtoBank, TBank, BankQueryRequest, BankDeleteRequest, BankCreateUpdateRequest>
        // : CustomControllerFeatrueBased<DtoAccount>
        , IBankFeatures
{
    private readonly IServiceBase<DtoBank, TBank> _service;

    public BankController(ILocalizationService localizationService, ILogger<BankController> logger, IMediator mediator, IServiceBase<DtoBank, TBank> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;

        OnCreateUpdate += BackOnCreateUpdate;
    }

    [NonAction]
    public override Expression<Func<DtoBank, bool>> GetWhere(Expression<Func<DtoBank, bool>> expression, BankQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }

    [NonAction]
    private async ValueTask<DtoBank> BackOnCreateUpdate(BankCreateUpdateRequest q)
    {
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();

        var dto = q.Adapt<DtoBank>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.BankAccount);
            if (suplAccountId is null)
                throw new Exception("Safe  account not ready"); // TODO: Localization

            var baseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (baseAccount is null)
                throw new Exception("Safe account not ready"); // TODO: Localization
            var baseAccountNumber = long.Parse(baseAccount.Number.ToString().Replace("20002000", ""));
            Logger.LogInformation("N Account number: {0}", baseAccountNumber);
            var nextNumber = await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString().StartsWith(baseAccount.Number.ToString()));
            
            Logger.LogInformation("N Account number: {0}", nextNumber);
            nextNumber = long.Parse(nextNumber.ToString().Replace("20002000", ""));

            if (nextNumber == baseAccountNumber)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(baseAccountNumber.ToString().Length);
                Logger.LogInformation("L Account number: {0}", nextNumber);
                Logger.LogInformation("Las Account number: {0}", las);

                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{baseAccountNumber}{a}");
            }

            var account = await accountService.Add(new DtoAccount()
            {
                Name = dto.Name,
                Natural = baseAccount.Natural,
                Number = nextNumber,
                AccountCat = baseAccount.AccountCat,
                CurrencyId = baseAccount.CurrencyId,
                ParentId = baseAccount.Id,
                StatementAccountType = StatementAccountType.Banks
            });
            dto.AccountId = account.Id;
            await _service.Add(dto);
        }
        else
        {
            var db = await _service.GetById(dto.Id);
            q.Adapt(db);
            dto = await _service.Update(db);
        }

        _service.CommitTransaction();
        return dto;
    }



}