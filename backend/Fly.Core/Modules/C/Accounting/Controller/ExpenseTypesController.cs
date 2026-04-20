using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Features;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.C.Accounting.Controller;

public class ExpenseTypesController
    : CustomControllerFeatrueBased<DtoExpenseTypes, TExpenseTypes, ExpenseTypesQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
        // : CustomControllerFeatrueBased<DtoAccount>
        , IExpenseTypesFeatures
//DeleteRequestBase, RCreateAccountRequest>
{

    public ExpenseTypesController(
        ILocalizationService localizationService, ILogger<ExpenseTypesController> logger,
        IMediator mediator,
        IServiceBase<DtoExpenseTypes, TExpenseTypes> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnDelete += ExpenseTypesController_OnDelete;
        OnCreateUpdate += ExpenseTypesController_OnCreateUpdate;
    }

    private async ValueTask<DtoExpenseTypes> ExpenseTypesController_OnDelete(DeleteRequestBase q)
    {
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        _service.BeginTransaction();

        var expense = await _service.GetById(q.Id);
        if (expense.AccountId is not null)
            await accountService.Remove(x => x.Id == expense.AccountId.Value);
        await _service.Remove(expense);

        _service.CommitTransaction();
        return expense;
    }

    private async ValueTask<DtoExpenseTypes> ExpenseTypesController_OnCreateUpdate(CreateUpdateNamedRequestBase q)
    {
        //
        //
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        //
        DtoExpenseTypes? expense = null;
        _service.BeginTransaction();
        if (q.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.ExpenseAccount);
            if (suplAccountId is null)
                throw new Exception("Expense account not ready"); // TODO: Localization
                                                                  //
            var supplyerBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (supplyerBaseAccount is null)
                throw new Exception("Expense account not ready"); // TODO: Localization
                                                                  //
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
            //
            //
            var account = await accountService.Add(new DtoAccount()
            {
                Name = q.Name,
                Natural = supplyerBaseAccount.Natural,
                Number = nextNumber,
                AccountCat = supplyerBaseAccount.AccountCat,
                CurrencyId = supplyerBaseAccount.CurrencyId,
                ParentId = supplyerBaseAccount.Id,
            });
            expense = q.Adapt<DtoExpenseTypes>();
            expense.AccountId = account.Id;
            await _service.Add(expense);
        }
        else
        {
            expense = await _service.GetById(q.Id);
            if (expense is null)
            {
                return null;
            }
            q.Adapt(expense);
            await _service.Update(expense);
            var acc = await accountService.GetById(expense.AccountId.Value);
            acc.Name = q.Name;
            await accountService.Update(acc);
        }
        //
        _service.CommitTransaction();
        var dbNew = await _service.GetById(expense.Id);
        return dbNew;

    }

    public override Expression<Func<DtoExpenseTypes, bool>> GetWhere(Expression<Func<DtoExpenseTypes, bool>> expression, ExpenseTypesQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {
            query.SearchText = query.SearchText.Trim();
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        }
        return expression;
    }
}