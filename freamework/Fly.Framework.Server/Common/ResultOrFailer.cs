using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Ardalis.Result;

namespace Fly.Framework.Server.Common;

public class ResultOrFailer<T>
{

    public ResultOrFailer(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }

    public TService GetService<TService>()
        where TService : class
    {
        return ServiceProvider.GetRequiredService<TService>();
    }

    public IValidator<TRequest>? GetValidation<TRequest>()
        where TRequest : class
    {
        return ServiceProvider.GetService<IValidator<TRequest>>();
    }

    public async Task<ValidationResult?> Validate<TRequest>(TRequest request)
        where TRequest : class
    {
        var validator = ServiceProvider.GetService<IValidator<TRequest>>();
        var validationStatus = await validator.ValidateAsync(request);
        return validationStatus;
    }

    public IServiceBase<TDto, TEnt> GetService<TDto, TEnt>()
        where TDto : DtoBase
        where TEnt : TEntity
    {
        return ServiceProvider.GetRequiredService<IServiceBase<TDto, TEnt>>();
    }

    #region Problem

    public HttpResponseWrapper<T> Problem(Exception exception)
    {
        return HttpResponseWrapper<T>.Problem(exception);
    }
    public HttpResponseWrapper<T> Problem(List<ValidationError> errors)
    {
        var errosString = errors.Select(s => s.ErrorMessage).ToArray();
        return HttpResponseWrapper<T>.Problem(string.Join("\n\r", errosString));
    }
    public HttpResponseWrapper<T> Problem(HttpStatusCode statusCodes, string message)
    {
        return HttpResponseWrapper<T>.Problem((int)statusCodes, message);
    }
    public HttpResponseWrapper<T> Problem(string message)
    {
        return HttpResponseWrapper<T>.Problem(message);
    }
    public HttpResponseWrapper<T> Problem(List<ValidationFailure> errors)
    {
        var erro = string.Join("\n", errors.Select((s, indx) => "\r" + (indx + 1) + ") " + s.ErrorMessage));
        return Problem(erro);
    }

    #endregion

    public HttpResponseWrapper<T> Success(T data)
    {
        return HttpResponseWrapper<T>.Success(data);
    }
}
public class ResultListOrFailer<T>
{
    public ResultListOrFailer(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public IServiceProvider ServiceProvider { get; }

    public IServiceBase<TDto, TEnt> MakeChildService<TDto, TEnt>()
        where TDto : DtoBase
        where TEnt : TEntity
    {
        return ServiceProvider.GetRequiredService<IServiceBase<TDto, TEnt>>();
    }

    public TService GetService<TService>()
        where TService : class
    {
        return ServiceProvider.GetRequiredService<TService>();
    }

    public IValidator<TRequest>? GetValidation<TRequest>()
        where TRequest : class
    {
        return ServiceProvider.GetService<IValidator<TRequest>>();
    }

    public IServiceBase<TDto, TEnt> GetService<TDto, TEnt>()
        where TDto : DtoBase
        where TEnt : TEntity
    {
        return ServiceProvider.GetRequiredService<IServiceBase<TDto, TEnt>>();
    }

    #region Problem

    public HttpResponseWrapperList<T> Problem(string message)
    {
        return HttpResponseWrapperList<T>.Problem(message);
    }
    public HttpResponseWrapperList<T> Problem(Exception exception)
    {
        return HttpResponseWrapperList<T>.Problem(exception);
    }
    public HttpResponseWrapperList<T> Problem(List<ValidationFailure> errors)
    {
        var erro = string.Join("\n", errors.Select((s, indx) => "\r" + (indx + 1) + ") " + s.ErrorMessage));
        return Problem(erro);
    }
    public HttpResponseWrapperList<T> Problem(HttpStatusCode statusCodes, string message)
    {
        return HttpResponseWrapperList<T>.Problem((int)statusCodes, message);
    }

    #endregion

    #region Success

    public HttpResponseWrapperList<T> Success(PageList<T> data)
    {
        return HttpResponseWrapperList<T>.Success(data);
    }
    public HttpResponseWrapperList<T> Success(IEnumerable<T> data)
    {
        return HttpResponseWrapperList<T>.Success(data);
    }

    #endregion

    #region Problem

    public HttpResponseWrapperList<T> ShowMessage(string message)
    {
        return HttpResponseWrapperList<T>.ShowMessage(message);
    }
    #endregion

}