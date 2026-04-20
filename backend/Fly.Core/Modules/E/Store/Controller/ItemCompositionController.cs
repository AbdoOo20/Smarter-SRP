using Fly.Core.Modules.E.Store.Services.InternalServices;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.E.Store.Controller;

public class ItemCompositionController
    : CustomControllerFeatrueBased<DtoItemComposition,
            TItemComposition, ItemCompositionQueryRequest,
            DeleteItemCompositionRequest,
            CreateUpdateItemCompositionRequest>,
        IItemCompositionFeatures, IDisposable

{
    public ItemCompositionController(ILocalizationService localizationService,
        ILogger<ItemCompositionController> logger,
        IMediator mediator, IItemCompositionService service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += ItemCompositionControllerOnCreateUpdate;
    }

    public override Expression<Func<DtoItemComposition, bool>> GetWhere(
        Expression<Func<DtoItemComposition, bool>> expression,
        ItemCompositionQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(
                x =>
                x.Name.Contains(query.SearchText) |
                x.Details.Any(u => u.ItemName.Contains(query.SearchText))
            );
        return base.GetWhere(expression, query);
    }

    [HttpGet("NextNumber")]
    public string GetNextString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "a"; // Start from 'a' if input is empty
        }

        char[] chars = input.ToCharArray();
        for (int i = chars.Length - 1; i >= 0; i--)
        {
            if (chars[i] == 'z')
            {
                chars[i] = 'a'; // Reset 'z' to 'a'
            }
            else
            {
                chars[i]++;
                return new string(chars);
            }
        }

        // If all characters were 'z', add 'a' at the beginning
        return "a" + new string(chars);
    }

    private async ValueTask<DtoItemComposition> ItemCompositionControllerOnCreateUpdate(CreateUpdateItemCompositionRequest q)
    {
        // get needed services
        //var unitService = _service.MakeChildService<DtoUnit, TUnit>();
        var itemCompositionDetailService = _service.MakeChildService<DtoItemCompositionDetail, TItemCompositionDetail>();
        //var storageModuleService = _service.MakeChildService<IStorageModuleService>();

        //var itemunits = q.ItemUnits.Adapt<List<DtoItemUnit>>();
        //q.ItemUnits = null;

        var item = q.Adapt<DtoItemComposition>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (item.IsNew())
        {
            await _service.Add(item);
        }
        else
        {
            var db = await _service.GetById(item.Id);
            await itemCompositionDetailService.Remove(w => w.ItemCompositionId == db.Id);
            var all = item.Details.ToList();
            item.Adapt(db);
            all.ForEach(f => f.ItemCompositionId = db.Id);
            await itemCompositionDetailService.AddRange(all);
            await _service.Update(item);
        }

        _service.CommitTransaction();
        return item;
    }

    [HttpGet("Categories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCategories([FromQuery] CategoryQueryRequest request)
        => GetSelectableList<DtoCategory, TCategory>(x => x.Name);

    [HttpGet("ItemVariances")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupVariances([FromQuery] ItemVarianceQueryRequest request)
        => GetSelectableList<DtoItemVariance, TItemVariance>(x => x.Name);
    
    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
        => GetSelectableList<DtoItem, TItem>(x => $"{x.Name} ({x.Barcode}) • {x.Price}",
            request.ItemId != null
                ? x => x.CategoryId == request.ItemId.Value && x.IsActive
                : y => y.IsActive);
    
    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        => GetSelectableList<DtoUnit, TUnit>(x => x.Name);

    public virtual void Dispose()
    {
        OnCreateUpdate -= ItemCompositionControllerOnCreateUpdate;
    }
}