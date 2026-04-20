using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.Factoring.ManufacturingFacilities.Models.DbModels;
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
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Features;
using Fly.Shared.Modules.Factoring.Requests;
using Fly.Shared.Modules.Global.Dtos;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Factoring.ManufacturingFacilities.Controller;

public class ManufacturingFacilityController
        : CustomControllerFeatrueBased<DtoManufacturingFacility, TManufacturingFacility,
                ManufacturingFacilityQueryRequest, ManufacturingFacilityDeleteRequest,
                ManufacturingFacilityCreateUpdateRequest>,
        IManufacturingFacilityFeatures
{
    public ManufacturingFacilityController(ILocalizationService localizationService,
        ILogger<ManufacturingFacilityController> logger,
        IMediator mediator, IServiceBase<DtoManufacturingFacility, TManufacturingFacility> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += ManufacturingFacilityControllerOnCreateUpdate;
    }

    [NonAction]
    private async ValueTask<DtoManufacturingFacility> ManufacturingFacilityControllerOnCreateUpdate(ManufacturingFacilityCreateUpdateRequest q)
    {
        var tokenService = _service.MakeChildService<ITokenService>();
        var userService = _service.MakeChildService<DtoUser, TUser>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var sector = q.Adapt<DtoManufacturingFacility>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var maintenanceAcc = await configService.GetOneAsAsync(x => x.ManufacturingFacilitiesAccount);
            if (maintenanceAcc is null)
                throw new Exception("Manufacturing Facility account not ready"); // TODO: Localization

            var manufacturingFacility = await accountService.GetAsync(w => w.Id == Identifier.Parse(maintenanceAcc));
            if (manufacturingFacility is null)
                throw new Exception("Manufacturing Facility account not ready"); // TODO: Localization

            var nextNumber = await accountService.GetMaxAsync(c => c.Number,
                w => w.Number.ToString().StartsWith(manufacturingFacility.Number.ToString()));

            if (nextNumber == manufacturingFacility.Number)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(manufacturingFacility.Number.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{manufacturingFacility.Number}{a}");
            }

            var account = await accountService.Add(new DtoAccount
            {
                Name = sector.Name,
                Natural = manufacturingFacility.Natural,
                Number = nextNumber,
                AccountCat = manufacturingFacility.AccountCat,
                CurrencyId = manufacturingFacility.CurrencyId,
                ParentId = manufacturingFacility.Id,
                OpeningCreditor = q.AccountOpeningCreditor,
                OpeningDebtor = q.AccountOpeningDebtor,
            });
            
            //var passInfo = await tokenService.GeneratePassword(q.Phone);
            /*var user = await userService.Add(new DtoUser
            {
                Accesser = "M",
                IsRedy = false,
                IsActive = true,
                Phone = q.Phone,
                Name = sector.Name,
                Sult = passInfo.Salt,
                Password = passInfo.Password,
                //  TODO: create new action
                //UserRoles = [
                //    new DtoUserRole(){
                //        RoleId = Identifier.Parse("08b4dae1-2eef-4509-b0e0-1ffffffff101")
                //    }
                //]
            });*/
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
    public override Expression<Func<DtoManufacturingFacility, bool>> GetWhere(
        Expression<Func<DtoManufacturingFacility, bool>> expression,
        ManufacturingFacilityQueryRequest query)
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

    public ValueTask<HttpResponseWrapperList<DtoManufacturingFacility>> GetAllByQueryAsync(ManufacturingFacilityCreateRequest getAllQuery)
    {
        return default;
    }
}
