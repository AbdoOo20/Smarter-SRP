using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Microsoft.AspNetCore.Mvc;

namespace Fly.Core.Modules.E.Store.Controller;

public class StoreController
    : CustomControllerFeatrueBased<DtoStore, TStore, StoreQueryRequest, DeleteRequestBase, CreateUpdateStoreRequest>
        , IStoreFeatures

{
    public StoreController(ILocalizationService localizationService, ILogger<CustomControllerFeatrueBased<DtoStore, TStore, StoreQueryRequest, DeleteRequestBase, CreateUpdateStoreRequest>> logger, IMediator mediator, IServiceBase<DtoStore, TStore> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
    }

    public override Expression<Func<DtoStore, bool>> GetWhere(Expression<Func<DtoStore, bool>> expression, StoreQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }
    
    [HttpPost("MoveItems")]
    public async ValueTask<HttpResponseWrapper<MoveItemsRequest>> MoveItemsAsync([FromBody] MoveItemsRequest request)
    {
        return await TryOne<MoveItemsRequest>(async opt =>
        {
            var service = _service.MakeChildService<IStorageModuleService>();
            return opt.Success(await service.TransferItemsBetweenStoragesAsync(request));
        });
    }

    [HttpPost("DamageItems")]
    public async ValueTask<HttpResponseWrapper<DamageItemsRequest>> DamageItemsAsync([FromBody] DamageItemsRequest request)
    {
        return await TryOne<DamageItemsRequest>(async opt =>
        {
            var service = _service.MakeChildService<IStorageModuleService>();
            return opt.Success(await service.MarkItemsAsDamagedAsync(request));
        });
    }
    
    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name}");
    
    [HttpGet("Categories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCategories([FromQuery] CategoryQueryRequest request)
        => GetSelectableList<DtoCategory, TCategory>(x => x.Name);
    
    [HttpGet("Stores")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupStores(
        [FromQuery] StoreQueryRequest request)
        => GetSelectableList<DtoStore, TStore>(x => x.Name);
    
    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoUnit, TUnit>(x => x.Name);
}