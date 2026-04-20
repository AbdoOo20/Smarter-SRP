using System.Text.Json.Serialization;
using Fly.Framework.Shared.Common.Http.Abstractions;
using Fly.Framework.Shared.Exceptions;

namespace Fly.Framework.Shared.Common.Http;

public class HttpResponseWrapper<T> : /*PageList<T>,*/ IHttpResponseWrapper<T>
{
    public T? Data { get; set; }

    public bool Status { get; set; }

    [JsonIgnore] public Exception? Exception { get; set; }

    [JsonIgnore] public string? Error => Exception?.Message;

    public string? ErrorMessage { get; set; }
    
    public int StatusCode { get; set; }

    public void SuccessOrException()
    {
        if (!Status) throw new CallAdminException("Responce not success");
        if (Data is null) throw new CallAdminException("Responce dose not have data");
    }

    public T GetDataOrException()
    {
        SuccessOrException();
        return Data!;
    }

    public static HttpResponseWrapper<T> Success(T data)
    {
        return new HttpResponseWrapper<T>
        {
            Data = data,
            Status = true,
            StatusCode = 200,
        };
    }

    public static HttpResponseWrapper<T> Moc( /*T data = default*/)
    {
        return new HttpResponseWrapper<T>
        {
            Data = Activator.CreateInstance<T>(),
            Status = true
        };
    }

    public static HttpResponseWrapper<T> Problem(Exception exception)
    {
        return new HttpResponseWrapper<T>
        {
            Data = default,
            Status = false,
            StatusCode = 500,
            Exception = exception,
            ErrorMessage = exception.Message,
        };
    }
    public static HttpResponseWrapper<T> Problem(string message)
    {
        return new HttpResponseWrapper<T>
        {
            Data = default,
            Status = false,
            StatusCode = 500,
            ErrorMessage = message,
        };
    }

    public static HttpResponseWrapper<T> Problem(int statusCodes, string message)
    {
        return new HttpResponseWrapper<T>
        {
            Data = default,
            Status = false,
            ErrorMessage = message,
            StatusCode = statusCodes,
        };
    }
}