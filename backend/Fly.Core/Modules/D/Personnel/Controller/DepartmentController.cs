using Fly.Core.Persistence.Ef.Models.Personnel;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Features;
using Fly.Shared.Modules.Personnel.Requests;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.D.Personnel.Controller;

public class DepartmentController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoDepartment, TDepartment, DepartmentQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>,
        IDepartmentFeatures
{
    public DepartmentController(ILocalizationService localizationService, ILogger<DepartmentController> logger, IMediator mediator, IServiceBase<DtoDepartment, TDepartment> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
    }

    public override Expression<Func<DtoDepartment, bool>> GetWhere(Expression<Func<DtoDepartment, bool>> expression, DepartmentQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }
}
