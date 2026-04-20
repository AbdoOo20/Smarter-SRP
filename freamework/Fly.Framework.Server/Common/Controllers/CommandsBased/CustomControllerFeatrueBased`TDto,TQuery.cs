using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Fly.Framework.Shared.Common;
using Fly.Framework.Server.Common.Localization;

namespace Fly.Framework.Server.Common.Controllers.CommandsBased;

[Authorize]
public class CustomControllerFeatrueBased<TDto, TEn, TQuery>
    : CustomControllerBase,
    IFeatureBase<TDto, TQuery>
    where TEn : TEntity
    where TDto : DtoBase
    where TQuery : QueryRequestBase
{
    private readonly IServiceBase<TDto, TEn> _service;
    public IServiceBase<TDto, TEn> Service => _service;
    
    public CustomControllerFeatrueBased(
        ILocalizationService localizationService,
        ILogger<CustomControllerFeatrueBased<TDto, TEn, TQuery>> logger,
        IMediator mediator,
        IServiceBase<TDto, TEn> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, mapper, mapster)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public virtual ValueTask<HttpResponseWrapper<TDto?>> GetOneAsync(Guid id)
        =>
        TryOne<TDto?>(async (opt) =>
        {
            var target = await _service.GetById(id);
            return opt.Success(target);
        });

    [HttpGet]
    public virtual ValueTask<HttpResponseWrapperList<TDto>> GetAllByQueryAsync([FromQuery] TQuery getAllQuery)
        =>
        TryMeny<TDto>(async (opt) =>
        {
            var w = GetWhere(_service.Build(), getAllQuery);
            var list = await _service.GetAllAsync(w, getAllQuery.Page, getAllQuery.Liimit);
            await BeforReturn(list);
            return opt.Success(list);
        });

    [NonAction]
    public virtual Task BeforReturn(PageList<TDto> list)
    {
        return Task.CompletedTask;
    }


    #region Helpers


    [NonAction]
    public virtual Expression<Func<TDto, bool>> GetWhere(Expression<Func<TDto, bool>> expression, TQuery query)
    {
        return expression;
    }


    [NonAction]
    public HttpResponseWrapper<TDto> PrintError(ResultOrFailer<TDto> opt, TDto dto)
    {
        var typeName = typeof(TDto).Name;
        var id = Request.HttpContext.TraceIdentifier;
        Logger.LogError("\n\rerror with {id} | Error in Add new entity for type {type}\n\robject : [{dto}\n\r]\n\r", id, typeName, dto.ToString());
        return opt.Problem("error - " + id);
    }


    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null, Expression<Func<TDtoo, object>>? descendingOrderBy = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetSelecListAsync(textSelector, where, descendingOrderBy)));


    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, Identifier> idSelector, Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null, Expression<Func<TDtoo, object>>? descendingOrderBy = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
        =>
            TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetDefaultLookupAsync(idSelector, textSelector, where, descendingOrderBy)));


    #endregion

}


//Obsolete]
//[Authorize]
//public class CustomControllerFeatrueBased<TDto, TEn>
//    : CustomControllerBase, IFeatureBase<TDto>
//    where TEn : TEntity
//    where TDto : DtoBase
//{
//    private readonly IServiceBase<TDto, TEn> _service;
//    public IServiceBase<TDto, TEn> Service => _service;


//    public CustomControllerFeatrueBased(
//        ILogger<CustomControllerFeatrueBased<TDto, TEn>> logger,
//        IMediator mediator,
//        IServiceBase<TDto, TEn> service,
//        AutoMapper.IMapper mapper,
//        MapsterMapper.IMapper mapster)
//        : base(logger, mediator, mapper, mapster)
//    {
//        _service = service;
//    }

//    [HttpGet("{Id}")]
//    public ValueTask<HttpResponseWrapper<TDto?>> GetOneAsync(Guid Id)
//        =>
//        TryOne<TDto?>(async (opt) =>
//        {
//            var target = await _service.GetById(Id);
//            return opt.Success(target);
//        });



//    [NonAction]
//    public virtual Expression<Func<TDto, bool>> GetWhere(Expression<Func<TDto, bool>> expression, string search)
//    {
//        return expression;
//    }


//    [NonAction]
//    public HttpResponseWrapper<TDto> PrintError(ResultOrFailer<TDto> opt, TDto dto)
//    {
//        var typeName = typeof(TDto).Name;
//        var id = Request.HttpContext.TraceIdentifier;
//        Logger.LogError("\n\rerror with {id} | Error in Add new entity for type {type}\n\robject : [{dto}\n\r]\n\r", id, typeName, dto.ToString());
//        return opt.Problem("error - " + id);
//    }


//    [NonAction]
//    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null)
//        where TDtoo : DtoBase
//        where TEnto : TEntity, new()
//    =>
//        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetSelecListAsync(textSelector, where)));


//    public ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync(TDto dto)
//    {
//        throw new NotImplementedException();
//    }

//    public ValueTask<HttpResponseWrapper<TDto>> DeleteRemoveAsync(Guid guid)
//    {
//        throw new NotImplementedException();
//    }

//    [HttpGet]
//    public ValueTask<HttpResponseWrapperList<TDto>> GetAllAsync(int? page = null, int? limit = null, string? text = null, Dictionary<string, object[]>? filter = null)
//    =>
//        TryMeny<TDto>(async (opt) =>
//        {
//            var w = GetWhere(_service.Build(), text);
//            var list = await _service.GetAllAsync(w, page, limit);
//            return opt.Success(list);
//        });
//}

