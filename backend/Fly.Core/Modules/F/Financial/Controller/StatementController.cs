using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.Financial.Features;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Core.Persistence.Ef.Models.Personnel;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Framework.Server.Common.Localization;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.F.Financial.Controller;

public class StatementController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerBase,
        IStetementFeatures
{

    private readonly IEntryService _service;

    public StatementController(
        ILocalizationService localizationService, ILogger<FinancialController> logger
        , IMediator mediator
        , AutoMapper.IMapper mapper
        , MapsterMapper.IMapper mapster
        , IEntryService service) : base(localizationService, logger, mediator, mapper, mapster)
    {
        _service = service;
    }

    [HttpGet("Account")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts(int itemId)
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            //itemId = (itemId == -1) ? new Random().Next(0, 4) : itemId;
            Logger.LogInformation("GetAccounts | Statement => " + itemId);
            switch (itemId)
            {
                case 0:
                    return opt.Success(await _service.MakeChildService<DtoAccount, TAccount>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
                case 1:
                    return opt.Success(await _service.MakeChildService<DtoSupplier, TSupplier>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 2:
                    return opt.Success(await _service.MakeChildService<DtoEmployee, TEmployee>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 3:
                    return opt.Success(await _service.MakeChildService<DtoClient, TClient>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 4:
                    return opt.Success(await _service.MakeChildService<DtoBankBranch, TBankBranch>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 5:
                    return opt.Success(await _service.MakeChildService<DtoSafe, TSafe>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                default:
                    return opt.Problem("Unknown select"); // TODO: localization
            }
        });

    [HttpPost]
    public ValueTask<HttpResponseWrapperList<DtoStatment>> GetStetements(StatementQueryRequest request)
    =>
        TryMeny<DtoStatment>(async opt =>
        {
            var all = await _service.GetDetailsStatements(request.StartDate, request.EndDate, request.AccountId, request.CollectTheQuantitiesBeforeTime, request.Page, request.Liimit);
            all.CurrentPage = 1;
            all.TotalCount = 0;
            return opt.Success(all);
        });

    [HttpGet]
    public ValueTask<HttpResponseWrapperList<DtoStatment>> GetSAlltetements([FromQuery] StatementQueryRequest request)
    =>
        TryMeny<DtoStatment>(async opt =>
        {
            request.EndDate = (request.EndDate == DateTime.MinValue) ? DateTime.Now.AddYears(1) : request.EndDate;
            var all = await _service.GetDetailsStatements(request.StartDate, request.EndDate, request.AccountId, request.CollectTheQuantitiesBeforeTime, request.Page, request.Liimit);
            all.CurrentPage = 1;
            all.TotalCount = 0;
            return opt.Success(all);
        });
}
