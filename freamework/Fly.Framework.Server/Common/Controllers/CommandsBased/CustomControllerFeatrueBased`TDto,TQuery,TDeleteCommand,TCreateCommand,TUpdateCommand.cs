using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Framework.Server.Common.Controllers.CommandsBased;

public class CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand, TCreateCommand, TUpdateCommand>
    : CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand>,
    IFeatureBase<TDto, TQuery, TDeleteCommand, TCreateCommand, TUpdateCommand>
    where TDto : DtoBase
    where TEn : TEntity
    where TQuery : QueryRequestBase
    where TDeleteCommand : DeleteRequestBase, new()
    where TCreateCommand : CreateRequestBase
    where TUpdateCommand : UpdateRequestBase
{
    public CustomControllerFeatrueBased(
        ILocalizationService localizationService,
        ILogger<CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand, TCreateCommand, TUpdateCommand>> logger,
        IMediator mediator,
        IServiceBase<TDto, TEn> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
    }

    public delegate ValueTask<TDto> OnCreateEventHandler<T>(TCreateCommand q);
    public event OnCreateEventHandler<TDeleteCommand>? OnCreate;

    public delegate ValueTask<TDto> OnUpdateEventHandler<T>(TUpdateCommand q);
    public event OnUpdateEventHandler<TDeleteCommand>? OnUpdate;

    [HttpPost("Create")]
    public ValueTask<HttpResponseWrapper<TDto?>> PostCreateAsync(TCreateCommand command)
    =>
        TryOne<TDto?>(async (opt) =>
        {
            if (OnCreate is null)
            {
                var dto = command.Adapt<TDto>();
                var newDto = await Service.Add(dto);
                if (newDto is null)
                {
                    var type = typeof(TDto).Name;
                    var id = Request.HttpContext.TraceIdentifier;
                    Logger.LogError("error with {id} | Error in Add new entity for type {type}\n\robject : [{dto}]", id, type, dto.ToString());
                    opt.Problem("error");
                }
                return opt.Success(dto);
            }
            else
            {
                var r = await OnCreate.Invoke(command);
                return opt.Success(r);
            }
        });

    [HttpPost("Update")]
    public ValueTask<HttpResponseWrapper<TDto?>> PostUpdateAsync(TUpdateCommand command)
    =>
        TryOne<TDto?>(async (opt) =>
        {
            if (OnUpdate is null)
            {
                var dto = command.Adapt<TDto>();
                var updatedDto = await Service.Update(dto);
                if (updatedDto is null)
                {
                    var type = typeof(TDto).Name;
                    var id = Request.HttpContext.TraceIdentifier;
                    Logger.LogError("error with {id} | Error in Add new entity for type {type}\n\robject : [{dto}]", id, type, dto.ToString());
                    opt.Problem("error");
                }
                return opt.Success(dto);
            }
            else
            {
                var d = await OnUpdate.Invoke(command);
                return opt.Success(d);
            }
        });
}