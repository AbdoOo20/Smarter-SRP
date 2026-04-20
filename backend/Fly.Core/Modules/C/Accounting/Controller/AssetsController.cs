using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.Global.Service;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Features;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.C.Accounting.Controller;

public class AssetsController
        // : CustomControllerBase
        : CustomControllerFeatrueBased<DtoAssets, TAssets, AssetsQueryRequest, DeleteRequestBase, AssetCreateUpdateCommand>
        // : CustomControllerFeatrueBased<DtoAccount>
        , IAssetsFeatures
{
    private readonly IServiceBase<DtoAssets, TAssets> _service;

    public AssetsController(ILocalizationService localizationService, ILogger<AssetsController> logger, IMediator mediator, IServiceBase<DtoAssets, TAssets> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;
        OnCreateUpdate += AssetsControllerOnCreateUpdate;
    }

    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(
       [FromQuery] AccountQueryRequest request)
    =>
       TryMeny<DefaultLookup>(async (opt) =>
       {
           var _configService = opt.ServiceProvider.GetRequiredService<IConfigService>();
           var _accountService = opt.ServiceProvider.GetRequiredService<AccountService>();
           var expenseAccount = await _configService.GetConfigValue(w => w.AssetsAccount);

           if(string.IsNullOrEmpty(expenseAccount))
           {
               return opt.Problem("������ ��� ���� �������"); // TODO: Localization
           }

           var all = await _accountService.GetDefaultLookupAsync(sId => sId.Id, sText => sText.Name, w => w.ParentId == Identifier.Parse(expenseAccount));
           return opt.Success(all);
       });


    private async ValueTask<DtoAssets> AssetsControllerOnCreateUpdate(AssetCreateUpdateCommand q)
    {
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();

        var assets = q.Adapt<DtoAssets>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (assets.IsNew())
        {
            var assetsBaseAccount = await accountService.GetAsync(w => w.Id == q.AccountId);
            if (assetsBaseAccount is null)
                throw new Exception("assets account not ready."); // TODO: Localization

            var nextNumber = await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString().StartsWith(assetsBaseAccount.Number.ToString()));

            if (nextNumber == assetsBaseAccount.Number)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(assetsBaseAccount.Number.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{assetsBaseAccount.Number}{a}");
            }

            var account = await accountService.Add(new DtoAccount()
            {
                Name = assets.Name,
                Natural = assetsBaseAccount.Natural,
                Number = nextNumber,
                AccountCat = assetsBaseAccount.AccountCat,
                CurrencyId = assetsBaseAccount.CurrencyId,
                ParentId = assetsBaseAccount.Id,
            });
            assets.AccountId = account.Id;
            await _service.Add(assets);
        }
        else
        {
            var db = await _service.GetById(assets.Id);
            q.Adapt(db);
            assets = await _service.Update(db);
        }

        _service.CommitTransaction();
        return assets;
    }


    public override Expression<Func<DtoAssets, bool>> GetWhere(Expression<Func<DtoAssets, bool>> expression, AssetsQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {
            query.SearchText = query.SearchText.Trim();
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        }
        return expression;
    }
}