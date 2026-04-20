using Ardalis.Result;
using Fly.Framework.Shared.Common.Http;

namespace Fly.Framework.Server.Exctentions;

public static class HttpResponseWrapperExctentions
{
    public static HttpResponseWrapper<T> Problem<T>(this HttpResponseWrapper<T> httpResponseWrapper, List<ValidationError> errors)
    {
        var errorString = string.Join(", ", errors.Select(x => x.ErrorMessage));
        return new HttpResponseWrapper<T>
        {
            Data = default,
            Status = false,
            StatusCode = 500,
            ErrorMessage = errorString,
        };
    }
}