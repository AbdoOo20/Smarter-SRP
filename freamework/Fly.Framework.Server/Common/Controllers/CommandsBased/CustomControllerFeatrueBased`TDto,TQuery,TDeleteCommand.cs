using Ardalis.Result.FluentValidation;
using FluentValidation;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Framework.Server.Common.Controllers.CommandsBased;

/// <summary>
/// Delete oprations
/// </summary>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TEn"></typeparam>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TDeleteCommand"></typeparam>
public class CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand>
    : CustomControllerFeatrueBased<TDto, TEn, TQuery>,
    IFeatureBase<TDto, TQuery, TDeleteCommand>
    where TDto : DtoBase
    where TEn : TEntity
    where TQuery : QueryRequestBase
    where TDeleteCommand : DeleteRequestBase, new()
{
    protected IServiceBase<TDto, TEn> _service;
    public CustomControllerFeatrueBased(
        ILocalizationService localizationService,
        ILogger<CustomControllerFeatrueBased<TDto, TEn, TQuery, TDeleteCommand>> logger,
        IMediator mediator,
        IServiceBase<TDto, TEn> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;
    }

    public event OnDeleteEventHandler<TDeleteCommand>? OnDelete;
    public delegate ValueTask<TDto> OnDeleteEventHandler<T>(TDeleteCommand q);


    public event OnDeleteHandlerEx<TDeleteCommand>? OnDeleteEx;
    public delegate ValueTask<HttpResponseWrapper<TDto?>> OnDeleteHandlerEx<T>(TDeleteCommand q, ResultOrFailer<TDto?> opt);


    public async ValueTask<HttpResponseWrapper<TDto?>?> DeleteValidate<T>(T validationRules, TDeleteCommand delete, ResultOrFailer<TDto?> opt)
        where T : AbstractValidator<TDeleteCommand>
    {
        var validationStatusinfo = await validationRules.ValidateAsync(delete);
        if (!validationStatusinfo.IsValid) return opt.Problem(validationStatusinfo.AsErrors());
        return null;
    }

    [NonAction]
    public virtual async ValueTask<HttpResponseWrapper<TDto?>?> ValidateDeleteAction(TDeleteCommand command, ResultOrFailer<TDto?> opt)
    {
        await ValueTask.CompletedTask;
        return null;
    }


    [NonAction]
    protected virtual ValueTask AfterDelete(TDto dto, TDeleteCommand command, ResultOrFailer<TDto?> opt) => ValueTask.CompletedTask;



    //[HttpPost("delete/{guid}")]
    //[NonAction] // works
    [HttpDelete("{id}")]
    public ValueTask<HttpResponseWrapper<TDto?>> DeleteRemoveAsync([FromRoute] Identifier id)
    =>
        TryOne<TDto?>(async (opt) =>
        {
            var command = new TDeleteCommand() { Id = id };
            var validator = opt.GetValidation<TDeleteCommand>();
            if (validator is not null)
            {
                var validationStatus = await validator.ValidateAsync(command);

                if (!validationStatus.IsValid)
                {
                    return opt.Problem(validationStatus.Errors);
                }
            }
            var otherValidator = await ValidateDeleteAction(command, opt);
            if (otherValidator is not null)
            {
                return otherValidator;
            }

            //  ----------------------
            if (OnDeleteEx is not null)
            {
                var dto = await OnDeleteEx.Invoke(command, opt);
                return dto;
            }
            else if (OnDelete is not null)
            {
                var dto = await OnDelete.Invoke(command);
                return opt.Success(dto);
            }
            else
            {
                var dbObj = await Service.GetById(id);
                if (dbObj is null) return opt.Problem(new Exception("target not found"));
                var dto = await Service.Remove(dbObj);
                await AfterDelete(dto, command, opt);
                return opt.Success(dto);
            }
        });

    [NonAction]
    //[HttpDelete]
    public ValueTask<HttpResponseWrapper<TDto?>> DeleteRemoveAsync(TDeleteCommand guid)
    =>
        TryOne<TDto?>(async (opt) =>
        {
            if (OnDelete is null)
            {
                var dbObj = await Service.AnyAsync(w => guid.Id == w.Id);
                if (dbObj) return opt.Problem(new Exception("target not found"));
                var dto = await Service.Remove(guid.Id);
                return opt.Success(dto);
            }
            else
            {
                var d = await OnDelete.Invoke(guid);
                return opt.Success(d);
                throw new NotImplementedException();
            }
        });

}