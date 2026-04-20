using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Features;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Extentions;
using Mapster;

namespace Fly.Core.Modules.E.Store.Controller;



public class CategoryController
    : CustomControllerFeatrueBased<DtoCategory, TCategory, CategoryQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
    , ICategoryFeatures

{
    private Identifier _primaryCat = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4CCB016");
    
    public CategoryController(ILocalizationService localizationService, ILogger<CategoryController> logger,
        IMediator mediator,
        IServiceBase<DtoCategory, TCategory> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += OnOnCreateUpdate;
        OnDelete += OnOnDelete;
    }

    private async ValueTask<DtoCategory> OnOnDelete(DeleteRequestBase q)
    {
        var cat = q.Adapt<DtoCategory>();
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (cat.Id == _primaryCat)
        {
            throw new InvalidInputException("لا يمكن حذف الفئة الأساسية."); // locale
        }
        await _service.Remove(cat);

        _service.CommitTransaction();

        return cat;
    }

    private async ValueTask<DtoCategory> OnOnCreateUpdate(CreateUpdateNamedRequestBase q)
    {
        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);
        var cat = q.Adapt<DtoCategory>();

        if (cat.CateguryId.IsNullOrEmpty())
        {
            cat.CateguryId = _primaryCat;
        }

        await _service.AddOrUpdateAsync(cat);

        _service.CommitTransaction();
        
        return cat;
    }

    [HttpGet("categuries")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCateguries([FromQuery] CategoryQueryRequest request)
    => GetSelectableList<DtoCategory, TCategory>(x => x.Name);

    public override Expression<Func<DtoCategory, bool>> GetWhere(Expression<Func<DtoCategory, bool>> expression, CategoryQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }
}
