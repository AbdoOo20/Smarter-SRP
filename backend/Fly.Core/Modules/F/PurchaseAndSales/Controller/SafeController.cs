using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
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

public class SafeController
    // : CustomControllerBase
    : CustomControllerFeatrueBased<DtoSafe, TSafe, SafeQueryRequest, SafeDeleteRequest, SafeCreateUpdateRequest>
        // : CustomControllerFeatrueBased<DtoAccount>
        , ISafeFeatures
{
    private readonly IServiceBase<DtoSafe, TSafe> _service;

    public SafeController(ILocalizationService localizationService, ILogger<SafeController> logger, IMediator mediator, IServiceBase<DtoSafe, TSafe> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;

        OnCreateUpdate += SafeOnCreateUpdate;

    }

    

    public override Expression<Func<DtoSafe, bool>> GetWhere(Expression<Func<DtoSafe, bool>> expression, SafeQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }

    [NonAction]
    private async ValueTask<DtoSafe> SafeOnCreateUpdate(SafeCreateUpdateRequest q)
    {
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var dto = q.Adapt<DtoSafe>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.SafeAccount);
            if (suplAccountId is null)
                throw new Exception("Safe  account not ready"); // TODO: Localization

            var baseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (baseAccount is null)
                throw new Exception("Safe account not ready"); // TODO: Localization

            var nextNumber = await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString().StartsWith(baseAccount.Number.ToString()));

            if (nextNumber == baseAccount.Number)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(baseAccount.Number.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{baseAccount.Number}{a}");
            }

            var account = await accountService.Add(new DtoAccount()
            {
                Name = dto.Name,
                Natural = baseAccount.Natural,
                Number = nextNumber,
                AccountCat = baseAccount.AccountCat,
                CurrencyId = baseAccount.CurrencyId,
                ParentId = baseAccount.Id,
                OpeningCreditor = q.AccountOpeningCreditor,
                OpeningDebtor = q.AccountOpeningDebtor,
                StatementAccountType = StatementAccountType.Safes
            });
            dto.AccountId = account.Id;
            await _service.Add(dto);
        }
        else
        {
            var db = await _service.GetById(dto.Id);
            q.Adapt(db);
            dto = await _service.Update(db);
            await accountService.UpdateOpeningBalance(dto.AccountId, q.AccountOpeningDebtor, q.AccountOpeningCreditor);
        }

        _service.CommitTransaction();
        return dto;
    }

}