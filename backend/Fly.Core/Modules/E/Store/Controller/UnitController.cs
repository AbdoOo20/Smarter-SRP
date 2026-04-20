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

namespace Fly.Core.Modules.E.Store.Controller;

public class UnitController
    : CustomControllerFeatrueBased<DtoUnit, TUnit, UnitQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
        , IUnitFeatures

{
    public UnitController( ILocalizationService localizationService ,ILogger<UnitController> logger, IMediator mediator, IServiceBase<DtoUnit, TUnit> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
    }

    public override Expression<Func<DtoUnit, bool>> GetWhere(Expression<Func<DtoUnit, bool>> expression, UnitQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }

}