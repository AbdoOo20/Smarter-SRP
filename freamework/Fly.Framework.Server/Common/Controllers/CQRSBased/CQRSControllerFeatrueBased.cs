using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Framework.Server.Common.Controllers.Base;

namespace Fly.Framework.Server.Common.Controllers.CQRSBased;

#if DEBUG
[AllowAnonymous]
#else
[Authorize]
#endif
public class CQRSControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand, TCreateCommand/*, TUpdateCommand*/>
    : CustomControllerBase,
    IFeatureBase<TDto, TQuery, TDeleteCommand, TCreateCommand/*, TUpdateCommand*/>
    where TDto : DtoBase
    where TEn : TEntity
    where TQuery : QueryRequestBase, IRequest<HttpResponseWrapperList<TDto>>
    where TDeleteCommand : DeleteRequestBase, IRequest<HttpResponseWrapper<TDto>>, new()
    where TCreateCommand : CreateRequestBase, IRequest<HttpResponseWrapper<TDto?>>
    //where TUpdateCommand : UpdateRequestBase, IRequest<HttpResponseWrapper<TDto?>>
{
    public CQRSControllerFeatrueBased(
        ILocalizationService localizationService,
        ILogger<CQRSControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand, TCreateCommand/*, TUpdateCommand*/>> logger,
        IMediator mediator,
        IServiceBase<TDto, TEn> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, mapper, mapster)
    {
    }

    public delegate ValueTask<TDto> OnCreateEventHandler<T>(TCreateCommand q);
    public event OnCreateEventHandler<TDeleteCommand>? OnCreate;

    //public delegate ValueTask<TDto> OnUpdateEventHandler<T>(TUpdateCommand q);
    //public event OnUpdateEventHandler<TDeleteCommand>? OnUpdate;

    //[HttpPost("Create")]
    //public ValueTask<HttpResponseWrapper<TDto?>> PostCreateAsync(TCreateCommand command)
    //=>
    //    TryOne<TDto?>(async (opt) =>
    //    {
    //        return await Mediator.Send(command);
    //    });

    //[HttpPost("Update")]
    //public ValueTask<HttpResponseWrapper<TDto?>> PostUpdateAsync(TUpdateCommand command)
    //=>
    //    TryOne<TDto?>(async (opt) =>
    //    {
    //        return await Mediator.Send(command);
    //    });


    //[HttpGet("{Id}")]
    //[NonAction]
    //public virtual ValueTask<HttpResponseWrapper<TDto?>> GetOneAsync(Guid Id)
    //=>
    //    TryOne<TDto?>(async (opt) =>
    //    {
    //        return await Mediator.Send(Id);
    //    });
    // todo: check auth
    [AllowAnonymous]
    [HttpGet]
    public virtual ValueTask<HttpResponseWrapperList<TDto>> GetAllByQueryAsync([FromQuery] TQuery getAllQuery)
    =>
        TryMeny<TDto>(async (opt) =>
        {
            return await Mediator.Send(getAllQuery);
        });



    //[NonAction]
    [HttpPost("Delete")]
    public ValueTask<HttpResponseWrapper<TDto?>> DeleteRemoveAsync(TDeleteCommand command)
    =>
        TryOne<TDto>(async (opt) =>
        {
            return await Mediator.Send(command);
        });


    [HttpPost]
    public ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync(TCreateCommand command)
    =>
        TryOne<TDto?>(async (opt) =>
        {
            return await Mediator.Send(command);
        });


    [HttpGet("{Id}")]
    public ValueTask<HttpResponseWrapper<TDto?>> GetOneAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}
