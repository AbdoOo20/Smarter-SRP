using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Models.DbModels;
using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Service;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Features;
using Fly.Shared.Modules.Maintenance.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Controller;


public class MaintenanceSectorController
        : CustomControllerFeatrueBased<DtoMaintenanceSector, TMaintenanceSector,
                MaintenanceSectorQueryRequest, MaintenanceSectorDeleteRequest,
                MaintenanceSectorCreateUpdateRequest>,
        IMaintenanceSectorsFeatures
{
    public MaintenanceSectorController(ILocalizationService localizationService, ILogger<MaintenanceSectorController> logger, IMediator mediator, IServiceBase<DtoMaintenanceSector, TMaintenanceSector> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += MaintenanceSectorControllerOnCreateUpdate;
    }

    [NonAction]
    private async ValueTask<DtoMaintenanceSector> MaintenanceSectorControllerOnCreateUpdate(MaintenanceSectorCreateUpdateRequest q)
    {
        var tokenService = _service.MakeChildService<ITokenService>();
        var userService = _service.MakeChildService<DtoUser, TUser>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var sector = q.Adapt<DtoMaintenanceSector>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var maintenanceAcc = await configService.GetOneAsAsync(x => x.MaintenanceSectorsAccount);
            if (maintenanceAcc is null)
                throw new Exception("maintenance sector account not ready"); // TODO: Localization

            var maintenanceBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(maintenanceAcc));
            if (maintenanceBaseAccount is null)
                throw new Exception("maintenance sector account not ready"); // TODO: Localization

            var nextNumber = await accountService.GetMaxAsync(c => c.Number,
                w => w.Number.ToString().StartsWith(maintenanceBaseAccount.Number.ToString()));

            if (nextNumber == maintenanceBaseAccount.Number)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(maintenanceBaseAccount.Number.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{maintenanceBaseAccount.Number}{a}");
            }

            var account = await accountService.Add(new DtoAccount
            {
                Name = sector.Name,
                Natural = maintenanceBaseAccount.Natural,
                Number = nextNumber,
                AccountCat = maintenanceBaseAccount.AccountCat,
                CurrencyId = maintenanceBaseAccount.CurrencyId,
                ParentId = maintenanceBaseAccount.Id,
                OpeningCreditor = q.AccountOpeningCreditor,
                OpeningDebtor = q.AccountOpeningDebtor,
            });
            
            sector.AccountId = account.Id;

            await _service.Add(sector);
        }
        else
        {
            var db = await _service.GetById(sector.Id);
            q.Adapt(db);
            sector = await _service.Update(db);
            await accountService.UpdateOpeningBalance(sector.AccountId, q.AccountOpeningDebtor, q.AccountOpeningCreditor);
        }

        _service.CommitTransaction();
        return sector;
    }

    [NonAction]
    public override Expression<Func<DtoMaintenanceSector, bool>> GetWhere(Expression<Func<DtoMaintenanceSector, bool>> expression, MaintenanceSectorQueryRequest query)
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

    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts([FromQuery] AccountQueryRequest request)
    =>
        GetSelectableList<DtoAccount, TAccount>(x => x.Name);

    public ValueTask<HttpResponseWrapperList<DtoMaintenanceSector>> GetAllByQueryAsync(MaintenanceSectorCreateRequest getAllQuery)
    {
        throw new NotImplementedException();
    }
}
