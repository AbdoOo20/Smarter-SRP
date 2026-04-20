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

/// <summary>
/// TCreateOrUpdateCommand
/// </summary>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TEn"></typeparam>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TDeleteCommand"></typeparam>
/// <typeparam name="TCreateOrUpdateCommand"></typeparam>

///
public class CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand, TCreateOrUpdateCommand>

    : CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand>,
    IFeatureBase<TDto, TQuery, TDeleteCommand, TCreateOrUpdateCommand>
    where TDto : DtoBase
    where TEn : TEntity
    where TQuery : QueryRequestBase
    where TDeleteCommand : DeleteRequestBase, new()
    where TCreateOrUpdateCommand : CreateUpdateRequestBase
{
    public CustomControllerFeatrueBased(
        ILocalizationService localizationService,
        ILogger<CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand, TCreateOrUpdateCommand>> logger,
        IMediator mediator,
        IServiceBase<TDto, TEn> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
    }

    public delegate ValueTask<TDto> OnCreateUpdateEventHandler<T>(TCreateOrUpdateCommand q);
    public event OnCreateUpdateEventHandler<TDeleteCommand>? OnCreateUpdate;

    public delegate ValueTask<HttpResponseWrapper<TDto?>> OnCreateUpdateEventHandlerEx<T>(TCreateOrUpdateCommand q, ResultOrFailer<TDto?> opt);
    public event OnCreateUpdateEventHandlerEx<TCreateOrUpdateCommand>? OnCreateUpdateEx;

    [NonAction]
    protected virtual ValueTask BeforAdd(TDto dto, TCreateOrUpdateCommand command, TEn? entity) => ValueTask.CompletedTask;


    [NonAction]
    public virtual async ValueTask<HttpResponseWrapper<TDto?>?> ValidateCreateUpdateAction(TCreateOrUpdateCommand command, ResultOrFailer<TDto?> opt)
    {
        await ValueTask.CompletedTask;
        return null;
    }
    
    /*[HttpPost("batch")]
    public ValueTask<HttpResponseWrapper<string>> PostSaveBatchAsync([FromBody] List<TCreateOrUpdateCommand> command)
    =>
    TryOne<string>(async (opt) =>
        {
            foreach (var item in command)
            {
                await PostSaveAsync(item);
            }

            return opt.Success("500");
        });*/
    
    [HttpPost]
    public ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync([FromBody] TCreateOrUpdateCommand command)
    =>
    TryOne<TDto?>(async (opt) =>
        {
            var validator = opt.GetValidation<TCreateOrUpdateCommand>();
            if (validator is not null)
            {
                var validationStatus = await validator.ValidateAsync(command);

                if (!validationStatus.IsValid)
                {
                    return opt.Problem(validationStatus.Errors);
                }
            }
            var otherValidator = await ValidateCreateUpdateAction(command, opt);
            if (otherValidator is not null)
            {
                return otherValidator;
            }

            if (OnCreateUpdateEx is null && OnCreateUpdate is not null)
            {
                var dto = await OnCreateUpdate.Invoke(command);
                return opt.Success(dto);
            }
            else if (OnCreateUpdateEx is not null && OnCreateUpdate is null)
            {
                var result = await OnCreateUpdateEx.Invoke(command, opt);
                return result;
            }
            else
            {
                
                //var dto = Mapper.Map<TDto>(command);
                if (command.Id.IsNullOrEmpty())
                {
                    var dto = command.Adapt<TDto>();
                    await BeforAdd(dto, command, null);
                    await Service.Add(dto);
                    return opt.Success(dto);
                }
                else
                {
                    var db = await Service.GetById(command.Id);
                    var raw = await Service.GetTById(command.Id);
                    if (db == null) return opt.Problem("no user exists");  // TODO: localize
                                                                                       //Mapper.Map(command, db);
                    command.Adapt(db);
                    await BeforAdd(db, command, raw);
                    var status = await Service.Update(db);
                    if (status is null) return opt.Problem("Cannot process your request"); // TODO: localize
                    return opt.Success(db);
                }
            }
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    return opt.Problem(ex.Message);
            //}
        });
}