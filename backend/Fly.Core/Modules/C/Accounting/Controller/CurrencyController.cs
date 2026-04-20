using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Features;
using Fly.Shared.Modules.Accounting.Requests;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.C.Accounting.Controller;

public class CurrencyController
        //: CustomControllerBase
        : CustomControllerFeatrueBased<DtoCurrency, TCurrency, CurrencyQueryRequest, DeleteRequestBase, CreateUpdateCurrencyRequest>
        // : CustomControllerFeatrueBased<DtoAccount>
        , ICurrencyFeatures
//DeleteRequestBase, RCreateAccountRequest>
{
    private readonly IServiceBase<DtoCurrency, TCurrency> _service;


    public CurrencyController(ILocalizationService localizationService, ILogger<CurrencyController> logger, IMediator mediator, IServiceBase<DtoCurrency, TCurrency> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;
    }

    public override Expression<Func<DtoCurrency, bool>> GetWhere(Expression<Func<DtoCurrency, bool>> expression, CurrencyQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {
            query.SearchText = query.SearchText.Trim();
            expression = expression.And(x =>
                x.Name.Contains(query.SearchText)
            );
        }

        if (query.IsPrimary != null)
        {
            expression = expression.And(x =>
                x.IsPrimary == query.IsPrimary
            );
        }

        return base.GetWhere(expression, query);
    }

    /*
    [HttpDelete]
    public ValueTask<HttpResponseWrapper<DtoCurrency?>> DeleteRemoveAsync([FromBody] DeleteRequestBase guid)
     =>
       TryOne<DtoCurrency?>(async (opt) =>
       {
           var dbObj = await _service.GetById(guid.Id);
           var dto = await _service.Remove(dbObj);
           return opt.Success(dto);
       });
     
    [HttpGet]
    public ValueTask<HttpResponseWrapperList<DtoCurrency?>> GetAllByQueryAsync([FromQuery] CurrencyQueryRequest getAllQuery)
    =>
        TryMeny<DtoCurrency?>(async (opt) =>
        {
            var w = _service.Build();
            if (getAllQuery.SearchText != null)
            {
                w = w.And(w => w.Name.Contains(getAllQuery.SearchText));
            }

            var dbObj = await _service.GetAllAsync(w);
            return opt.Success(dbObj);
        });


    [HttpGet("{id}")]
    public ValueTask<HttpResponseWrapper<DtoCurrency>> GetOneAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public ValueTask<HttpResponseWrapper<DtoCurrency?>> PostSaveAsync(CreateUpdateCurrencyRequest command)
    =>
        TryOne<DtoCurrency?>(async (opt) =>
        {
            var validation = new CreateUpdateCurrencyRequestValidation(_service);
            var validationStatus = await validation.ValidateAsync(command);
            if (!validationStatus.IsValid)
            {
                var errors = string.Join(",", validationStatus.Errors.Select(s=> s.ErrorMessage));
                throw new Exception(errors);
            }
            var dto = command.Adapt<DtoCurrency>();
            if (dto.IsNew)
            {
                var newDto = await _service.Add(dto);
                if (newDto is null)
                {
                    var type = typeof(DtoAssets).Name;
                    var id = Request.HttpContext.TraceIdentifier;
                    Logger.LogError("error with {id} | Error in Add new entity for type {type}\n\robject : [{dto}]", id, type, dto.ToString());
                    opt.Problem("error");
                }
            }
            else
            {
                var updatedDto = await _service.Update(dto);
                if (updatedDto is null)
                {
                    var type = typeof(DtoAssets).Name;
                    var id = Request.HttpContext.TraceIdentifier;
                    Logger.LogError("error with {id} | Error in update entity for type {type}\n\robject : [{dto}]", id, type, dto.ToString());
                    opt.Problem("error");
                }
            }
            return opt.Success(dto);
        });

    */
}