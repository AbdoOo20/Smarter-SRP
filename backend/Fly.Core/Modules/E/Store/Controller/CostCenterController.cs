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

public class CostCenterController
    : CustomControllerFeatrueBased<DtoCostCenter, TCostCenter, CostCenterQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
        , ICostCenterFeatures

{
    public CostCenterController(ILocalizationService localizationService, ILogger<CostCenterController> logger,
        IMediator mediator,
        IServiceBase<DtoCostCenter, TCostCenter> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {

    }


    public override Expression<Func<DtoCostCenter, bool>> GetWhere(Expression<Func<DtoCostCenter, bool>> expression, CostCenterQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }

}