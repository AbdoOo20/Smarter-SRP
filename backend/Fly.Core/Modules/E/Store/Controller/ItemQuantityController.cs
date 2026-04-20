using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.Financial.Controller;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.E.Store.Controller;

public class ItemQuantityController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerBase,
        IItemQnitites
{

    private readonly IEntryService _service;
    private readonly IStorageModuleService _storageModuleService;

    public ItemQuantityController(
        ILocalizationService localizationService,
        ILogger<FinancialController> logger
        , IMediator mediator
        , AutoMapper.IMapper mapper
        , MapsterMapper.IMapper mapster
        , IEntryService service,
IStorageModuleService storageModuleService) : base(localizationService,logger, mediator, mapper, mapster)
    {
        _service = service;
        _storageModuleService = storageModuleService;
    }

    [NonAction]
    public ValueTask<HttpResponseWrapperList<DtoItemQuntityStatement>> GetAllByQueryAsync(ItemQuntityQueryRequest getAllQuery)
        => throw new NotImplementedException();

    [HttpPost]
    public ValueTask<HttpResponseWrapper<string>> GetResultByQueryAsync(ItemQuntityQueryRequest getAllQuery)
    =>
        TryOne<string>(async (opt) =>
        {
            var res = await _storageModuleService.GetItemQuntity(getAllQuery.ItemId, getAllQuery.UnitVemf, getAllQuery.StoreId);
            return opt.Success(res.Quantity.ToString());
        });


    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetStores([FromQuery]ItemQueryRequest request)
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            return opt.Success(await _service.MakeChildService<DtoStore, TStore>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
        });
    

    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetItems([FromQuery] ItemQueryRequest request)
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            return opt.Success(await _service.MakeChildService<DtoItem, TItem>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
        });


    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
         TryMeny<DefaultLookup>(async (opt) =>
         {
             return opt.Success(await _service.MakeChildService<DtoItemUnit, TItemUnit>()
                 .GetDefaultLookupAsync(x => x.UnitId, x => x.Unit.Name, w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty)));
         });


    [NonAction]
    public ValueTask<HttpResponseWrapper<DtoItemQuntityStatement?>> GetOneAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}