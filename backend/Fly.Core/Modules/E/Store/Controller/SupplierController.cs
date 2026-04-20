using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Core.Modules.E.Store.Controller;

public class SupplierController
    : CustomControllerFeatrueBased<DtoSupplier, TSupplier, SupplierQueryRequest, DeleteRequestBase, CreateUpdateSupplierRequest>
        , ISupplierFeatures, IDisposable

{
    public SupplierController(ILocalizationService localizationService, ILogger<SupplierController> logger, IMediator mediator, IServiceBase<DtoSupplier, TSupplier> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += SupplierControllerOnCreateUpdate;
    }

    public override Expression<Func<DtoSupplier, bool>> GetWhere(Expression<Func<DtoSupplier, bool>> expression, SupplierQueryRequest query)
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
    private async ValueTask<DtoSupplier> SupplierControllerOnCreateUpdate(CreateUpdateSupplierRequest q)
    {
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var supplyer = q.Adapt<DtoSupplier>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (supplyer.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.SupplyersAccount);
            if (suplAccountId is null)
                throw new Exception("Supplyer account not ready"); // TODO: Localization

            var supplyerBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (supplyerBaseAccount is null)
                throw new Exception("Supplyer account not ready"); // TODO: Localization

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
                StatementAccountType = StatementAccountType.Suppliers
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
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(AccountQueryRequest request)
        =>
            GetSelectableList<DtoAccount, TAccount>(x => x.Name);

    public virtual void Dispose()
    {
        OnCreateUpdate -= SupplierControllerOnCreateUpdate;
    }
}