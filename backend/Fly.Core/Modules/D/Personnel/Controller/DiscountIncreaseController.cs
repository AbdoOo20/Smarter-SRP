using Fly.Core.Persistence.Ef.Models.Personnel;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Features;
using Fly.Shared.Modules.Personnel.Requests;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.D.Personnel.Controller;

public class DiscountIncreaseController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased
            <DtoDiscountIncrease, TDiscountIncrease, DiscountIncreaseQueryRequest, DeleteRequestBase, CreateUpdateDiscountIncreaseRequest>,
        IDiscountIncreaseFeatures
{

    public DiscountIncreaseController(ILocalizationService localizationService, ILogger<DiscountIncreaseController> logger,
                                      IMediator mediator,
                                      IServiceBase<DtoDiscountIncrease, TDiscountIncrease> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
    }


    [HttpGet("employees")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupEmployees([FromQuery] EmployeeQueryRequest request)
        =>
        GetSelectableList<DtoEmployee, TEmployee>(x => x.Name);

    public override Expression<Func<DtoDiscountIncrease, bool>> GetWhere(Expression<Func<DtoDiscountIncrease, bool>> expression, DiscountIncreaseQueryRequest query)
    {

        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.EmployeeName == query.SearchText);
        return base.GetWhere(expression, query);
    }
}
